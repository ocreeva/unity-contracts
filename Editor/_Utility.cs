using System;
using UnityEditor;

namespace Moyba.Contracts.Editor
{
    internal static class _Utility
    {
        private const string _DefaultRootNamespace = "Moyba";

        public static string ContractsNamespace => "Moyba.Contracts";

        public static string GetRootNamespace()
        {
            // try to get the root namespace from the Editor settings
            var rootNamespace = EditorSettings.projectGenerationRootNamespace;

            // fall back to the default root namespace
            return String.IsNullOrEmpty(rootNamespace) ? _DefaultRootNamespace : rootNamespace;
        }
    }
}
