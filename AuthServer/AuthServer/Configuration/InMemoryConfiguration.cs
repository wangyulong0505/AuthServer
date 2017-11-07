using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Configuration
{
    public class InMemoryConfiguration
    {
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[]
            {
                //这里指定了name和display name, 以后api使用authorization server的时候, 这个name一定要一致
                new ApiResource("socialnetwork", "社交网络")
            };
        }

        public static IEnumerable<Client> Clients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "socialnetwork",//  
                    ClientSecrets = new [] { new Secret("secret".Sha256())},                    //ClientSecrets是Client用来获取token用的.
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,   //AllowedGrantType: 这里使用的是通过用户名密码和ClientCredentials来换取token的方式.
                                                                                                //ClientCredentials允许Client只使用ClientSecrets来获取token. 这比较适合那种没有用户参与的api动作.
                    AllowedScopes = new []{ "socialnetwork" }                                   //AllowedScopes: 这里只用socialnetwork
                },
                new Client
                {
                    ClientId = "mvc_implicit",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = {"http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc"},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "socialnetwork"
                    },
                    //允许浏览器在使用implicit flow时返回Access token.
                    AllowAccessTokensViaBrowser = true
                }
            };
        }

        public static IEnumerable<TestUser> Users()
        {
            return new[]
            {
                //实际生产环境中还是需要使用数据库来存储用户信息的
                new TestUser
                {
                    SubjectId = "1",
                    Username = "mail@qq.com",
                    Password = "password"
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }
}
