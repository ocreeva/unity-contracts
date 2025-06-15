using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Moyba.Contracts
{
    public partial class _ExCollection<TEntity>
    {
        /// <remarks>
        /// Entities are expected to define a property or field which provides their collection key, indicated by the
        /// Key attribute.
        /// 
        /// This method locates the entity type's field or property matching the key constraints, and generates a
        /// function to retrieve it.
        /// </remarks>
        internal static Func<TEntity, TKey> _Reflection_GetKeyFromEntity<TKey>()
        {
            var entityType = typeof(TEntity);
            var entityParameter = Expression.Parameter(entityType);

            var keyFieldExpressions = entityType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(fieldInfo => fieldInfo.GetCustomAttribute<KeyAttribute>() != null)
                .Where(fieldInfo => typeof(TKey).IsAssignableFrom(fieldInfo.FieldType))
                .Select(fieldInfo => Expression.Field(entityParameter, fieldInfo));
            var keyPropertyExpressions = entityType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(propertyInfo => propertyInfo.GetCustomAttribute<KeyAttribute>() != null)
                .Where(propertyInfo => typeof(TKey).IsAssignableFrom(propertyInfo.PropertyType))
                .Select(propertyInfo => Expression.Property(entityParameter, propertyInfo));

            return Expression.Lambda<Func<TEntity, TKey>>(
                keyFieldExpressions.Concat(keyPropertyExpressions).Single(),
                entityParameter
            ).Compile();
        }
    }
}
