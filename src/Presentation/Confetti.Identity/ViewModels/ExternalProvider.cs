namespace Confetti.Identity.ViewModels
{
    /// <summary>
    /// Represents a external provider
    /// </summary>
    public class ExternalProvider
    {
        /// <summary>
        /// Gets or sets frendly external provider name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets external provider authentication scheme
        /// </summary>
        public string AuthenticationScheme { get; set; }
    }
}