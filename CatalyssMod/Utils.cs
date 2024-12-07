using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

namespace CatalyssMod
{
    internal class Utils : MonoBehaviour
    {
        //Helper class for other game stuff
        public static Player GetPlayer() { return Player._mainPlayer; }

        private void Awake()
        {
            //To do
        }

        private void Start()
        {
            CatalyssMonoMod.LoopGM = false;
            CatalyssMonoMod.LoopStam = false;
        }

        private void Update()
        {
            //Loops
            try
            {
                if (CatalyssMonoMod.LoopGM)
                {
                    GetPlayer().GetComponentInChildren<StatusEntity>()._currentHealth = 9999;
                    GetPlayer().GetComponentInChildren<StatusEntity>().Network_currentHealth = 9999;
                    GetPlayer().GetComponentInChildren<StatusEntity>().Add_Health(9999);
                }

                if (CatalyssMonoMod.LoopStam)
                {
                    GetPlayer().GetComponentInChildren<StatusEntity>().Change_Stamina(9999);
                    GetPlayer().GetComponentInChildren<StatusEntity>().Network_currentStamina = 9999;
                    GetPlayer().GetComponentInChildren<StatusEntity>()._currentStamina = 9999;
                }
            }
            catch { }

            if (CatalyssMonoMod.AutoRez)
            {
                if (GetPlayer().GetComponentInChildren<StatusEntity>().Network_currentHealth < 1 ||
                    GetPlayer().GetComponentInChildren<StatusEntity>()._currentHealth < 1)
                {
                    GetPlayer().GetComponentInChildren<StatusEntity>().Cmd_RevivePlayer(Player._mainPlayer);
                    GetPlayer().GetComponentInChildren<StatusEntity>().Cmd_ReplenishAll();
                }
            }

            if (CatalyssMonoMod.SpinBotPlayer)
            {
                Spin(500);
            }

            if(CatalyssMonoMod.IframePlyr)
            {
                GetPlayer().GetComponentInChildren<Player>()._inIFrame = true;
                GetPlayer().GetComponentInChildren<Player>().Set_IFrame(20);
            }
            else
            {
                GetPlayer().GetComponentInChildren<Player>()._inIFrame = false;
                GetPlayer().GetComponentInChildren<Player>().Set_IFrame(0);
            }

            if(CatalyssMonoMod.InfManaTog)
            {
                GetPlayer().GetComponentInChildren<StatusEntity>().Change_Mana(9999);
                GetPlayer().GetComponentInChildren<StatusEntity>()._manaRegenRate = 9999;
            }
        }

        public static void Spin(float speed_val)
        {
            if (GetPlayer() != null)
            {
                GetPlayer().GetComponentInChildren<PlayerVisual>().Rpc_RandomSpinPlayerModel(speed_val);
            }
        }

        public static void JoinPlyrByID()
        {
            try 
            { 
                string s = File.ReadAllText($"{Directory.GetCurrentDirectory}\\PlayerSteamID.txt");
                if (s.Length > 0)
                {

                }
            }
            catch { return; }
        }

        private void OnGUI() { GUI.Label(new Rect(15, 10, 2000, 30), $"{Application.unityVersion}"); }
    }
}