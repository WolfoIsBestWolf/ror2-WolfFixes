using BepInEx;
using System.IO;
using System.Linq;
using UnityEngine;

namespace WolfoFixes
{
    //Taken from EnFucker, Thank you EnFucker
    public static class Assets
    {
        public static PluginInfo PluginInfo;
        public static string Folder = "Fixes\\";


        internal static string assemblyDir
        {
            get
            {
                return System.IO.Path.GetDirectoryName(PluginInfo.Location);
            }
        }

        internal static void Init(PluginInfo info)
        {
            PluginInfo = info;

            if (WConfig.cfgDisable.Value || !WConfig.cfgTextFixes.Value)
            {
                return;
            }
            On.RoR2.Language.SetFolders += SetFolders;
        }

        private static void SetFolders(On.RoR2.Language.orig_SetFolders orig, RoR2.Language self, System.Collections.Generic.IEnumerable<string> newFolders)
        {
            var dirs = System.IO.Directory.EnumerateDirectories(Path.Combine(GetPathToFile("Languages")), self.name);
            orig(self, newFolders.Union(dirs));
        }


        internal static string GetPathToFile(string folderName)
        {
            string path = Path.Combine(assemblyDir, Folder + folderName);
            if (Directory.Exists(path))
            {
                return path;
            }
            path = Path.Combine(assemblyDir, "plugins\\" + Folder + folderName);
            if (Directory.Exists(path))
            {
                return path;
            }
            else
            {
                Debug.LogError($"COULD NOT FIND {folderName} FOLDER");
            }
            return assemblyDir;
        }

    }
}