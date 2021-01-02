// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using IdentityServer4.Models;

namespace Chat.DIP.IS4
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("chat-api", "Chat API")
            };
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("chat-api", "Chat API")
            };

        public static IEnumerable<Client> Clients=> new List<Client>
        {
            // machine to machine client (from quickstart 1)
            new Client
            {
                ClientId = "Developer",
                ClientSecrets = { new Secret("SupperPass".Sha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // scopes that client has access to
                AllowedScopes = { "chat-api" }
            },
            new Client
            {
                ClientId = "ResourceOwner",
                ClientSecrets = { new Secret("SupperPass".Sha256()) },

                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                // scopes that client has access to
                AllowedScopes = { "chat-api" }
            },
            new Client
            {
                ClientId = "chat-spa-client",
                ClientName = "Projects SPA",
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,


                RedirectUris =           { "http://localhost:4200/signin-callback", "http://localhost:4200/assets/silent-callback.html"  },
                PostLogoutRedirectUris = { "http://localhost:4200/signout-callback" },
                AllowedCorsOrigins =     { "http://localhost:4200" },

                AllowedScopes =
                {
                    "openid", "profile", "chat-api"
                },
                AccessTokenLifetime = 600
            }
        };
    }
}