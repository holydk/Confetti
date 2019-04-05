namespace Confetti.Api.Infrastructure.Cache
{
    public static class ConfettiModelCacheDefaults
    {
        /// <summary>
        /// Key for list of CategorySimpleModel caching
        /// </summary>
        /// <remarks>
        /// {0} : comma separated list of customer roles
        /// {1} : current store ID
        /// </remarks>
        public static string CategoryAllModelKey => "Confetti.pres.category.all-{0}-{1}";
        public static string CategoryAllPatternKey => "Confetti.pres.category.all";

        /// <summary>
        /// Key for caching of categories displayed on home page
        /// </summary>
        /// <remarks>
        /// {0} : roles of the current user
        /// {1} : current store ID
        /// {2} : root category ID
        /// </remarks>
        public static string CategoryHomepageKey => "Confetti.pres.category.homepage-{0}-{1}-{2}";
        public static string CategoryHomepagePatternKey => "Confetti.pres.category.homepage";

        /// <summary>
        /// Key for caching of category breadcrumb
        /// </summary>
        /// <remarks>
        /// {0} : category id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// </remarks>
        public static string CategoryBreadcrumbKey => "Nop.pres.category.breadcrumb-{0}-{1}-{2}";
        public static string CategoryBreadcrumbPatternKey => "Nop.pres.category.breadcrumb";
    }
}