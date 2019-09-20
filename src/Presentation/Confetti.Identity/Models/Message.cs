using System;

namespace Confetti.Identity.Models
{
    /// <summary>
    /// Base class for data that needs to be written out as cookies.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        internal Message()
        {
            Created = DateTimeOffset.UtcNow.Ticks;
        }

        /// <summary>
        /// Gets or sets the UTC ticks the <see cref="Message"/> was created.
        /// </summary>
        /// <value>
        /// The created UTC ticks.
        /// </value>
        public long Created { get; set; }
    }
}