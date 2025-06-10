using System;
using System.Collections.Generic;

namespace Moyba.Contracts
{
    /// <remarks>
    /// A partial signature to allow type scoping without knowing the full generic signature.
    /// </remarks>
    public abstract class _ExCollection<TEntity>
        where TEntity : _AnEntity
    {
        internal _ExCollection() { }

        internal abstract void Add(TEntity entity);
        internal abstract void Remove(TEntity entity);
    }

    public partial class ExCollection<TKey, TEntity> : _ExCollection<TEntity>, IExCollection<TKey, TEntity>
        where TEntity : _AnEntity
    {
        private static readonly Func<TEntity, TKey> _GetKeyFromEntity;

        [NonSerialized] private readonly IDictionary<TKey, TEntity> _entities = new Dictionary<TKey, TEntity>();

        static ExCollection()
        {
            _GetKeyFromEntity = _Reflection_GetKeyFromEntity();
        }

        public event ActionEventHandler<TEntity> OnAdded;
        public event ActionEventHandler<TEntity> OnRemoved;

        public TEntity this[TKey key]
        => _entities[key];

        public bool ContainsKey(TKey key)
        => _entities.ContainsKey(key);

        internal override void Remove(TEntity entity)
        {
            var key = _GetKeyFromEntity(entity);
            entity._Assert(
                _entities.ContainsKey(key) && ReferenceEquals(entity, _entities[key]),
                $"trying to deregister with a mismatched key '{key}'.");

            _entities.Remove(key);

            this.OnRemoved?.Invoke(entity);
        }

        internal override void Add(TEntity entity)
        {
            var key = _GetKeyFromEntity(entity);
            entity._Assert(!_entities.ContainsKey(key), $"trying to register with an existing key '{key}'.");

            _entities.Add(key, entity);

            this.OnAdded?.Invoke(entity);
        }
    }
}
