using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Moyba.Contracts
{
    internal static class AnApi_Reflection<TContract, IApi>
    {
#if UNITY_EDITOR
        /// <remarks>
        /// This method generates a function to get the current value of the API property on its parent contract.
        /// </remarks>
        public static Func<TContract, IApi> GetApiFromContract(PropertyInfo apiPropertyInfo)
        {
            var contractParameter = Expression.Parameter(typeof(TContract));

            return Expression.Lambda<Func<TContract, IApi>>(
                Expression.Property(contractParameter, apiPropertyInfo),
                contractParameter
            ).Compile();
        }
#endif

        /// <remarks>
        /// This method generates a function to set the current value of the API property on its parent contract.
        /// </remarks>
        public static Action<TContract, IApi> SetApiOnContract(PropertyInfo apiPropertyInfo)
        {
            var contractParameter = Expression.Parameter(typeof(TContract));
            var apiParameter = Expression.Parameter(typeof(IApi));

            return Expression.Lambda<Action<TContract, IApi>>(
                Expression.Assign(Expression.Property(contractParameter, apiPropertyInfo), apiParameter),
                contractParameter,
                apiParameter
            ).Compile();
        }

        /// <remarks>
        /// APIs should be exposed on their manager/entity as an interface property, for facilitating cross-feature
        /// calls. The class constructor is required to provide an expression which illustrates where the property is
        /// located on its parent contract, which looks like this:
        ///   _ => _.Api
        /// 
        /// This method extracts the <see cref="PropertyInfo" /> from the expression.
        /// </remarks>
        public static PropertyInfo ApiPropertyInfo(Expression<Func<TContract, IApi>> getApiFromManager)
        {
            var memberExpression = (MemberExpression)getApiFromManager.Body;
            return (PropertyInfo)memberExpression.Member;
        }
    }

    public partial class AnApi<TManager, IApi>
    {
#if UNITY_EDITOR
        private static Func<TManager, IApi> _Reflection_GetApiFromManager(PropertyInfo apiPropertyInfo)
        => AnApi_Reflection<TManager, IApi>.GetApiFromContract(apiPropertyInfo);
#endif

        private static Action<TManager, IApi> _Reflection_SetApiOnManager(PropertyInfo apiPropertyInfo)
        => AnApi_Reflection<TManager, IApi>.SetApiOnContract(apiPropertyInfo);

        private static PropertyInfo _Reflection_ApiPropertyInfo(Expression<Func<TManager, IApi>> getApiFromManager)
        => AnApi_Reflection<TManager, IApi>.ApiPropertyInfo(getApiFromManager);
    }

    public partial class AnApi<TManager, TEntity, IApi>
    {
#if UNITY_EDITOR
        private static Func<TEntity, IApi> _Reflection_GetApiFromEntity(PropertyInfo apiPropertyInfo)
        => AnApi_Reflection<TEntity, IApi>.GetApiFromContract(apiPropertyInfo);
#endif

        private static Action<TEntity, IApi> _Reflection_SetApiOnEntity(PropertyInfo apiPropertyInfo)
        => AnApi_Reflection<TEntity, IApi>.SetApiOnContract(apiPropertyInfo);

        private static PropertyInfo _Reflection_ApiPropertyInfo(Expression<Func<TEntity, IApi>> getApiFromEntity)
        => AnApi_Reflection<TEntity, IApi>.ApiPropertyInfo(getApiFromEntity);
    }
}
