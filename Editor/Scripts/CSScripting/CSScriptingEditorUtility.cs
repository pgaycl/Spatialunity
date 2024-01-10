using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.Build.Player;
using System.Security.Cryptography;
using System.Text;
using SpatialSys.UnitySDK.Internal;

namespace SpatialSys.UnitySDK.Editor
{
    public static class CSScriptingEditorUtility
    {
        public static readonly string OUTPUT_ASSET_PATH = Path.Combine(EditorUtility.AUTOGENERATED_ASSETS_DIRECTORY, CSScriptingUtility.DEFAULT_CSHARP_ASSEMBLY_NAME + ".dll.txt");

        private const string COMPILE_DESTINATION_DIR = "Temp/CSScriptingCompiledDlls";

        public static void EnforceCustomAssemblyName(AssemblyDefinitionAsset assemblyDefinition, string sku)
        {
            string asmDefAssetPath = AssetDatabase.GetAssetPath(assemblyDefinition);
            string asmDefOriginal = File.ReadAllText(asmDefAssetPath);
            string assemblyName = GetAssemblyNameForSKU(sku);
            string asmDefModified = Regex.Replace(asmDefOriginal, "\"name\":\\s*\".*?\",", $"\"name\": \"{assemblyName}\",");
            File.WriteAllText(asmDefAssetPath, asmDefModified);
            AssetDatabase.ImportAsset(asmDefAssetPath);
        }

        public static bool ValidateCustomAssemblyName(AssemblyDefinitionAsset assemblyDefinition, string sku)
        {
            return GetAssemblyName(assemblyDefinition) == GetAssemblyNameForSKU(sku);
        }

        private static string Sha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private static string GetAssemblyNameForSKU(string sku)
        {
            switch (sku == null ? "" : Sha256Hash(sku))
            {
                case "71303bc05617d4e9aa08596f62b1aa3ebb74ffc0000d81d2d0e76df15ab2c088":
                    return "Placeholder1";
                case "bab100648c3f568cb4b14f7765ae46cf50e973819fc972e61e25d703c135fd88":
                    return "Placeholder2";
                case "d028dbd5e554c43f2c33d58603af76c6e70b5c3b99fbcdce157e343718047310":
                    return "Placeholder3";
                case "1b6ab678554cbf1db00532dcb14d5965f3cb97e66f2d31ba4ee7c8774940176a":
                    return "Placeholder4";
                case "d8831920b04b56a3dee9f5af1bc1666d98ac350cf91d21bac206ba35b3707e8e":
                    return "Placeholder2";
                case "da2c04554cb7595da4d6a242afcdfa773d684010919e3986d930310023dbe4d0":
                    return "Placeholder1";
                case "4f5de27d999b69eb282bcde9a11def038d17ad17b0bb6f1c0006890c7422ab04":
                    return "Placeholder3";
                case "d39823ecc528ddd94b75bd0dd40147e167c6f400fee9514653e511182b039a8e":
                    return "Placeholder5";
                case "75007390cd6064ade0a043c92984fb1d995c50ec3355de015f8cddb433656ac6":
                    return "Placeholder6";
                case "7344b73a96d76dfdf10a42d57ebdb8f972387ba534f8a71466e713eb8e428c8e":
                    return "Placeholder7";
                case "0b8bce1bbb354b351976de22d4087b49308f67a69f136eb09eae7c249bedbe12":
                    return "Placeholder8";
                case "0c9e2615ed89abfdf6ebb816c4a7c57cb47d17c7d3e25cef27d34dc5f1bbfab6":
                    return "Placeholder9";
                default:
                    return CSScriptingUtility.DEFAULT_CSHARP_ASSEMBLY_NAME;
            }
        }

        public static bool CompileAssembly(AssemblyDefinitionAsset assemblyDefinition, string sku)
        {
            string assemblyName = GetAssemblyNameForSKU(sku);

            if (!ValidateCustomAssemblyName(assemblyDefinition, sku))
            {
                Debug.LogError($"Failed to compile c# assembly: Assembly name must be {assemblyName}; Did you forget to call EnforceCustomAssemblyName");
                return false;
            }

            // Compile
            try
            {
                BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
                BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
                ScriptCompilationSettings scriptCompilationSettings = new() {
                    target = buildTarget,
                    group = buildTargetGroup,
                    options = ScriptCompilationOptions.None,
                };

                string outputDir = Path.Combine(COMPILE_DESTINATION_DIR, buildTarget.ToString());
                if (!Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);

                PlayerBuildInterface.CompilePlayerScripts(scriptCompilationSettings, outputDir);

                // Copy dll to generated folder
                string dllPath = Path.Combine(outputDir, assemblyName + ".dll");
                if (File.Exists(dllPath))
                {
                    string dllAssetPathOutputDir = Path.GetDirectoryName(OUTPUT_ASSET_PATH);
                    if (!Directory.Exists(dllAssetPathOutputDir))
                        Directory.CreateDirectory(dllAssetPathOutputDir);

                    File.Copy(dllPath, OUTPUT_ASSET_PATH, true);
                    AssetDatabase.ImportAsset(OUTPUT_ASSET_PATH);
                    AssetDatabase.Refresh();
                    return true;
                }
                else
                {
                    Debug.LogError($"Failed to compile C# assembly; Output dll not found at {dllPath}\n{assemblyDefinition}");
                    return false;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to compile c# assembly: {e}\n{assemblyDefinition}");
                return false;
            }
        }

        public static string GetAssemblyName(AssemblyDefinitionAsset assemblyDefinition)
        {
            string txt = File.ReadAllText(AssetDatabase.GetAssetPath(assemblyDefinition));
            Match match = Regex.Match(txt, "\"name\":\\s*\"(.*?)\"");
            if (match.Success)
                return match.Groups[1].Value;

            return null;
        }
    }
}