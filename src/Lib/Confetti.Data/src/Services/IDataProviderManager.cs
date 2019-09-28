namespace Confetti.Data.Services
{
    /// <summary>
    /// Provides an abstraction for a managing of <see cref="IDataProviderHandler"/>'s.
    /// </summary>
    public interface IDataProviderManager
    {
        /// <summary>
        /// Gets a default <see cref="IDataProviderHandler"/>.
        /// </summary>
        /// <value>The <see cref="IDataProviderHandler"/>.</value>
        IDataProviderHandler GetDefault();

        /// <summary>
        /// Gets a <see cref="IDataProviderHandler"/> by name.
        /// </summary>
        /// <value>The <see cref="IDataProviderHandler"/>.</value>
        IDataProviderHandler GetByName(string name);
    }
}