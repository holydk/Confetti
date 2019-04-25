using System.Collections.Generic;
using System.Threading.Tasks;
using Confetti.Account.Application.Security;
using Confetti.Common.Security;

namespace Confetti.Account.Infrastructure.Security
{
    /// <summary>
    /// Represents a account permission provider.
    /// </summary>
    public class PermissionProvider : IPermissionProvider
    {
        #region User category of permissions
            
        public static PermissionModel ViewUsers = new PermissionModel() { Name = "View users", SystemName = PermissionTypes.ViewUsers };
        public static PermissionModel ManageUsers = new PermissionModel() { Name = "Manage users", SystemName = PermissionTypes.ManageUsers };

        #endregion

        #region Roles category of permissions
            
        public static PermissionModel ViewRoles = new PermissionModel() { Name = "View roles", SystemName = PermissionTypes.ViewRoles };
        public static PermissionModel ManageRoles = new PermissionModel() { Name = "Manage roles", SystemName = PermissionTypes.ManageRoles };
        public static PermissionModel AssignRoles = new PermissionModel() { Name = "Assign roles", SystemName = PermissionTypes.AssignRoles };

        #endregion


        /// <summary>
        /// Get permissions.
        /// </summary>
        /// <returns>Permission.</returns>
        public IEnumerable<PermissionModel> GetPermissions()
        {
            return new []
            {
                ViewUsers,
                ManageUsers,
                ViewRoles,
                ManageRoles,
                AssignRoles
            };
        }

        /// <summary>
        /// Get default (const) permissions.
        /// </summary>
        /// <returns>Default permission.</returns>
        public IEnumerable<DefaultPermissionModel> GetDefaultPermissions()
        {
            return new []
            {
                new DefaultPermissionModel()
                {
                    UserRoleSystemName = DefaultGlobalRoles.Administrators,
                    Permissions = new []
                    {
                        ViewUsers,
                        ManageUsers,
                        ViewRoles,
                        ManageRoles,
                        AssignRoles
                    }
                },
                new DefaultPermissionModel()
                {
                    UserRoleSystemName = DefaultGlobalRoles.Employees
                },
                new DefaultPermissionModel()
                {
                    UserRoleSystemName = DefaultGlobalRoles.Customers
                }
            };
        }
    }
}