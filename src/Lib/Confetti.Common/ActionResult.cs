using System.Collections.Generic;
using System.Linq;

namespace Confetti.Common
{
    /// <summary>
    /// Represents a action result.
    /// </summary>
    public class ActionResult
    {
        /// <summary>
        /// Returns an Confetti.Common.ActionResult indicating a successful identity operation.
        /// </summary>
        /// <value></value>
        public static ActionResult Success { get; }

        /// <summary>
        /// Gets a action errors.
        /// </summary>
        /// <value></value>
        public ICollection<ActionError> Errors { get; }

        /// <summary>
        /// Gets a flag indicating whether if the operation succeeded or not.
        /// </summary>
        public bool Succeeded => !Errors.Any();

        static ActionResult()
        {
            Success = new ActionResult();
        }

        public ActionResult()
        {
            Errors = new List<ActionError>();
        }

        /// <summary>
        /// Add error.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="code">Operation error code.</param>
        public void AddError(string message, int? code = null)
        {
            Errors.Add(new ActionError(message, code));
        }
    }
}