using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using SocialNetworkBE.Services.JsonWebToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Testing {
    [TestClass]
    public class JsonWebTokenServiceTest {
        private readonly JsonWebTokenService TokenService = new JsonWebTokenService();

        private TokenClaimsModel TokenClaimsModelToTest() {
            string email = "usertest@gmail.com";
            string displayName = "User Test";
            string role = "user";
            string objectId = ObjectId.GenerateNewId().ToString();
            string username = "usertest";
            string userProfileURL = "/usertest";
            string userAvatar = "https://avatar/avatar.png";
            string hash = Guid.NewGuid().ToString();
            return new TokenClaimsModel() {
                AvatarURL = userAvatar,
                DisplayName = displayName,
                Email = email,
                Role = role,
                Username = username,
                KeyPairHash = hash,
                ObjectId = objectId,
                UserProfileURL = userProfileURL,
            };
        }
        [TestMethod]
        public void GivenTokenClaimsModel_WhenCreateClaimIdentityForAccessToken_ThenReturnRightClaimsIdentity() {
            TokenClaimsModel tokenClaimsModel = TokenClaimsModelToTest();
            ClaimsIdentity claimsIdentity = TokenService.CreateClaimsIdentityFromModel(tokenClaimsModel, TokenService.AccessToken);

            Claim emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email);
            Assert.IsNotNull(emailClaim);
            Assert.IsTrue(emailClaim.Value.Equals(tokenClaimsModel.Email));

            Claim displayNameClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
            Assert.IsNotNull(displayNameClaim);
            Assert.IsTrue(displayNameClaim.Value.Equals(tokenClaimsModel.DisplayName));

            Claim roleClaim = claimsIdentity.FindFirst(ClaimTypes.Role);
            Assert.IsNotNull(roleClaim);
            Assert.IsTrue(roleClaim.Value.Equals(tokenClaimsModel.Role));

            Claim objectIdClaim = claimsIdentity.FindFirst(ClaimTypes.Sid);
            Assert.IsNotNull(objectIdClaim);
            Assert.IsTrue(objectIdClaim.Value.Equals(tokenClaimsModel.ObjectId));

            Claim usernameClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Assert.IsNotNull(usernameClaim);
            Assert.IsTrue(usernameClaim.Value.Equals(tokenClaimsModel.Username));

            Claim userProfileURLClaim = claimsIdentity.FindFirst(ClaimTypes.Webpage);
            Assert.IsNotNull(userProfileURLClaim);
            Assert.IsTrue(userProfileURLClaim.Value.Equals(tokenClaimsModel.UserProfileURL));

            Claim userAvatarURLClaim = claimsIdentity.FindFirst(ClaimTypes.UserData);
            Assert.IsNotNull(userAvatarURLClaim);
            Assert.IsTrue(userAvatarURLClaim.Value.Equals(tokenClaimsModel.AvatarURL));

            Claim hashClaim = claimsIdentity.FindFirst(ClaimTypes.Hash);
            Assert.IsNotNull(hashClaim);
            Assert.IsTrue(hashClaim.Value.Equals(tokenClaimsModel.KeyPairHash));
        }

        [TestMethod]
        public void GivenClaimIdentityAndExpriseTime_WhenCreateAccessTokenFromClaims_ThenReturnRightToken() {
            TokenClaimsModel tokenClaimsModel = TokenClaimsModelToTest();
            ClaimsIdentity claimsIdentity = TokenService.CreateClaimsIdentityFromModel(tokenClaimsModel, TokenService.AccessToken);

            string accessToken = TokenService.CreateTokenFromClaims(claimsIdentity, TokenService.accessTokenExpriseTime);
            Assert.IsNotNull(accessToken);


            ClaimsIdentity claimsIdentityFromToken = TokenService.GetClaimsIdentityFromToken(accessToken);
            Assert.IsNotNull(claimsIdentityFromToken);

            Claim emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email);
            Assert.IsNotNull(emailClaim);
            Assert.IsTrue(emailClaim.Value.Equals(tokenClaimsModel.Email));

            Claim displayNameClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
            Assert.IsNotNull(displayNameClaim);
            Assert.IsTrue(displayNameClaim.Value.Equals(tokenClaimsModel.DisplayName));

            Claim roleClaim = claimsIdentity.FindFirst(ClaimTypes.Role);
            Assert.IsNotNull(roleClaim);
            Assert.IsTrue(roleClaim.Value.Equals(tokenClaimsModel.Role));

            Claim objectIdClaim = claimsIdentity.FindFirst(ClaimTypes.Sid);
            Assert.IsNotNull(objectIdClaim);
            Assert.IsTrue(objectIdClaim.Value.Equals(tokenClaimsModel.ObjectId));

            Claim usernameClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Assert.IsNotNull(usernameClaim);
            Assert.IsTrue(usernameClaim.Value.Equals(tokenClaimsModel.Username));

            Claim userProfileURLClaim = claimsIdentity.FindFirst(ClaimTypes.Webpage);
            Assert.IsNotNull(userProfileURLClaim);
            Assert.IsTrue(userProfileURLClaim.Value.Equals(tokenClaimsModel.UserProfileURL));

            Claim userAvatarURLClaim = claimsIdentity.FindFirst(ClaimTypes.UserData);
            Assert.IsNotNull(userAvatarURLClaim);
            Assert.IsTrue(userAvatarURLClaim.Value.Equals(tokenClaimsModel.AvatarURL));

            Claim hashClaim = claimsIdentity.FindFirst(ClaimTypes.Hash);
            Assert.IsNotNull(hashClaim);
            Assert.IsTrue(hashClaim.Value.Equals(tokenClaimsModel.KeyPairHash));
        }
    }
}