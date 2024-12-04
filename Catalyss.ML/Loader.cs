using Catalyss.ML;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
[assembly: MelonInfo(typeof(Loader), "Catalyss Loader", "1.0", "Scrimmane (Scrim)")]
[assembly: MelonGame("KisSoft", "ATLYSS")]
[assembly: MelonPriority(0)]
[assembly: MelonColor(ConsoleColor.Magenta)]
[assembly: MelonAuthorColor(ConsoleColor.DarkMagenta)]

namespace Catalyss.ML
{
    public class Loader : MelonMod
    {
        private static string dllLoc = $"{MelonUtils.GameDirectory}\\Catalyss";
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Loader ready!");

            if (!Directory.Exists(dllLoc))
            {
                try { Directory.CreateDirectory(dllLoc); } 
                catch { MelonLogger.Error("Failed to load directory."); }
            }
        }

        public override void OnLateInitializeMelon()
        {
            LoadDll($"{dllLoc}\\CatalyssMod.dll");
        }

        public override void OnApplicationQuit()
        {
            Process.GetCurrentProcess().Kill(); //Faster close
        }

        internal static void LoadDll(string dllPath)
        {
            MelonLogger.Msg("Loader Initialized!");

            if (File.Exists(dllPath))
            {
                try
                {
                    Assembly loadedAssembly = Assembly.LoadFile(dllPath);

                    Type entryType = loadedAssembly.GetType("CatalyssMod.Entry");
                    MethodInfo entryMethod = entryType?.GetMethod("Load", BindingFlags.Public | BindingFlags.Static);

                    if (entryMethod != null)
                    {
                        MelonLogger.Msg($"Found and invoking entry point.");
                        entryMethod.Invoke(null, null);
                    }
                    else
                    {
                        MelonLogger.Warning($"No entry point found. Loaded without invoking.");
                    }
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"Failed to load {dllPath}: {ex.Message}");
                }
            }
            else
            {
                MelonLogger.Error($"DLL not found: {dllPath}");
            }
        }
    }
}
