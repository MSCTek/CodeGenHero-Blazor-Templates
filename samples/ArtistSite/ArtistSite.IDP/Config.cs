// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace ArtistSite.IDP
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>For and explanation of ApiResource vs ApiScope vs IdentityResource see: https://stackoverflow.com/questions/63811157/apiresource-vs-apiscope-vs-identityresource
    /// </remarks>
    public static class Config
    {
        public static IEnumerable<ApiResource> Apis =>
           new ApiResource[]
           {
                new ApiResource(name: "CGHApi",
                    displayName: "CodeGenHero API",
                    userClaims: new [] { "country", "userId", "role" })
                {
                    ApiSecrets = { new Secret( "24178c2d-1ec6-4250-ab00-78ee304bba1f".Sha256()) },
                    Scopes = new [] { "CGHApi" } // Set the scopes value to ensure the aud (audience) property gets set on the access token.
                }
           };

        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[] { new ApiScope("CGHApi") };

        public static IEnumerable<Client> Clients => new Client[] {
            new Client {
                ClientId = "CGHClientId",
                ClientName = "CodeGenHero Client",
                //AccessTokenLifetime = 120,
                //AlwaysIncludeUserClaimsInIdToken = , // Use the UserInfo Endpoint
                AllowOfflineAccess = true,
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                RedirectUris = {
                    "https://localhost:5201/authentication/login-callback",
                    "https://localhost:44343/authentication/login-callback",
                    "https://localhost:44301/signin-oidc",
                    "https://localhost:5151/signin-oidc"
                },
                PostLogoutRedirectUris = {
                    "https://localhost:5201/authentication/logout-callback",
                    "https://localhost:44343/authentication/logout-callback",
                    "https://localhost:44301/_Host",
                    "https://localhost:5151/_Host"
                },
                AllowedScopes = {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "CGHApi",
                    "country",
                    "userId",
                    "roles",
                },
                AllowedCorsOrigins = { "https://localhost:44341", "https://localhost:5201",
                    "https://localhost:44301", "https://localhost:5151"
                },
                RequireConsent = false,
                ClientSecrets = {
                    new Secret("4bde0c0d-3162-49b6-a3d3-b0fb4d5367b3".Sha256())
                }
            }
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                    new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource("country", new [] { "country" }),
            new IdentityResource("userId", new [] { "userId" }),
            new IdentityResource("roles", "User role(s)", new List<string> { "role" })
        };
    }
}