using System.Collections.Generic;
using System.Threading.Tasks;

namespace Confetti.Common.Security
{
    /// <summary>
    /// Represents a permission provider.
    /// </summary>
    public interface IPermissionProvider
    {
        /// <summary>
        /// Get permissions.
        /// </summary>
        /// <returns>Permission.</returns>
        IEnumerable<PermissionModel> GetPermissions();

        /// <summary>
        /// Get default (const) permissions.
        /// </summary>
        /// <returns>Default permission.</returns>
        IEnumerable<DefaultPermissionModel> GetDefaultPermissions();
    }
}