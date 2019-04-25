using System.Collections.Generic;

namespace Confetti.Common.Security
{
    /// <summary>
    /// Represents a default (const) permission model
    /// </summary>
    public class DefaultPermissionModel
    {
        /// <summary>
        /// Gets or sets system name of user role 
        /// </summary>
        public string UserRoleSystemName { get; set; }

        /// <summary>
        /// Gets or sets permissions
        /// </summary>
        public IEnumerable<PermissionModel> Permissions { get; set; }
    }
}