using Confetti.Data.Configuration;
using Confetti.Data.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Represents a confetti data extensions for <see cref="ConfettiDataBuilder"/>.
    /// </summary>
    public static class ConfettiDataBuilderExtensionsAdditional
    {
        /// <summary>
        /// Adds a data provider.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The name of data provider.</param>
        /// <typeparam name="T">The type of data provider handler.</typeparam>
        /// <returns></returns>
        public static ConfettiDataBuilder AddDataProvider<T>(this ConfettiDataBuilder builder, string name)
            where T : class, IDataProviderHandler
        {
            if (name == null) throw new System.ArgumentNullException(nameof(name));

            builder.Services.AddTransient<IDataProviderHandler, T>();
            builder.Services.AddSingleton(new DataProvider(name, typeof(T)));

            return builder;
        }

        /// <summary>
        /// Adds a data provider manager.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <typeparam name="T">The type of data provider manager.</typeparam>
        /// <returns></returns>
        public static ConfettiDataBuilder AddDataProviderManager<T>(this ConfettiDataBuilder builder)
            where T : class, IDataProviderManager
        {
            builder.Services.AddScoped<IDataProviderManager, T>();

            return builder;
        }
    }
}