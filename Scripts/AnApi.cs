using System;

namespace Moyba.Contracts
{
    public partial class AnApi<TManager, IApi> : ATrait<TManager>
        where TManager : AManager
        where IApi : class
    {
        [NonSerialized] private readonly IApi _this;
#if UNITY_EDITOR
        [NonSerialized] private readonly Func<TManager, IApi> _GetApiFromManager;
#endif
        [NonSerialized] private readonly Action<TManager, IApi> _SetApiOnManager;

        private AnApi() => _this = (IApi)(object)this;
        protected AnApi(System.Linq.Expressions.Expression<Func<TManager, IApi>> getApiFromManager) : this()
        {
            var apiProperty = _Reflection_ApiPropertyInfo(getApiFromManager);
#if UNITY_EDITOR
            _GetApiFromManager = _Reflection_GetApiFromManager(apiProperty);
#endif
            _SetApiOnManager = _Reflection_SetApiOnManager(apiProperty);
        }

        protected virtual void Awake()
        {
#if UNITY_EDITOR
            // try to catch issues like duplicate traits in the same scope, overwriting each other
            var current = _GetApiFromManager(_manager);
            this._Assert(current == null, $"is replacing a non-null trait {current?.GetType().Name}.");
#endif

            _SetApiOnManager(_manager, _this);
        }

        protected virtual void OnDestroy()
        {
            _SetApiOnManager(_manager, null);
        }
    }

    public partial class AnApi<TManager, TEntity, IApi> : ATrait<TManager, TEntity>
        where TManager : AManager
        where TEntity : AnEntity<TManager, TEntity>
        where IApi : class
    {
        [NonSerialized] private readonly IApi _this;
#if UNITY_EDITOR
        [NonSerialized] private readonly Func<TEntity, IApi> _GetApiFromEntity;
#endif
        [NonSerialized] private readonly Action<TEntity, IApi> _SetApiOnEntity;

        protected AnApi(System.Linq.Expressions.Expression<Func<TEntity, IApi>> getApiFromEntity)
        {
            _this = (IApi)(object)this;

            var apiProperty = _Reflection_ApiPropertyInfo(getApiFromEntity);
#if UNITY_EDITOR
            _GetApiFromEntity = _Reflection_GetApiFromEntity(apiProperty);
#endif
            _SetApiOnEntity = _Reflection_SetApiOnEntity(apiProperty);
        }

        protected virtual void Awake()
        {
#if UNITY_EDITOR
            // try to catch issues like duplicate traits in the same scope, overwriting each other
            var current = _GetApiFromEntity(_entity);
            this._Assert(current == null, $"is replacing a non-null trait {current?.GetType().Name}.");
#endif

            _SetApiOnEntity(_entity, _this);
        }

        protected virtual void OnDestroy()
        {
            _SetApiOnEntity(_entity, null);
        }
    }
}
