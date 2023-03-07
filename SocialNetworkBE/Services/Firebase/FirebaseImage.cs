using Firebase.Auth;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Firebase.Storage;
using MongoDB.Driver.Core.Misc;
using System.Collections.Generic;
using FireSharp;
using Firebase.Database;
using System.Drawing;
using Firebase;
using FirebaseClient = Firebase.Database.FirebaseClient;
using System.Windows.Forms;
using System.Web;
using System.Drawing.Imaging;

namespace SocialNetworkBE.Services.Firebase
{
    public class FirebaseImage
    {

        public static async Task UploadImage(FileStream fileStream)
        {
            var imgInput = Resize2Max50Kbytes(fileStream);
            
            var stream = new MemoryStream();
            imgInput.Save(stream, ImageFormat.Jpeg);

            var auth = new FirebaseAuthProvider(new FirebaseConfig(ServerEnvironment.GetFirebaseApiKey());
            var a = await auth.SignInWithEmailAndPasswordAsync(ServerEnvironment.GetFirebaseAuthEmail(), ServerEnvironment.GetFirebaseAuthPwd());

            var task = new FirebaseStorage(
                ServerEnvironment.GetFirebaseBucket(),

                 new FirebaseStorageOptions
                 {
                     AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                     ThrowOnCancel = true,
                 })
                .Child("AvatarUrl")
                .Child("nameAcc.jpg")
                .PutAsync(stream);
            task.Progress.ProgressChanged += (s, e) => System.Diagnostics.Debug.WriteLine($"Progress: {e.Percentage} %");

            try
            {
                System.Diagnostics.Debug.WriteLine("Link ảnh:\n" + await task);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi: {0}", ex);
            }
        }

        

        public static async Task getUrl(string namePic)
        {
            FirebaseStorage storage = new FirebaseStorage("socialnetwork-4c654.appspot.com");
            var starsRef = storage.Child("AvatarUrl").Child(namePic);
            string link = await starsRef.GetDownloadUrlAsync();
            System.Diagnostics.Debug.WriteLine(link);
        }

        public static Image Resize2Max50Kbytes(FileStream fileStream)
        {
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

            while (currentByteImageArray.Length > 40000)
            {
                Bitmap fullSizeBitmap = new Bitmap(fullsizeImage, new Size((int)(fullsizeImage.Width * scale), (int)(fullsizeImage.Height * scale)));
                MemoryStream resultStream = new MemoryStream();

                fullSizeBitmap.Save(resultStream, fullsizeImage.RawFormat);

                currentByteImageArray = resultStream.ToArray();
                resultStream.Dispose();
                resultStream.Close();

                scale -= 0.05f;
            }

            System.Diagnostics.Debug.WriteLine("after: "+ currentByteImageArray.Length);
            using (var ms = new MemoryStream(currentByteImageArray))
            { 
                var imgResult =  Image.FromStream(ms);
                imgResult.Save("C:/anh/kkne.jpg");
               
                return img;
            }

        }

        public string urlimage (File stream) {
            /// 
            return urlimage();
        }

    }

}