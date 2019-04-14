using System.Threading.Tasks;

namespace Confetti.Domain.Interfaces
{
    /// <summary>
    /// Installation service
    /// </summary>
    public partial interface IInstallationService
    {
        /// <summary>
        /// Install data
        /// </summary>
        /// <param name="updateData">A value indicating whether to rewrite old data</param>
        Task InstallDataAsync(bool updateData);
    }
}