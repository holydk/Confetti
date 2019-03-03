namespace Confetti.Application.Catalog
{
    /// <summary>
    /// Represents default values related to catalog services
    /// </summary>
    public static partial class ConfettiCatalogDefaults
    {
        #region Categories

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : current store ID
        /// {1} : comma separated list of customer roles
        /// {2} : show hidden records?
        /// </remarks>
        public static string CategoriesAllCacheKey => "Nop.category.all-{0}-{1}-{2}";
            
        #endregion
    }
}