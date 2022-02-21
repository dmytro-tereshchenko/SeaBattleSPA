// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace SeaBattle.AuthorizationService
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("token-api", "Authorization service"),
                new ApiScope("resourse-api", "Game Resources service")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // machine to machine client (from quickstart 1)
                new Client
                {
                    ClientId = "resourseApi",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret-resourse-api".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "resourse-api", "token-api" }
                },
                new Client
                {
                    ClientId = "client json",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret-resourse-client-json".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "resourse-api", "token-api" }
                },
                new Client
                {
                    ClientId = "client-js",
                    ClientName = "Angular Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    AccessTokenLifetime = 3600,
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AbsoluteRefreshTokenLifetime = 360000,
                    SlidingRefreshTokenLifetime = 36000,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    //AccessTokenType = AccessTokenType.Reference,

                    RedirectUris =           { "https://localhost:44391/assets/signin-callback.html" },
                    PostLogoutRedirectUris = { "https://localhost:44391/index.html" },
                    AllowedCorsOrigins =     { "https://localhost:44391" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "resourse-api", "token-api"
                    }
                }
            };
    }
}