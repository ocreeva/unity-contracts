using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Moyba.Contracts
{
    public partial class Omnibus
    {
        /// <remarks>
        /// The feature-specific pattern typically looks like this:
        ///   [SerializeField, Require(typeof(IFeatureManager))] private Object _Feature;
        ///   public static IFeatureManager Feature { get; private set; }
        ///
        /// This method generates an action which finds all such property/field pairs, and assigns the field to the
        /// property after casting.
        /// </remarks>
        private static Action<Omnibus> _Reflection_AssignManagerProperties()
        {
            var omnibusType = typeof(Omnibus);
            var omnibusParameter = Expression.Parameter(omnibusType);

            var assignPropertyExpressions = omnibusType.GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(propertyInfo => propertyInfo.PropertyType.IsInterface)
                .Where(propertyInfo => propertyInfo.PropertyType.Name.Equals($"I{propertyInfo.Name}Manager"))
                .Select(propertyInfo =>
                {
                    var fieldInfo = omnibusType.GetField($"_{propertyInfo.Name}", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (fieldInfo == null) return null;

                    return Expression.Assign(
                        Expression.Property(null, propertyInfo),
                        Expression.Convert(
                            Expression.Field(omnibusParameter, fieldInfo),
                            propertyInfo.PropertyType
                        )
                    );
                })
                .Where(expression => expression != null);

            return Expression.Lambda<Action<Omnibus>>(
                Expression.Block(assignPropertyExpressions),
                omnibusParameter
            ).Compile();
        }

#if UNITY_EDITOR
        /// <remarks>
        /// The feature-specific pattern typically looks like this:
        ///   [SerializeField, Require(typeof(IFeatureManager))] private Object _Feature;
        ///   public static IFeatureManager Feature { get; private set; }
        /// 
        /// The default feature manager is typically located at the path:
        ///   /Assets/<feature>/<feature> Manager.asset
        ///
        /// This method generates an action which finds all such fields and attempts to set them to their default
        /// manager asset.
        /// </remarks>
        private static Action<Omnibus> _Reflection_ResetManagerFields()
        {
            var omnibusType = typeof(Omnibus);
            var omnibusParameter = Expression.Parameter(omnibusType);

            var assignFieldExpressions = omnibusType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(fieldInfo => fieldInfo.GetCustomAttribute<UnityEngine.SerializeField>() != null)
                .Select(fieldInfo =>
                {
                    var requireAttribute = fieldInfo.GetCustomAttribute<RequireAttribute>();
                    if (requireAttribute == null) return null;

                    return Expression.Assign(
                        Expression.Field(omnibusParameter, fieldInfo),
                        Expression.TypeAs(
                            Expression.Call(
                                omnibusType,
                                nameof(_LoadManagerAsset),
                                new[] { requireAttribute.Type }),
                            fieldInfo.FieldType));
                })
                .Where(expression => expression != null);

            return Expression.Lambda<Action<Omnibus>>(
                Expression.Block(assignFieldExpressions),
                omnibusParameter
            ).Compile();
        }
#endif
    }
}
