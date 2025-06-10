#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;

namespace Moyba.Contracts
{
    public abstract class AContract : MonoBehaviour
    {
        internal AContract() { }

#if UNITY_EDITOR
        private const string _InterfacePrefix = "I";
        private const string _ManagerSuffix = "Manager";

        protected internal static TManager _LoadManagerAsset<TManager>()
            where TManager : class
        {
            var featureName = _GetFeatureName<TManager>();

            // organization convention: features have a self-named folder in the Assets directory
            // organization convention: the default manager asset for a feature is located in the feature folder
            // naming convention: the default manager asset is named '<feature> Manager'
            return _LoadAssetAtPath<TManager>("Assets", featureName, $"{featureName} Manager.asset");
        }

        private static string _GetFeatureName<TManager>()
            where TManager : class
        {
            var type = typeof(TManager);
            var name = type.Name;

            // naming convention: interfaces are prefixed with 'I'
            if (type.IsInterface && name.StartsWith(_InterfacePrefix)) name = name[_InterfacePrefix.Length..];

            // naming convention: manager classes are named '<feature>Manager'
            if (name.EndsWith(_ManagerSuffix)) name = name[..^_ManagerSuffix.Length];

            return name;
        }

        private static TAsset _LoadAssetAtPath<TAsset>(params string[] pathSegments)
            where TAsset : class
        {
            var path = Path.Combine(pathSegments);
            return AssetDatabase.LoadMainAssetAtPath(path) as TAsset;
        }
#endif
    }
}
