﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using SocialNetworkBE.Controllers;
using SocialNetworkBE.Payload.Request;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Data;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.ServerConfiguration;
using SocialNetworkBE.Services.Hash;

namespace Testing.InterationTesting {
    [TestClass]
    public class AuthEndpointTesting {
        [DataTestMethod]
        [DataRow("user-token-test", "user-token-test-pass")]
        public void GivenUsernameAndPassword_WhenAuthenticate_ThenRecieveTokenAndUserInfo(string username, string password) {
            // Given account test
            AccountResponsitory accountResponsitory = new AccountResponsitory();
            BCryptService bCryptService = new BCryptService();

            string randomSalt = bCryptService.GetRandomSalt();
            string secretKey = ServerEnvironment.GetServerSecretKey();
            string passwordHash =
                bCryptService
                .HashStringBySHA512(bCryptService.GetHashCode(randomSalt, password, secretKey));

            Account accountTest = new Account() {
                Id = ObjectId.GenerateNewId(),
                Username = username,
                Email = "user-token-test@gmail.com",
                DisplayName = username,
                Password = passwordHash,
                HashSalt = randomSalt,
            };

            Account accountSaved = accountResponsitory.CreateNewAccount(accountTest);
            Assert.IsNotNull(accountSaved);

            // When Call authcontroller
            AuthController authController = new AuthController();
            Auth authRequest = new Auth() {
                Username = username,
                Password = password
            };

            ResponseBase response = authController.SignIn(authRequest);

            // Then expect
            Assert.IsTrue(response.Status == Status.Success);
            Assert.IsTrue(response.Message == "Authorized");

            Assert.IsNotNull(response.Data);

            AuthResponse authResponse = response.Data as AuthResponse;
            Assert.IsNotNull(authResponse);

            Assert.IsNotNull(authResponse.Token);
            Assert.IsNotNull(authResponse.User);

            bool isAccountDeleted = accountResponsitory.DeleteAccount(accountSaved.Id);
            Assert.IsTrue(isAccountDeleted);
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow(" ", "")]
        [DataRow("", " ")]
        [DataRow(" ", " ")]

        [DataRow("have-user-name", "")]
        [DataRow("have-user-name", " ")]

        [DataRow("", "have-password")]
        [DataRow(" ", "have-password")]
        public void GivenEmptyParamRequest_WhenAuthenticate_ThenReceiveWrongFormatResponse(string username, string password) {
            AuthController authController = new AuthController();
            Auth authRequest = new Auth() {
                Username = username,
                Password = password
            };

            ResponseBase response = authController.SignIn(authRequest);

            Assert.IsNotNull(response);
            Assert.IsNull(response.Data);
            Assert.IsTrue(response.Message == "Request missing Username or Password in request's body");
            Assert.IsTrue(response.Status == Status.WrongFormat);
        }

        [TestMethod]
        [DataRow("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            "password-right-length")]
        [DataRow("username-right-length",
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaa")]

        [DataRow("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void GivenTooLongRequest_WhenAuthenticate_ThenReceiveWrongFormatResponse(string username, string password) {
            AuthController authController = new AuthController();
            Auth authRequest = new Auth() {
                Username = username,
                Password = password
            };

            ResponseBase response = authController.SignIn(authRequest);

            Assert.IsNotNull(response);
            Assert.IsNull(response.Data);
            Assert.IsTrue(response.Message == "Request param value too long, must be < 254 charactor");
            Assert.IsTrue(response.Status == Status.WrongFormat);
        }


        [TestMethod]
        [DataRow("username-wrong", "password-wrong")]
        [DataRow("username-wrong", "right-password")]
        public void GivenWrongAccountAuth_WhenAuthenticate_ThenReceiveUnauthorize(string username, string password) {
            AuthController authController = new AuthController();
            Auth authRequest = new Auth() {
                Username = username,
                Password = password
            };

            ResponseBase response = authController.SignIn(authRequest);

            Assert.IsNotNull(response);
            Assert.IsNull(response.Data);
            Assert.IsTrue(response.Status == Status.Unauthorized);
            Assert.IsTrue(response.Message == "User: " + username + " is not exist");
        }

        [TestMethod]
        [DataRow("KhaThiPhan", "password-wrong")]
        [DataRow("thanhdatne", "password-wrong")]
        public void GivenRightUsernameAuth_WhenAuthenticate_ThenReceiveUnauthorize(string username, string password) {
            AuthController authController = new AuthController();
            Auth authRequest = new Auth() {
                Username = username,
                Password = password
            };

            ResponseBase response = authController.SignIn(authRequest);

            Assert.IsNotNull(response);
            Assert.IsNull(response.Data);
            Assert.IsTrue(response.Status == Status.Unauthorized);
            Assert.IsTrue(response.Message == "Username or password was wrong");
        }
    }
}

