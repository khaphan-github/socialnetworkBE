using Firebase.Auth;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using SocialNetworkBE.ServerConfiguration;
using Firebase.Storage;
using System.Threading;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace SocialNetworkBE.Services.Firebase {
    public class FirebaseImage {

        private string APIKey { get; set; }
        private string UserEmail { get; set; }
        private string Password { get; set; }
        private string Bucket { get; set; }
        public string StorageDomain { get; set; }
        private FirebaseAuthLink FirebaseAuthLink { get; set; }
        public FirebaseImage() {
            APIKey = ServerEnvironment.GetFirebaseApiKey();
            UserEmail = ServerEnvironment.GetFirebaseAuthEmail();
            Password = ServerEnvironment.GetFirebaseAuthPwd();
            Bucket = ServerEnvironment.GetFirebaseBucket();
            StorageDomain = ServerEnvironment.GetFirebaseStorageDomain();
        }

        private async Task<FirebaseAuthLink> ExecuteWithPostContentAsync(string googleUrl, string postContent) {
            string responseData = "N/A";
            try {
                HttpClient client = new HttpClient();

                HttpResponseMessage response =
                    await client.PostAsync(
                        new Uri(string.Format(googleUrl, new object[1] { APIKey })),
                        new StringContent(postContent, Encoding.UTF8,
                        "application/json"));

                responseData = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();

                FirebaseAuthLink firebaseAuthLink = JsonConvert.DeserializeObject<FirebaseAuthLink>(responseData);
                return firebaseAuthLink;
            } catch (Exception innerException) {
                throw new FirebaseAuthException(googleUrl, postContent, responseData, innerException);
            }
        }


        public async Task<FirebaseAuthLink> SignInWithEmailAndPasswordAsync(string email, string password) {
            string contentFormat = "{{\"email\":\"{0}\",\"password\":\"{1}\",\"returnSecureToken\":false}}";
            string postContent = string.Format(contentFormat, new object[2] { email, password });

            string googleApisEndpoint = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key={0}";
            
            return await ExecuteWithPostContentAsync(googleApisEndpoint, postContent)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string folder, string mediaName) {

            if(FirebaseAuthLink == null) {
                FirebaseAuthLink = await SignInWithEmailAndPasswordAsync(UserEmail, Password);
            }

            CancellationToken cancellationToken = new CancellationToken();

            FirebaseStorageTask pushMediaTask = new FirebaseStorage(
                Bucket,
                 new FirebaseStorageOptions {
                     AuthTokenAsyncFactory =
                        () => Task.FromResult(FirebaseAuthLink.FirebaseToken),
                     ThrowOnCancel = true,
                 })
                .Child(folder)
                .Child(mediaName)
                .PutAsync(fileStream, cancellationToken);

            return await pushMediaTask;
        }


        public Image Resize2Max50Kbytes(FileStream fileStream) {
            System.Diagnostics.Debug.WriteLine(fileStream.GetType());

            var memoryStream = new MemoryStream();

            fileStream.CopyTo(memoryStream);

            byte[] byteArray = memoryStream.ToArray();
            System.Diagnostics.Debug.WriteLine("before: " + byteArray.Length);

            byte[] currentByteImageArray = byteArray;
            double scale = 1f;

            MemoryStream inputMemoryStream = new MemoryStream(byteArray);
            Image img = Image.FromStream(inputMemoryStream);
            Image fullsizeImage = Image.FromStream(inputMemoryStream);

            while (currentByteImageArray.Length > 40000) {
                Bitmap fullSizeBitmap = new Bitmap(fullsizeImage, new Size((int)(fullsizeImage.Width * scale), (int)(fullsizeImage.Height * scale)));
                MemoryStream resultStream = new MemoryStream();

                fullSizeBitmap.Save(resultStream, fullsizeImage.RawFormat);

                currentByteImageArray = resultStream.ToArray();
                resultStream.Dispose();
                resultStream.Close();
                scale -= 0.05f;
            }
            return img;
        }
    }

}