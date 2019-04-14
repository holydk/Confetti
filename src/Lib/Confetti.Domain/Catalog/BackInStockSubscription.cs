using System;
using Confetti.Domain.Customers;

namespace Confetti.Domain.Catalog
{
    /// <summary>
    /// Represents a back in stock subscription
    /// </summary>
    public partial class BackInStockSubscription
    {
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the customer profile identifier
        /// </summary>
        public string CustomerProfileId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets the customer profile
        /// </summary>
        public virtual CustomerProfile CustomerProfile { get; set; }
    }
}