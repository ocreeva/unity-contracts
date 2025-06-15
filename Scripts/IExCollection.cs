using System.Collections.Generic;

namespace Moyba.Contracts
{
    /// <summary>
    /// Represents a collection of entities.
    /// </summary>
    /// <typeparam name="IEntity">The entity interface.</typeparam>
    public interface IExCollection<out IEntity> : IEnumerable<IEntity>
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
    }

    /// <summary>
    /// Represents a keyed collection of entities.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="IEntity">The entity interface.</typeparam>
    public interface IExCollection<in TKey, out IEntity> : IExCollection<IEntity>
        where IEntity : class
    {
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

    /// <summary>
    /// Represents a dual-keyed collection of entities.
    /// </summary>
    /// <typeparam name="TKey1">The first part of the key type.</typeparam>
    /// <typeparam name="TKey2">The second part of the key type.</typeparam>
    /// <typeparam name="IEntity">The entity interface.</typeparam>
    public interface IExCollection<in TKey1, in TKey2, out IEntity> : IExCollection<IEntity>
        where IEntity : class
    {
        /// <summary>
        /// Get an entity by dual-key.
        /// </summary>
        /// <param name="key1">The first part of the key.</param>
        /// <param name="key2">The second part of the key.</param>
        /// <returns>The entity.</returns>
        IEntity this[TKey1 key1, TKey2 key2] { get; }

        /// <summary>
        /// Determines whether an entity with a dual-key is contained in the collection.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <returns><c>true</c> if an entity with the provided key is in the collection; otherwise, <c>false</c>.</returns>
        bool ContainsKey(TKey1 key1, TKey2 key2);
    }
}
