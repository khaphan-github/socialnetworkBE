using Firebase.Auth;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using SocialNetworkBE.ServerConfiguration;
using Firebase.Storage;

namespace SocialNetworkBE.Services.Firebase {
    public class FirebaseImage {
        public async Task<string> UploadImage(FileStream fileStream, string folder, string name, string fileFormat) {
            Image imageResize = Resize2Max50Kbytes(fileStream);
            MemoryStream stream = new MemoryStream();

            imageResize.Save(stream, ImageFormat.Jpeg);

            var auth = new FirebaseAuthProvider(new FirebaseConfig(ServerEnvironment.GetFirebaseApiKey()));
            var a = await auth
                .SignInWithEmailAndPasswordAsync(
                    ServerEnvironment.GetFirebaseAuthEmail(),
                    ServerEnvironment.GetFirebaseAuthPwd()
                );

            var task = new FirebaseStorage(
                 ServerEnvironment.GetFirebaseBucket(),
                 new FirebaseStorageOptions {
                     AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                     ThrowOnCancel = true,
                 })
                .Child(folder)
                .Child(name + fileFormat)
                .PutAsync(stream);

            return await task;
        }



        public async Task getUrl(string namePic) {
            FirebaseStorage storage = new FirebaseStorage("socialnetwork-4c654.appspot.com");
            var starsRef = storage.Child("AvatarUrl").Child(namePic);
            string link = await starsRef.GetDownloadUrlAsync();
            System.Diagnostics.Debug.WriteLine(link);
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