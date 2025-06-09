using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Moyba.Contracts.Editor
{
    public class ScriptTemplateProcessor : AssetModificationProcessor
    {
        private const string _NamespaceParameter = "#NAMESPACE#";

        private const string _ContractsDirectoryName = "Contracts";
        private const string _CoreDirectoryName = "Core";
        private const string _EditorDirectoryName = "Editor";
        private const string _ScriptsDirectoryName = "Scripts";

        private const string _ScriptMetadataFileExtension = ".cs.meta";

        public static void OnWillCreateAsset(string assetPath)
        {
            // only process C# script metadata files
            if (!assetPath.EndsWith(_ScriptMetadataFileExtension)) return;

            // get the path of the C# script, and parse it
            var scriptPath = AssetDatabase.GetAssetPathFromTextMetaFilePath(assetPath);
            var scriptName = Path.GetFileNameWithoutExtension(scriptPath);
            var directorySegments = Path.GetDirectoryName(scriptPath).Split(Path.DirectorySeparatorChar);
            var featureHierarchy = _GetFeatureHierarchy(directorySegments);
            var isEditorScript = directorySegments.Any(s => s.Equals(_EditorDirectoryName));

            // create a lookup for template values
            var templateParameters = new Dictionary<string, string>
            {
                { _NamespaceParameter, _GenerateNamespaceValue(featureHierarchy, isEditorScript) },
            };

            // read the script file
            var originalContent = File.ReadAllText(scriptPath);

            // replace the template values
            var updatedContent = originalContent;
            foreach ((var key, var value) in templateParameters) updatedContent = updatedContent.Replace(key, value);

            // check whether any template parameters were applied
            if (originalContent.Equals(updatedContent)) return;

            // update the script file
            var lastWriteTime = File.GetLastWriteTimeUtc(scriptPath);
            File.WriteAllText(scriptPath, updatedContent);

            // HACK: reset the last write time, lest AssetDatabase spame the console with warnings
            File.SetLastWriteTimeUtc(scriptPath, lastWriteTime);
        }

        private static string _GenerateNamespaceValue(string[] featureHierarchy, bool isEditorScript)
        {
            // get the root namespace for the project
            var rootNamespace = _Utility.GetRootNamespace();

            // prepend the root namespace to the feature hierarchy
            var namespaceSegments = featureHierarchy.Prepend(rootNamespace);

            // append an ending 'Editor' segment for Editor scripts
            if (isEditorScript) namespaceSegments = namespaceSegments.Append(_EditorDirectoryName);

            // generate the namespace
            return String.Join('.', namespaceSegments);
        }

        private static string[] _GetFeatureHierarchy(IEnumerable<string> directorySegments)
        {
            // strip the root 'Assets' directory; 'Assets' is common, and not relevant to the feature hierarchy
            directorySegments = directorySegments.Skip(1);

            // strip the 'Scripts' directory at the second level of the hierarchy; 'Scripts' is present for organizing
            // script files apart from other file types, but does not serve a purpose in the feature hierarchy
            directorySegments = directorySegments.Where((s, i) => i != 1 || !s.Equals(_ScriptsDirectoryName));

            // strip the top-level 'Core' directory; 'Core' scripts are considered common across the project, and use a
            // root feature hierarchy to help denote its lack of separation
            directorySegments = directorySegments.Where((s, i) => i != 0 || !s.Equals(_CoreDirectoryName));

            // strip all 'Editor' directories, anywhere in the hierarchy; 'Editor' is only denoted by the last segment
            // of the namespace, and is re-incorporated there when the namespace is generated
            directorySegments = directorySegments.Where(s => !s.Equals(_EditorDirectoryName));

            // strip the 'Contracts' directory at the second level of the remaining hierarchy; 'Contracts' are
            // considered the same namespace, but are kept in a separate directory since they're a different assembly
            directorySegments = directorySegments.Where((s, i) => i != 1 || !s.Equals(_ContractsDirectoryName));

            return directorySegments.ToArray();
        }
    }
}
