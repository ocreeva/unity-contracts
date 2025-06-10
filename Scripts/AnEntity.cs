using System;
using UnityEngine;

namespace Moyba.Contracts
{
    /// <remarks>
    /// A partial signature to allow type scoping without knowing the full generic signature.
    /// </remarks>
    public abstract class _AnEntity : AContract
    {
        internal _AnEntity() { }
    }

    public abstract class AnEntity<TManager, TEntity> : _AnEntity
        where TManager : AManager
        where TEntity : AnEntity<TManager, TEntity>
    {
        [NonSerialized] private readonly TEntity _this;
        [NonSerialized] private readonly Func<TManager, _ExCollection<TEntity>> _GetCollectionFromManager;

        [SerializeField] private TManager _manager;

        protected AnEntity(Func<TManager, _ExCollection<TEntity>> getCollectionFromManager)
        {
            _this = (TEntity)this;
            _GetCollectionFromManager = getCollectionFromManager;
        }

        protected virtual void OnDisable()
        {
            var collection = _GetCollectionFromManager(_manager);
            collection.Remove(_this);
        }

        protected virtual void OnEnable()
        {
            var collection = _GetCollectionFromManager(_manager);
            collection.Add(_this);
        }

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            _manager = _LoadManagerAsset<TManager>();
        }
#endif
    }
}
