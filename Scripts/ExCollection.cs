using System;
using System.Collections;
using System.Collections.Generic;

namespace Moyba.Contracts
{
    /// <remarks>
    /// A partial signature to allow type scoping without knowing the full generic signature.
    /// </remarks>
    public abstract partial class _ExCollection<TEntity> : IExCollection<TEntity>
        where TEntity : _AnEntity
    {
        internal _ExCollection() { }

        public event ActionEventHandler<TEntity> OnAdded;
        public event ActionEventHandler<TEntity> OnRemoved;

        public abstract IEnumerator<TEntity> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        internal virtual void Add(TEntity entity) => this.OnAdded?.Invoke(entity);
        internal virtual void Remove(TEntity entity) => this.OnRemoved?.Invoke(entity);
    }

    public partial class ExCollection<TKey, TEntity> : _ExCollection<TEntity>, IExCollection<TKey, TEntity>
        where TEntity : _AnEntity
    {
        private static readonly Func<TEntity, TKey> _GetKeyFromEntity;

        [NonSerialized] private readonly IDictionary<TKey, TEntity> _entities = new Dictionary<TKey, TEntity>();

        static ExCollection()
        {
            _GetKeyFromEntity = _Reflection_GetKeyFromEntity<TKey>();
        }

        public TEntity this[TKey key]
        => _entities[key];

        public bool ContainsKey(TKey key)
        => _entities.ContainsKey(key);

        public sealed override IEnumerator<TEntity> GetEnumerator()
        => _entities.Values.GetEnumerator();

        internal override void Add(TEntity entity)
        {
            var key = _GetKeyFromEntity(entity);
            entity._Assert(!_entities.ContainsKey(key), $"trying to register with an existing key '{key}'.");

            _entities.Add(key, entity);

            base.Add(entity);
        }

        internal override void Remove(TEntity entity)
        {
            var key = _GetKeyFromEntity(entity);
            entity._Assert(
                _entities.ContainsKey(key) && ReferenceEquals(entity, _entities[key]),
                $"trying to deregister with a mismatched key '{key}'.");

            _entities.Remove(key);

            base.Remove(entity);
        }
    }

    public partial class ExCollection<TKey1, TKey2, TEntity> : _ExCollection<TEntity>, IExCollection<TKey1, TKey2, TEntity>
        where TEntity : _AnEntity
    {
        private static readonly Func<TEntity, TKey1> _GetKey1FromEntity;
        private static readonly Func<TEntity, TKey2> _GetKey2FromEntity;

        [NonSerialized] private readonly IDictionary<(TKey1, TKey2), TEntity> _entities = new Dictionary<(TKey1, TKey2), TEntity>();

        static ExCollection()
        {
            _GetKey1FromEntity = _Reflection_GetKeyFromEntity<TKey1>();
            _GetKey2FromEntity = _Reflection_GetKeyFromEntity<TKey2>();
        }

        public TEntity this[TKey1 key1, TKey2 key2]
        => _entities[(key1, key2)];

        public bool ContainsKey(TKey1 key1, TKey2 key2)
        => _entities.ContainsKey((key1, key2));

        public sealed override IEnumerator<TEntity> GetEnumerator()
        => _entities.Values.GetEnumerator();

        internal override void Add(TEntity entity)
        {
            var key = (_GetKey1FromEntity(entity), _GetKey2FromEntity(entity));
            entity._Assert(!_entities.ContainsKey(key), $"trying to register with an existing key '{key}'.");

            _entities.Add(key, entity);

            base.Add(entity);
        }

        internal override void Remove(TEntity entity)
        {
            var key = (_GetKey1FromEntity(entity), _GetKey2FromEntity(entity));
            entity._Assert(
                _entities.ContainsKey(key) && ReferenceEquals(entity, _entities[key]),
                $"trying to deregister with a mismatched key '{key}'.");

            _entities.Remove(key);

            base.Remove(entity);
        }
    }
}
