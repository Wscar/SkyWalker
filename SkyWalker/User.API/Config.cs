﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;

namespace User.API
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>
             {
                 new IdentityResources.OpenId(),
                 new IdentityResources.Profile(),
                 new IdentityResources.Email()
             };
        }
        public static IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>
            {
               // new ApiResource("gateway_api","gateway service"),

                new ApiResource("user_api","user_api service"),
                //并且要把contactapi加入到apiResource,并加入到 client的allowedScopes中 
               // new ApiResource("contact_api","contact_api service")
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
          {
              new Client
              {
                    ClientId="pc",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets = { new Secret("yemobai".Sha256()) },
                   // AlwaysIncludeUserClaimsInIdToken = true,                 
                    //AllowOfflineAccess = true,
                    AllowedScopes=new List<string>
                    {
                        "user_api",
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OpenId,
                   // IdentityServerConstants.StandardScopes.OfflineAccess
                    }

              }
          };
        }
    }
}