using System.Linq;
using System.Threading.Tasks;
using Confetti.Common.Security;

namespace Confetti.Account.Infrastructure.Security
{
    /// <summary>
    /// Represents a account permission service.
    /// </summary>
    public class PermissionService : IPermissionService
    {
        #region Fields

        private readonly IPermissionProvider _permissionProvider;

        #endregion

        #region Ctor

        public PermissionService(IPermissionProvider permissionProvider)
        {
            _permissionProvider = permissionProvider;
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Find permission by system name.
        /// </summary>
        /// <param name="systemName">Permission system name.</param>
        /// <returns>Permission.</returns>
        public Task<PermissionModel> FindBySystemNameAsync(string systemName)
        {
            var permission = _permissionProvider
                .GetPermissions()
                .Where(p => p.SystemName == systemName)
                .FirstOrDefault();

            return Task.FromResult(permission);
        }
            
        #endregion
    }   
}