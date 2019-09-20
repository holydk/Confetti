using System.Collections.Generic;
using System.Linq;

namespace Confetti.Identity.Application.Models
{
    /// <summary>
    /// Represents a result of register operation.
    /// </summary>
    public class RegisterResult
    {
        /// <summary>
        /// Gets a errors.
        /// </summary>
        /// <value></value>
        public ICollection<string> Errors { get; }

        /// <summary>
        /// Gets a flag indicating whether if the operation succeeded or not.
        /// </summary>
        public bool Succeeded => !Errors.Any();

        public RegisterResult()
        {
            Errors = new List<string>();
        }

        /// <summary>
        /// Adds the new error.
        /// </summary>
        /// <param name="error">Error message.</param>
        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}