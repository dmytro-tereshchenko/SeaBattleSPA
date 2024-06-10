// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;
using IdentityModel;
using Microsoft.Extensions.Configuration;
using System;

namespace SeaBattle.AuthorizationService.Utilites
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                   };

        public static IEnumerable<ApiResource> GetAllResources =>
                  new[]
                    {
                // Add a resource for some set of APIs that we may be protecting
                // Note that the constructor will automatically create an allowed scope with
                // name and claims equal to the resource's name and claims. If the resource
                // has different scopes/levels of access, the scopes property can be set to
                // list specific scopes included in this resource, instead.
                new ApiResource(
                    "userInfo",                                       // Api resource name
                    "My API Set #1",                                // Display name
                    new[] { JwtClaimTypes.Name, JwtClaimTypes.Role }) // Claims to be included in access token
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
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "resourse-api", "token-api", "userInfo"
                    }
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
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "resourse-api", "token-api", "userInfo"
                    }
                },
                new Client
                {
                    ClientId = "client-js",
                    ClientName = "Angular Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    AccessTokenLifetime = 36000,
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AbsoluteRefreshTokenLifetime = 360000,
                    SlidingRefreshTokenLifetime = 36000,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    //AccessTokenType = AccessTokenType.Reference,
                    
                    RedirectUris =           { $"{Environment.GetEnvironmentVariable("SPA_CLIENT_DOMAIN")}/assets/signin-callback.html" },
                    PostLogoutRedirectUris = { $"{Environment.GetEnvironmentVariable("SPA_CLIENT_DOMAIN")}/index.html" },
                    AllowedCorsOrigins =     { Environment.GetEnvironmentVariable("SPA_CLIENT_DOMAIN") },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "resourse-api", "token-api", "userInfo"
                    }
                }
            };

        public static string GetConnectionString(IConfiguration config)
        {
            string connectionString;

            if (config["DBServer"] is null)
            {
                //in case of local db server
                connectionString = config.GetConnectionString("DefaultConnection");
            }
            else
            {
                //in case of containerization in docker
                string server = config["DBServer"] ?? config.GetValue<string>("ConnectionDb:Server");
                string port = config["DBPort"] ?? config.GetValue<string>("ConnectionDb:Port");
                string user = config["DBUser"] ?? config.GetValue<string>("ConnectionDb:User");
                string password = config["DBPassword"] ?? config.GetValue<string>("ConnectionDb:Password");
                string database = config["Database"] ?? config.GetValue<string>("ConnectionDb:Database");
                connectionString = $"Server={server},{port};Initial Catalog={database};User ID={user};Password={password}";
            }

            return connectionString;
        }
    }
}