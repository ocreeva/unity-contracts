namespace Moyba.Contracts
{
    /// <summary>
    /// Represents a keyed collection of entities.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="IEntity">The entity interface.</typeparam>
    public interface IExCollection<in TKey, out IEntity>
        where IEntity : class
    {
        /// <summary>
        /// Occurs when an entity is added to the collection.
        /// </summary>
        event ActionEventHandler<IEntity> OnAdded;

        /// <summary>
        /// Occurs when an entity is removed from the collection.
        /// </summary>
        event ActionEventHandler<IEntity> OnRemoved;

        /// <summary>
        /// Get an entity by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The entity.</returns>
        IEntity this[TKey key] { get; }

        /// <summary>
        /// Determines whether an entity with a key is contained in the collection.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if an entity with the provided key is in the collection; otherwise, <c>false</c>.</returns>
        bool ContainsKey(TKey key);
    }
}
