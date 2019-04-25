using System.Threading.Tasks;

namespace Confetti.Common.Security
{
    /// <summary>
    /// Represents a permission service.
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Find permission by system name.
        /// </summary>
        /// <param name="systemName">Permission system name.</param>
        /// <returns>Permission.</returns>
        Task<PermissionModel> FindBySystemNameAsync(string systemName);
    }
}