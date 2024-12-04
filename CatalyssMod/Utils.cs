using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace CatalyssMod
{
    internal class Utils : MonoBehaviour
    {
        //Helper class for other game stuff
        private void Start()
        {
            CatalyssMonoMod.LoopGM = false;
            CatalyssMonoMod.LoopStam = false;
        }

        private void Update()
        {
            if (CatalyssMonoMod.LoopGM)
            {
                try
                {
                    CatalyssMonoMod.GetPlayer().GetComponentInChildren<StatusEntity>()._currentHealth = 9999;
                    CatalyssMonoMod.GetPlayer().GetComponentInChildren<StatusEntity>().Network_currentHealth = 9999;
                    CatalyssMonoMod.GetPlayer().GetComponentInChildren<StatusEntity>().Add_Health(9999);
                }
                catch { }
            }

            if (CatalyssMonoMod.LoopStam)
            {
                try
                {
                    CatalyssMonoMod.GetPlayer().GetComponentInChildren<StatusEntity>().Change_Stamina(9999);
                    CatalyssMonoMod.GetPlayer().GetComponentInChildren<StatusEntity>().Network_currentStamina = 9999;
                    CatalyssMonoMod.GetPlayer().GetComponentInChildren<StatusEntity>()._currentStamina = 9999;
                }
                catch { }
            }
        }

        private void OnGUI() { }
    }
}