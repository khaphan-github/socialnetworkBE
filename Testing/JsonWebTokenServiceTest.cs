using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialNetworkBE.Services.JsonWebToken;
using System.Collections.Generic;
using System.Security.Claims;

namespace Testing {
    [TestClass]
    public class JsonWebTokenServiceTest {
        [TestMethod]
        public void TestGenerateTokenWhenValidateThenIsValidToken() {
            // Given
            JsonWebTokenService jsonWebTokenService = new JsonWebTokenService();

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, "ThisIsUsername"));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, "ThisIsEmail"));

            string token = jsonWebTokenService.CreateTokenFromUserData(claimsIdentity, 120);

            System.Diagnostics.Debug.WriteLine("Json Web Token: " + token);

            bool isValidToken = jsonWebTokenService.IsValidToken(token);

            Assert.IsTrue(isValidToken);
        }

        [TestMethod]
        public void GivenInvalidTokenWhenValidateTokenThenReturnFalse() {
            string invalidToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6" +
                "IlRoaXNJc1VzZXJuYW1lIiwiZW1haWwiOiJUaGlzSXNFbWFpbCIsIm5iZiI6MTY3NzA2MTYzOSw" +
                "iZXhwIjoxNjc3MDY4ODM5LCJpYXQiOjE2NzcwNjE2Mzl9.XIIehdOemwWh3S1E-k0Jjbb_3Gdeb" +
                "yoiwYvkentnxS6NiKEyFvdLC9XaSESiKaYK6irTstJMPlXkdU0gmoctDw";

            JsonWebTokenService jsonWebTokenService = new JsonWebTokenService();

            bool isValidToken = jsonWebTokenService.IsValidToken(invalidToken);

            Assert.IsTrue(!isValidToken);
        }

        [TestMethod]
        public void GivenUserDataWhenGenerateTokenKeyPairAndValidateThenRequrnTrue() {
            JsonWebTokenService jsonWebTokenService = new JsonWebTokenService();
            ClaimsIdentity claimsIdentity = jsonWebTokenService.CreateClaimsIdentity(
                "thisisusername",
                "thisisemail",
                "user");

            List<string> tokenkeyPair = jsonWebTokenService.GenerateKeyPairs(claimsIdentity);

            bool isValidAccessToken = jsonWebTokenService.IsValidToken(tokenkeyPair[0]);
            bool isValidRefreshToken = jsonWebTokenService.IsValidToken(tokenkeyPair[1]);

            Assert.IsTrue(isValidAccessToken);
            Assert.IsTrue(isValidRefreshToken);
        }
    }
}
