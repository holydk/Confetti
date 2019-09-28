using Confetti.Data.Configuration;
using Confetti.Infrastructure.EntityFrameworkCore.SqlServer;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Represents a sql server confetti data extensions for <see cref="ConfettiDataBuilder"/>.
    /// </summary>
    public static class SqlServerConfettiDataBuilderExtensionsAdditional
    {
        /// <summary>
        /// Adds a sql server data provider.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="T">The type of sql server data provider handler.</typeparam>
        /// <returns></returns>
        public static ConfettiDataBuilder AddSqlServerDataProvider<T>(this ConfettiDataBuilder builder)
            where T : SqlServerDataProviderHandler
        {
            builder
                .AddDataProvider<T>("SqlServer");

            return builder;
        }
    }
}