using UnityEngine;

namespace CatalyssMod
{
    public class Entry
    {
        public static GameObject? ModObj;
        public const string ModVersion = "1.6";
        public static Rect GuiRect = new Rect(15, 15, 700, 600);
        public static bool GuiTog { get; set; } = false;
        public static bool DebuggerGUI { get; set; } = false;

        public static void Load()
        {
            ModObj = new GameObject();
            ModObj.AddComponent<Utils>();
            ModObj.AddComponent<CatalyssMonoMod>();
            ModObj.AddComponent<ExtraGUIs>();
            Object.DontDestroyOnLoad(ModObj);
        }

        public static void Unload() { Object.Destroy(ModObj); }
    }
}
