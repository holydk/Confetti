namespace Confetti.Common
{
    /// <summary>
    /// Represents a action error.
    /// </summary>
    public class ActionError
    {
        /// <summary>
        /// Gets a operation error code.
        /// </summary>
        public int? Code { get; }

        /// <summary>
        /// Gets a error message.
        /// </summary>
        public string Message { get; }

        public ActionError(string message, int? code = null)
        {
            Code = code;
            Message = message;
        }
    }
}