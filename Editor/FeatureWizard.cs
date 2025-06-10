using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Moyba.Contracts.Editor
{
    public class FeatureWizard : ScriptableWizard
    {
        private const string _ScriptNameTemplateParameter = "#SCRIPTNAME#";

        private const string _Assets = "Assets";
        private const string _Contracts = "Contracts";
        private const string _Other = "Other";
        private const string _Scripts = "Scripts";

        private static readonly string _ScriptTemplates = Path.Join(_Assets, "ScriptTemplates");
        private static readonly string _InterfaceTemplate = Path.Join(_ScriptTemplates, "3-Scripting__Interface-Interface.cs.txt");
        private static readonly string _OmnibusTemplate = Path.Join(_ScriptTemplates, _Other, "4x-Scripting__Omnibus (feature)-Omnibus_.cs.txt");
        private static readonly string _ManagerTemplate = Path.Join(_ScriptTemplates, "41-Scripting__Manager-Manager.cs.txt");
        private static readonly string _AssemblyInfoTemplate = Path.Join(_ScriptTemplates, "91-Scripting__Assembly Info-_AssemblyInfo.cs.txt");
        private static readonly string _AsmdefTemplate = Path.Join(_ScriptTemplates, "92-Scripting__Assembly Definition-Assembly.asmdef.txt");
        private static readonly string _AsmrefTemplate = Path.Join(_ScriptTemplates, "93-Scripting__Assembly Reference-AssemblyReference.asmref.txt");

        [SerializeField] private string _name;
        [SerializeField] private bool _shouldOverwrite;

        [MenuItem("Assets/Create/Scripting/Feature")]
        static void CreateWizard()
        {
            var wizard = ScriptableWizard.DisplayWizard<FeatureWizard>("Create Feature", "Create");
            wizard.helpString = "Provide a feature name.";
        }

        private void OnWizardCreate()
        {
            var featureFolder = _CreateFolder(_Assets, _name);
            var scriptsFolder = _CreateFolder(featureFolder, _Scripts);
            var contractsFolder = _CreateFolder(scriptsFolder, _Contracts);

            var rootNamespace = _Utility.GetRootNamespace();
            var asmdefPath = Path.Join(scriptsFolder, $"{rootNamespace}.{_name}.asmdef");
            _CreateFile(_AsmdefTemplate, asmdefPath);

            var assemblyInfoPath = Path.Join(scriptsFolder, "_AssemblyInfo.cs");
            _CreateFile(_AssemblyInfoTemplate, assemblyInfoPath);

            var asmrefPath = Path.Join(contractsFolder, $"{_Utility.ContractsNamespace}.asmref");
            _CreateFile(_AsmrefTemplate, asmrefPath);

            var managerInterfacePath = Path.Join(contractsFolder, $"I{_name}Manager.cs");
            _CreateFile(_InterfaceTemplate, managerInterfacePath);

            var managerPath = Path.Join(scriptsFolder, $"{_name}Manager.cs");
            _CreateFile(_ManagerTemplate, managerPath);

            var omnibusPath = Path.Join(contractsFolder, $"Omnibus_{_name}.cs");
            _CreateFile(_OmnibusTemplate, omnibusPath);
        }

        private void OnWizardUpdate()
        {
            this.isValid = !String.IsNullOrEmpty(_name);
            this.errorString = null;

            var index = _name?.IndexOfAny(Path.GetInvalidFileNameChars()) ?? -1;
            if (index != -1)
            {
                this.isValid = false;
                this.errorString = $"File name cannot contain '{_name[index]}'.";
            }
        }

        private static string _CreateFolder(string parentFolder, string newFolderName)
        {
            var path = Path.Join(parentFolder, newFolderName);

            if (!AssetDatabase.AssetPathExists(path)) AssetDatabase.CreateFolder(parentFolder, newFolderName);

            return path;
        }

        private void _CreateFile(string templatePath, string filePath)
        {
            if (AssetDatabase.AssetPathExists(filePath))
            {
                if (!_shouldOverwrite) return;

                AssetDatabase.DeleteAsset(filePath);
            }

            var template = AssetDatabase.LoadAssetAtPath<TextAsset>(templatePath);

            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var contents = template.text.Replace(_ScriptNameTemplateParameter, fileName);

            File.WriteAllText(filePath, contents);
            AssetDatabase.Refresh();
        }
    }
}
