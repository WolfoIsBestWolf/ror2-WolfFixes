using System.IO;
using UnityEngine;

namespace WolfoLibrary
{
    public class AssetUtils
    {
        public static string GetPathToFile(string assemblyDir, string modFolder, string assetFolder)
        {
            if (Directory.Exists(Path.Combine(assemblyDir, modFolder + assetFolder)))
            {
                return Path.Combine(assemblyDir, modFolder + assetFolder);
            }
            else if (Directory.Exists(Path.Combine(assemblyDir, "plugins\\" + modFolder + assetFolder)))
            {
                return Path.Combine(assemblyDir, assetFolder);
            }
            else
            {
                Debug.LogError($"COULD NOT FIND {assetFolder} FOLDER");
            }
            return assemblyDir;
        }
        public static string GetPathToFile(string assemblyDir, string modFolder, string assetFolder, string fileName)
        {
            return Path.Combine(GetPathToFile(assemblyDir, modFolder, assetFolder), fileName);
        }
    }

}
