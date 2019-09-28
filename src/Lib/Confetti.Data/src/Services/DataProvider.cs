using System;

namespace Confetti.Data.Services
{
    /// <summary>
    /// Represents a data provider.
    /// </summary>
    public class DataProvider
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the handler.
        /// </summary>
        public Type Handler { get; set; }

        public DataProvider(string name, Type handler)
        {
            Name = name;
            Handler = handler;
        }
    }
}