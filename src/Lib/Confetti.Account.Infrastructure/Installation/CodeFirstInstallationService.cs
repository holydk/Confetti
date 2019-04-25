using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Confetti.Account.Infrastructure.Identity.Data;
using Confetti.Account.Infrastructure.Identity.Models;
using Confetti.Common.Installation;
using Confetti.Common.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Confetti.Account.Infrastructure.Installation
{
    /// <summary>
    /// Code first identity Installation service
    /// </summary>
    public class CodeFirstInstallationService : IInstallationService
    {
        #region Fields

        private readonly IConfiguration _configuration;
        private readonly AppIdentityDbContext _identityContext;
        private readonly IPermissionProvider _permissionProvider;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Ctor

        public CodeFirstInstallationService(
            IConfiguration configuration,
            AppIdentityDbContext identityContext,
            IPermissionProvider permissionProvider,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager
        )
        {
            _configuration = configuration;
            _identityContext = identityContext;
            _permissionProvider = permissionProvider;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        #endregion

        #region Methods

        public async Task InstallDataAsync(bool updateData)
        {
            await Roles.InstallAsync(updateData, _roleManager, _identityContext, _permissionProvider);
            await Users.InstallAsync(updateData, _userManager, _identityContext);
        }

        #endregion

        #region Utility classes

        private class Roles
        {
            public static async Task InstallAsync(
                bool updateData, 
                RoleManager<IdentityRole> roleManager,
                AppIdentityDbContext identityContext,
                IPermissionProvider permissionProvider)
            {
                if (updateData)
                {
                    var roles = await roleManager.Roles.ToListAsync();
                    identityContext.Roles.RemoveRange(roles);
                    await identityContext.SaveChangesAsync();
                }

                if (!roleManager.Roles.Any())
                {
                    foreach (var permission in permissionProvider.GetDefaultPermissions())
                    {
                        await CreateAsync(roleManager, permission);
                    }
                }
            }

            private static async Task CreateAsync(
                RoleManager<IdentityRole> roleManager,
                DefaultPermissionModel defaultPermission)
            {
                var name = defaultPermission.UserRoleSystemName;
                var result = await roleManager.CreateAsync(new IdentityRole(name));

                if (!result.Succeeded)
                    throw new Exception($"Exception while install new role - {name}.");

                if (defaultPermission.Permissions != null)
                {
                    var role = await roleManager.FindByNameAsync(name);

                    foreach (var permission in defaultPermission.Permissions)
                    {
                        result = await roleManager.AddClaimAsync(role, new Claim(ClaimConstants.Permission, permission.SystemName));

                        if (!result.Succeeded)
                            throw new Exception($"Exception while install new claim - {permission.SystemName} to role - {name}.");
                    }
                }
            }
        }
            
        private class Users
        {
            public static async Task InstallAsync(
                bool updateData,
                UserManager<ApplicationUser> userManager,
                AppIdentityDbContext identityContext
            )
            {
                if (updateData)
                {
                    var users = await userManager.Users.ToListAsync();
                    identityContext.Users.RemoveRange(users);
                    await identityContext.SaveChangesAsync();
                }

                if (!userManager.Users.Any())
                {
                    await CreateAsync(userManager, "admin", "Admin1337", "Admin@confetti.su", new [] { DefaultGlobalRoles.Administrators, DefaultGlobalRoles.Customers, DefaultGlobalRoles.Employees });
                    await CreateAsync(userManager, "employee", "Employee1337", "Employee@confetti.su", new [] { DefaultGlobalRoles.Employees });
                    await CreateAsync(userManager, "customer", "Customer1337", "Customer@confetti.su", new [] { DefaultGlobalRoles.Customers });
                }
            }

            private static async Task CreateAsync(
                UserManager<ApplicationUser> userManager,
                string name,
                string password,
                string email,
                string[] roles 
            )
            {
                var user = new ApplicationUser()
                {
                    UserName = name,
                    EmailConfirmed = true,
                    Email = email
                };

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                    throw new Exception($"Exception while install new user - {name}.");

                user = await userManager.FindByNameAsync(name);
                result = await userManager.AddToRolesAsync(user, roles);

                if (!result.Succeeded)
                    throw new Exception($"Exception at adds roles {roles} to user - {name}.");
            }
        }
        
        #endregion
    }
}