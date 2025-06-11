using UnityEngine;

namespace Moyba.Contracts
{
    public abstract partial class ATrait<TManager> : AContract
        where TManager : AManager
    {
        [SerializeField] protected internal TManager _manager;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            _manager = _LoadManagerAsset<TManager>();
        }
#endif
    }

    public abstract partial class ATrait<TManager, TEntity> : AContract
        where TManager : AManager
        where TEntity : AnEntity<TManager, TEntity>
    {
        [SerializeField] protected internal TEntity _entity;
        protected internal TManager _manager => _entity._manager;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            _entity = this.GetComponent<TEntity>() ?? this.GetComponentInParent<TEntity>();
        }
#endif
    }
}
