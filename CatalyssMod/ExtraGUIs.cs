using UnityEngine;

namespace CatalyssMod
{
    internal class ExtraGUIs : MonoBehaviour
    {
        private string inputText;
        private readonly float Offset = 2f;
        public static float S_SliderValue = 1f;

        public void OnGUI()
        {
            if (CatalyssMonoMod.HandleItemDropMenu)
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    CatalyssMonoMod.HandleItemDropMenu = false;
                }

                GUI.backgroundColor = Color.gray;
                GUI.contentColor = Color.magenta;
                GUI.color = Color.magenta;

                GUI.Box(new Rect(10, 25 * Offset, 350, 350), $"Game Items Menu");

                GUI.Label(new Rect(20, 40 * Offset, 100, 80), "Item to drop:\n(Increase slider for amount)");
                inputText = GUI.TextField(new Rect(120, 40 * Offset, 200, 20), inputText);
                S_SliderValue = GUI.HorizontalSlider(new Rect(125, 55 * Offset, 140, 30), S_SliderValue, 0f, 200f);

                if (GUI.Button(new Rect(20, 67 * Offset, 100, 30), "Submit"))
                {
                    Utils.DropNewItem(inputText, (int)S_SliderValue);
                }

                if (GUI.Button(new Rect(20, 85 * Offset, 100, 30), "Go Back"))
                {
                    CatalyssMonoMod.HandleItemDropMenu = false;
                }
            }

            if (Entry.DebuggerGUI) { }
        }
    }
}