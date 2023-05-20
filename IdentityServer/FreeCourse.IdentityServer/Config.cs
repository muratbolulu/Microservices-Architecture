// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes={"catalog_fullPermission"}},
            new ApiResource("resource_photo_stock"){Scopes={"photo_stock_fullPermission"}},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };


        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                    //IdentityResources hazır
                    new IdentityResources.Email(),
                    new IdentityResources.OpenId(),//jwt sub claim
                    new IdentityResources.Profile(),
                    //yeni yazılan IdentityResource, UserClaims 'e map'lenmelidir.
                    new IdentityResource{Name="roles",DisplayName="Roles", Description="Kullanıcı Rolleri",UserClaims=new[]{"role"}}
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullPermission","Catalog API için full erişim"),
                new ApiScope("photo_stock_fullPermission","Photo Stock API için full erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                //bu client unusers alınan token için
                new Client
                {
                    ClientName="Asp.Net Core Mvc",
                    ClientId="WebMvcClient",
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,// ClientCredentials refresh token yoktur, 
                    AllowedScopes={"catalog_fullPermission", "photo_stock_fullPermission",IdentityServerConstants.LocalApi.ScopeName}//erişim izinleri
                },

                //bu client user ile alınan tokens için
                new Client
                {
                    ClientName="Asp.Net Core Mvc",
                    ClientId="WebMvcClientForUser",
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess= true, //OfflineAccess için izin verilir.
                    AllowedScopes={IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.OfflineAccess,"roles",IdentityServerConstants.LocalApi.ScopeName}, //OfflineAccess = kullanıcı login olmasa dahi kullanıcı adına bir refresh token elde edebilmek için. kullanıcı offline olsa bile elimizdeki refresh token ile tekrar token alabiliriz. bu olmazsa login olmalı sonra token alır. Yani refresh token ile token alır.

                    AccessTokenLifetime=1*60*60,//token ömrü 1 saat
                    RefreshTokenExpiration=TokenExpiration.Absolute,//refresh token ömrü belirtir.
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,//60 günlük bir refresh token oluşturmak için.
                    RefreshTokenUsage= TokenUsage.ReUse//Refresh token için tekrar tekrar kullanılabilirlik.
                }
            };
    }
}