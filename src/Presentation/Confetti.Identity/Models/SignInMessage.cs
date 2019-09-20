using System;

namespace Confetti.Identity.Models
{
    /// <summary>
    /// Represents contextual information about a login request.
    /// </summary>
    public class SignInMessage : Message
    {
        /// <summary>
        /// The return URL to return to after authentication. If the login request is part of an authorization request, then this will be populated.
        /// </summary>
        /// <value>
        /// The return URL.
        /// </value>
        public string ReturnUrl { get; set; }
    }
}