﻿using MongoDB.Bson;
using ServiceStack;
using SocialNetworkBE.Payload.Request;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Data;
using SocialNetworkBE.Payloads.Request;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.ServerConfiguration;
using SocialNetworkBE.Services.Hash;
using SocialNetworkBE.Services.JsonWebToken;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace SocialNetworkBE.Services.Authenticate {
    public class AuthService {
        private readonly AccountResponsitory accountResponsitory = new AccountResponsitory();
        private readonly JsonWebTokenService jsonWebTokenService = new JsonWebTokenService();
        public ResponseBase HandleUserAuthenticate(Auth auth) {
            Account savedAccount =
                accountResponsitory.GetAccountByUsername(auth.Username);

            if (savedAccount == null) {
                return new ResponseBase() {
                    Status = Status.Unauthorized,
                    Message = "User: " + auth.Username + " is not exist",
                };
            }

            // Validate Password
            BCryptService bCryptService = new BCryptService();

            string secretKey = ServerEnvironment.GetServerSecretKey();

            string verifyText =
                bCryptService.GetHashCode(savedAccount.HashSalt, auth.Password, secretKey);

            bool isValidated =
                bCryptService.ValidateStringAndHashBySHA512(verifyText, savedAccount.Password);

            if (!isValidated) {
                return new ResponseBase() {
                    Status = Status.Unauthorized,
                    Message = "Username or password was wrong",
                };
            }

            // Create Tokens
            TokenClaimsModel tokenClaimsModel = new TokenClaimsModel() {
                Role = "user",
                Username = savedAccount.Username,
                Email = savedAccount.Email,
                AvatarURL = savedAccount.AvatarUrl,
                DisplayName = savedAccount.DisplayName,
                ObjectId = savedAccount.Id.ToString(),
                UserProfileURL = savedAccount.UserProfileUrl,
                KeyPairHash = Guid.NewGuid().ToString()
            };

            ClaimsIdentity accessTokenClaims =
                jsonWebTokenService.CreateClaimsIdentityFromModel(tokenClaimsModel, jsonWebTokenService.AccessToken);

            ClaimsIdentity refreshTokenClaims =
                 jsonWebTokenService.CreateClaimsIdentityFromModel(tokenClaimsModel, jsonWebTokenService.RefreshToken);


            string accessToken = jsonWebTokenService
                .CreateTokenFromClaims(accessTokenClaims, jsonWebTokenService.accessTokenExpriseTime);

            string refreshToken = jsonWebTokenService
                .CreateTokenFromClaims(refreshTokenClaims, jsonWebTokenService.refreshTokenExpriseTime);

            AuthResponse authResponse =
                new AuthResponse(
                     accessToken,
                     refreshToken,
                     savedAccount.Id.ToString(),
                     savedAccount.DisplayName,
                     savedAccount.AvatarUrl,
                     savedAccount.UserProfileUrl
                );

            ResponseBase response =
                new ResponseBase() {
                    Status = Status.Success,
                    Message = "Authorized",
                    Data = authResponse
                };

            return response;
        }

        /** 
         Logic refresh token:
            1. Only refresh when access token exprise and Refresh token is valid
            2. Check hash code in token - is a key pairs if hash code is match
         */
        public ResponseBase HandleRefreshToken(Token tokenRequest) {
            // Validate token
            ClaimsIdentity validAccessToken = 
                jsonWebTokenService.GetClaimsIdentityFromToken(tokenRequest.AccessToken);

            if (validAccessToken != null) {
                bool isRightAccessTokenType =
                    jsonWebTokenService
                    .GetValueFromClaimIdentityByTypeClaim(validAccessToken, jsonWebTokenService.KeyClaimsToken)
                    .Equals(jsonWebTokenService.AccessToken);

                if (!isRightAccessTokenType) {
                    return new ResponseBase() {
                        Status = Status.Unauthorized,
                        Message = "Token in AccessToken param is not an access token"
                    };
                }

                return new ResponseBase() {
                    Status = Status.Unauthorized,
                    Message = "Access token does not exprise - wait for it's exprision to refresh"
                };
            }

            ClaimsIdentity validRefreshToken = 
                jsonWebTokenService.GetClaimsIdentityFromToken(tokenRequest.RefreshToken);

            if (validRefreshToken != null) {
                bool isRightRefreshTokenType =
                   jsonWebTokenService
                   .GetValueFromClaimIdentityByTypeClaim(validRefreshToken, jsonWebTokenService.KeyClaimsToken)
                   .Equals(jsonWebTokenService.RefreshToken);

                if (!isRightRefreshTokenType) {
                    return new ResponseBase() {
                        Status = Status.Unauthorized,
                        Message = "Token in RefreshToken param is not a refresh token"
                    };
                }

                // TODO: Error when get Claims from token
                string refreshToken = jsonWebTokenService
                    .CreateTokenFromClaims(validRefreshToken, jsonWebTokenService.refreshTokenExpriseTime);

                var existingClaim = validRefreshToken.FindFirst(jsonWebTokenService.KeyClaimsToken);
                validRefreshToken.RemoveClaim(existingClaim);

                validRefreshToken.AddClaim(
                    new Claim(jsonWebTokenService.KeyClaimsToken, jsonWebTokenService.AccessToken));

                string accessToken = jsonWebTokenService
                    .CreateTokenFromClaims(validRefreshToken, jsonWebTokenService.accessTokenExpriseTime);


                return new ResponseBase() {
                    Status = Status.Success,
                    Message = "Authorized",
                    Data = new TokenResponse() {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        Exprise = jsonWebTokenService.accessTokenExpriseTime
                    }
                };
            }

            return new ResponseBase() {
                Status = Status.Unauthorized,
                Message = "Refresh token exprised - login again please"
            };
        }

        public ResponseBase HandleUserSignUp(
             string userName,
             string pwd,
             string email,
              string DisplayName,
              string AvatarUrl,
              string UserProfileUrl
             ) {

            BCryptService bCryptService = new BCryptService();

            string randomSalt = bCryptService.GetRandomSalt();
            string secretKey = ServerEnvironment.GetServerSecretKey();
            string password = pwd;
            string passwordHash =
                bCryptService
                .HashStringBySHA512(bCryptService.GetHashCode(randomSalt, password, secretKey));

            Account newAccount = new Account() {
                Id = ObjectId.GenerateNewId(),
                DisplayName = DisplayName,
                Email = email,
                AvatarUrl = AvatarUrl,
                UserProfileUrl = UserProfileUrl,
                Username = userName,
                Password = passwordHash,
                HashSalt = randomSalt,
                CreatedAt = DateTime.Now,
                NumberOfFriend = 0,
                ListFriendsObjectId = new List<ObjectId>(),
                ListPostsObjectId = new List<ObjectId>(),
                ListObjectId_GiveUserInvitation = new List<ObjectId>(),
                ListObjectId_UserSendInvite = new List<ObjectId>()
            };

            Account savedAccount = accountResponsitory.CreateNewAccount(newAccount);
            if (savedAccount == null) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "Create account failure"
                };
            }

            return new ResponseBase() {
                Status = Status.Success,
                Message = "Create account success",
                Data = accountResponsitory.GetAccountByObjectId(newAccount.Id)
            };
        }
    }
}