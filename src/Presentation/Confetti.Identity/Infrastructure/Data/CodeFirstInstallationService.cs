using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Confetti.Common.Installation;
using Confetti.Common.Security;
using Confetti.Identity.Models;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Confetti.Identity.Infrastructure.Data
{
    /// <summary>
    /// Code first identity Installation service
    /// </summary>
    public class CodeFirstInstallationService : IInstallationService
    {
        #region Fields

        private readonly IConfiguration _configuration;
        private readonly ConfigurationDbContext _configurationContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        #endregion

        #region Ctor

        public CodeFirstInstallationService(
            IConfiguration configuration,
            ConfigurationDbContext configurationContext
        )
        {
            _configuration = configuration;
            _configurationContext = configurationContext;
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Install data
        /// </summary>
        /// <param name="updateData">A value indicating whether to rewrite old data</param>
        public async Task InstallDataAsync(bool updateData)
        {        
            await Resources.InstallAsync(updateData, _configuration, _configurationContext);
            await Clients.InstallAsync(updateData, _configuration, _configurationContext);
        }
            
        #endregion

        #region Utility classes

        private class Clients
        {
            public static async Task InstallAsync(
                bool updateData,
                IConfiguration configuration,
                ConfigurationDbContext configurationContext
            )
            {
                if (updateData)
                {
                    var clients = await configurationContext.Clients.ToListAsync();
                    configurationContext.Clients.RemoveRange(clients);
                    await configurationContext.SaveChangesAsync();
                }

                if (!configurationContext.Clients.Any())
                {
                    foreach (var client in Get(configuration))
                    {
                        configurationContext.Clients.Add(client.ToEntity());
                    }
                    await configurationContext.SaveChangesAsync();
                }
            }

            public static IEnumerable<Client> Get(IConfiguration configuration)
            {
                return new List<Client>
                {
                    new Client()
                    {
                        ClientId = configuration["ApiClients:ConfettiSpa:ClientId"],
                        ClientName = configuration["ApiClients:ConfettiSpa:Name"],
                        AllowedGrantTypes = GrantTypes.Implicit,
                        AllowedScopes = configuration
                            .GetSection("ApiClients:ConfettiSpa:Scopes")
                            .Get<string[]>()
                            .Concat(new []
                            {
                                // For UserInfo endpoint.
                                IdentityServerConstants.StandardScopes.OpenId,
                                IdentityServerConstants.StandardScopes.Profile,
                                IdentityServerConstants.StandardScopes.Email,
                                ScopeConstants.Roles
                            }).ToArray(),
                        RedirectUris = configuration
                            .GetSection("ApiClients:ConfettiSpa:RedirectUris")
                            .Get<string[]>(),
                        PostLogoutRedirectUris = configuration
                            .GetSection("ApiClients:ConfettiSpa:PostLogoutRedirectUris")
                            .Get<string[]>(),
                        // For refresh token.
                        AllowOfflineAccess = true,
                        // OneTime the refresh token handle will be updated when refreshing tokens.
                        //RefreshTokenUsage = TokenUsage.OneTimeOnly,
                        // Lifetime of access token in seconds.
                        // 1 hour.
                        // AccessTokenLifetime = 3600,
                        AccessTokenLifetime = 60,
                        // Maximum lifetime of a refresh token in seconds.
                        // 30 days.
                        AbsoluteRefreshTokenLifetime = 2592000,
                        // When refreshing the token, the lifetime of the refresh token will be renewed 
                        // (by the amount specified in SlidingRefreshTokenLifetime). 
                        // The lifetime will not exceed AbsoluteRefreshTokenLifetime.
                        RefreshTokenExpiration = TokenExpiration.Sliding,
                        // Sliding lifetime of a refresh token in seconds.
                        // 15 days.
                        SlidingRefreshTokenLifetime = 1296000,
                        // Disable a consent screen.
                        RequireConsent = false,
                        AllowAccessTokensViaBrowser = true
                    }
                };
            }
        }         
            
        private class Resources
        {
            public static async Task InstallAsync(
                bool updateData,
                IConfiguration configuration,
                ConfigurationDbContext configurationContext
            )
            {
                if (updateData)
                {
                    var identityResources = await configurationContext.IdentityResources.ToListAsync();
                    configurationContext.IdentityResources.RemoveRange(identityResources);
                    var apiResources = await configurationContext.ApiResources.ToListAsync();
                    configurationContext.ApiResources.RemoveRange(apiResources);
                    await configurationContext.SaveChangesAsync();
                }

                if (!configurationContext.IdentityResources.Any())
                {
                    foreach (var identityResources in GetIdentityResources())
                    {
                        configurationContext.IdentityResources.Add(identityResources.ToEntity());
                    }
                    await configurationContext.SaveChangesAsync();
                }

                if (!configurationContext.ApiResources.Any())
                {
                    foreach (var apiResources in GetApiResources(configuration))
                    {
                        configurationContext.ApiResources.Add(apiResources.ToEntity());
                    }
                    await configurationContext.SaveChangesAsync();
                }
            }

            public static IEnumerable<IdentityResource> GetIdentityResources()
            {
                return new List<IdentityResource>()
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email(),
                    new IdentityResource(ScopeConstants.Roles, new[] { JwtClaimTypes.Role })
                };
            }

            public static IEnumerable<ApiResource> GetApiResources(IConfiguration configuration)
            {
                // JwtClaimTypes.Name,
                // JwtClaimTypes.Email,
                // JwtClaimTypes.Role,
                // ClaimConstants.Permission
                return new List<ApiResource> {
                    new ApiResource(
                        configuration["ApiResources:ConfettiApi:Name"], 
                        configuration["ApiResources:ConfettiApi:DisplayName"], 
                        new[] 
                        { 
                            JwtClaimTypes.Role, 
                            JwtClaimTypes.GivenName, 
                            JwtClaimTypes.FamilyName 
                        }),
                    new ApiResource(
                        configuration["ApiResources:ConfettiAccount:Name"], 
                        configuration["ApiResources:ConfettiAccount:DisplayName"],
                        new[] 
                        { 
                            JwtClaimTypes.Role, 
                            JwtClaimTypes.GivenName, 
                            JwtClaimTypes.FamilyName 
                        })
                };
            }
        }

        #endregion
    }
}