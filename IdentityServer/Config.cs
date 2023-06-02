using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer;

public class Config
{
            public static IEnumerable<Client> Clients (IConfiguration configuration) =>
            new Client[]
            {
                   new Client
                   {
//                        ClientId = "movieClient",
                        ClientId = configuration.GetSection("ClientId:MovieApi").Value,
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },
                        AllowedScopes = { "movieAPI" }
                   },
                   new Client
                   {
                       //ClientId = "movies_mvc_client",
                       ClientId = configuration.GetSection("ClientId:MovieClient").Value,
                       ClientName = "Movies MVC Web App",
                       AllowedGrantTypes = GrantTypes.Code,
                       AllowRememberConsent = false,
                       RedirectUris = new List<string>
                       {
                           "https://localhost:7101/signin-oidc"
                       },
                       PostLogoutRedirectUris = new List<string>
                       {
                           "https://localhost:7101/signout-callback-oidc"
                       },
                       ClientSecrets = new List<Secret>
                       {
                           new Secret("secret".Sha256())
                       },
                       AllowedScopes = new List<string>
                       {
                           IdentityServerConstants.StandardScopes.OpenId,
                           IdentityServerConstants.StandardScopes.Profile
                       }
                   }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
           new ApiScope[]
           {
               new ApiScope("movieAPI", "Movie API")
           };

        public static IEnumerable<ApiResource> ApiResources =>
          new ApiResource[]
          {
               //new ApiResource("movieAPI", "Movie API")
          };

        public static IEnumerable<IdentityResource> IdentityResources =>
          new IdentityResource[]
          {
              new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
              new IdentityResources.Address(),
              new IdentityResources.Email(),
              new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new List<string>() { "role" })
          };

        public static List<TestUser> TestUsers =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "petr",
                    Password = "kutil",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.GivenName, "petr"),
                        new Claim(JwtClaimTypes.FamilyName, "kutil")
                    }
                }
            };

}