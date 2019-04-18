namespace Confetti.Infrastructure.Caching
{
    /// <summary>
    /// Represents default values related to caching
    /// </summary>
    public static partial class ConfettiCachingDefaults
    {
        /// <summary>
        /// Gets the default cache time in minutes
        /// </summary>
        public static int CacheTime => 60;

        /// <summary>
        /// Gets the key used to store the protection key list to Redis (used with the PersistDataProtectionKeysToRedis option enabled)
        /// </summary>
        public static string RedisDataProtectionKey => "Confetti.DataProtectionKeys";
    }
}