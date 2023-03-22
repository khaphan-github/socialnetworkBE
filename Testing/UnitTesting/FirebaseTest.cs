using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using SocialNetworkBE.Services.Firebase;

namespace Testing.UnitTesting {
    [TestClass]
    public class FirebaseTest {
        [TestMethod]
        public async Task UploadImageTest() {
            FirebaseService firebaseImage = new FirebaseService();
            // Arrange
            string folder = "PostMedia";
            string name = "hehehe3444he";
            string fileFormat = ".png";
            var stream = File.Open(@"E:\SCHOOL\web\socialnetworkBE\Testing\TestData\curved-arrow-black-png-02.png", FileMode.Open);
            // Act
            var result = await firebaseImage.UploadMedia(stream, folder, name, fileFormat);

            Assert.IsNotNull(result);
            // Assert
        }






    }
}
