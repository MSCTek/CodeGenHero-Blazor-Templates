//// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

//using IdentityModel;
//using IdentityServer4.Test;
//using System.Collections.Generic;
//using System.Security.Claims;

//namespace IdentityServerHost.Quickstart.UI
//{
//    public class TestUsers
//    {
//        public static List<TestUser> Users = new List<TestUser>
//        {
//            new TestUser{
//                SubjectId = "AF02EE9B-96BD-467D-B14D-C4C18DC263D2",
//                Username = "Paul",
//                Password = "password",

//                Claims =
//                {
//                    new Claim(JwtClaimTypes.Name, "Paul Schroeder"),
//                    new Claim(JwtClaimTypes.GivenName, "Paul"),
//                    new Claim(JwtClaimTypes.FamilyName, "Schroeder"),
//                    new Claim(JwtClaimTypes.Email, "paul@msctek.com"),
//                    new Claim("country", "USA")
//                }
//            },
//            new TestUser{SubjectId = "6EE9A86C-09B8-4888-A963-2650EC0C2BC3",
//                Username = "Robin",
//                Password = "password",
//                Claims =
//                {
//                    new Claim(JwtClaimTypes.Name, "Robin Schroeder"),
//                    new Claim(JwtClaimTypes.GivenName, "Robin"),
//                    new Claim(JwtClaimTypes.FamilyName, "Schroeder"),
//                    new Claim(JwtClaimTypes.Email, "robin@msctek.com"),
//                    new Claim("country", "USA")
//                }
//            }
//        };
//    }
//}