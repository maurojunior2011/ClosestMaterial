using CloserMaterial.Info;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloserMaterial.Patches
{
    public class LocalizationPatch
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix() => Translate(root: typeof(InfoDataStrings));

            public static void Translate(Type root)
            {
                // Basic intended way to register strings, keeps namespace
                RegisterForTranslation(root);

                // Load user created translation files
                LoadStrings();

                // Register strings without namespace
                // because we already loaded user transltions, custom languages will overwrite these
                LocString.CreateLocStringKeys(root, null);
            }

            private static void LoadStrings()
            {
                string path = Path.Combine(ModPath, "translations", GetLocale()?.Code + ".po");
                if (File.Exists(path))
                    OverloadStrings(LoadStringsFile(path, false));
            }
        }
    }
}
