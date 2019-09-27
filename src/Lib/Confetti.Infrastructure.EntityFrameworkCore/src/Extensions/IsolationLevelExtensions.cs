namespace System.Transactions
{
    /// <summary>
    /// Converts <see cref="System.Transactions.IsolationLevel"/> to <see cref="IsolationLevel"/>.
    /// </summary>
    internal static class IsolationLevelExtensions
    {
        /// <summary>
        /// Converts <see cref="System.Transactions.IsolationLevel"/> to <see cref="IsolationLevel"/>.
        /// </summary>
        public static System.Data.IsolationLevel ToSystemDataIsolationLevel(this IsolationLevel isolationLevel)
        {
            switch (isolationLevel)
            {
                case IsolationLevel.Chaos:
                    return System.Data.IsolationLevel.Chaos;
                case IsolationLevel.ReadCommitted:
                    return System.Data.IsolationLevel.ReadCommitted;
                case IsolationLevel.ReadUncommitted:
                    return System.Data.IsolationLevel.ReadUncommitted;
                case IsolationLevel.RepeatableRead:
                    return System.Data.IsolationLevel.RepeatableRead;
                case IsolationLevel.Serializable:
                    return System.Data.IsolationLevel.Serializable;
                case IsolationLevel.Snapshot:
                    return System.Data.IsolationLevel.Snapshot;
                case IsolationLevel.Unspecified:
                    return System.Data.IsolationLevel.Unspecified;
                default:
                    throw new InvalidOperationException("Unknown isolation level: " + isolationLevel);
            }
        }
    }
}