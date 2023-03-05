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

        public static string ApiKey = "AIzaSyCBeCehqqIvzNRMP1eGYOcu5rYrWpA7xDQ";
        public static string Bucket = "socialnetwork-4c654.appspot.com";
        public static string AuthEmail = "kkneee0201hihi@gmail.com";
        public static string AuthPassword = "kimkhanh21";
        public static async Task UploadImage(string img)
        {
            var stream = File.Open(img, FileMode.Open);

            //authentication
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            var task = new FirebaseStorage(
                Bucket,

                 new FirebaseStorageOptions
                 {
                     AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                     ThrowOnCancel = true,
                 })
                .Child("AvatarUrl")
                .Child("kk.jpg")
                .PutAsync(stream);
            task.Progress.ProgressChanged += (s, e) => System.Diagnostics.Debug.WriteLine($"Progress: {e.Percentage} %");

            try
            {
                // error during upload will be thrown when you await the task
                System.Diagnostics.Debug.WriteLine("Download link:\n" + await task);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception was thrown: {0}", ex);
            }
        }

        

        public static async Task getUrl()
        {
            FirebaseStorage storage = new FirebaseStorage("socialnetwork-4c654.appspot.com");
            var starsRef = storage.Child("AvatarUrl").Child("kk.jpg");
            string link = await starsRef.GetDownloadUrlAsync();
            System.Diagnostics.Debug.WriteLine(link);
        }

        

        public static void UploadFoto()
        {
            FileStream streamObj = System.IO.File.OpenRead(@"C:\anh\kk.jpg");
            Byte[] imgByte = null;
            imgByte = lnkUpload(streamObj);
            System.Diagnostics.Debug.WriteLine(imgByte);
        }


        private static Byte[] lnkUpload(FileStream img)
        {
            byte[] resizedImage;
            using (Image orginalImage = Image.FromStream(img))
            {
                ImageFormat orginalImageFormat = orginalImage.RawFormat;
                int orginalImageWidth = orginalImage.Width;
                int orginalImageHeight = orginalImage.Height;
                int resizedImageWidth = 60; // Type here the width you want
                int resizedImageHeight = Convert.ToInt32(resizedImageWidth * orginalImageHeight / orginalImageWidth);
                using (Bitmap bitmapResized = new Bitmap(orginalImage, resizedImageWidth, resizedImageHeight))
                {
                    using (MemoryStream streamResized = new MemoryStream())
                    {
                        bitmapResized.Save(streamResized, orginalImageFormat);
                        resizedImage = streamResized.ToArray();
                    }
                }
            }
            return resizedImage;
        }
        public static Image Resize2Max50Kbytes()
        {
            var fileStream = File.Open("C:/anh/kk.jpg", FileMode.Open);
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

    }

}