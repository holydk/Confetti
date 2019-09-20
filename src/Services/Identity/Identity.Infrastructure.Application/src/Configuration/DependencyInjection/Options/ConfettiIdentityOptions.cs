namespace Confetti.Identity.Application.Configuration
{
    /// <summary>
    /// Represents all the options you can use to configure the identity system (additional to AspNetIdentity).
    /// </summary>
    public class ConfettiIdentityOptions
    {
        /// <summary>
        /// Gets or sets the <see cref="ConfettiUserOptions"/> for the identity system.
        /// </summary>
        /// <value>
        /// The <see cref="ConfettiUserOptions"/> for the identity system.
        /// </value>
        public ConfettiUserOptions User { get; set; } = new ConfettiUserOptions();

        /// <summary>
        /// Gets or sets the <see cref="ConfettiPasswordOptions"/> for the identity system.
        /// </summary>
        /// <value>
        /// The <see cref="ConfettiPasswordOptions"/> for the identity system.
        /// </value>
        public ConfettiPasswordOptions Password { get; set; } = new ConfettiPasswordOptions();
    }
}