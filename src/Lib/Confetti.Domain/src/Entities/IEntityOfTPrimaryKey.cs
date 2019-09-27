namespace Confetti.Domain.Entities
{
    /// <summary>
    /// Provides an abstraction of base entity with primary key.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    public interface IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Gets or sets unique identifier for this entity.
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}