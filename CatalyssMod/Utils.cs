using Mirror;
using UnityEngine;

namespace CatalyssMod
{
    internal class Utils : MonoBehaviour
    {
        //Helper class for other game stuff
        public static Player GetPlayer() { return Player._mainPlayer; }

        private void Awake() { Application.targetFrameRate = 999; }

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
            }

            if(CatalyssMonoMod.InfManaTog)
            {
                GetPlayer().GetComponentInChildren<StatusEntity>().Change_Mana(9999);
                GetPlayer().GetComponentInChildren<StatusEntity>()._manaRegenRate = 9999;
            }

            if (CatalyssMonoMod.PlyrRotate)
            {
                //To do
            }
        }

        public static void Spin(float speed_val)
        {
            if (GetPlayer() != null)
            {
                GetPlayer().GetComponentInChildren<PlayerVisual>().Rpc_RandomSpinPlayerModel(speed_val);
            }
        }

        public static void SendFX(int opt)
        {
            if (GetPlayer() != null)
            {
                switch(opt)
                {
                    case 0:
                        GetPlayer().GetComponentInChildren<PlayerVisual>().Rpc_VanitySparkleEffect();
                        break;
                    case 1:
                        GetPlayer().GetComponentInChildren<PlayerVisual>().Rpc_PoofSmokeEffect();
                        break;
                    case 2:
                        GetPlayer().GetComponentInChildren<PlayerVisual>().Rpc_PlayTeleportEffect();
                        break;
                    default:
                        GetPlayer().GetComponentInChildren<PlayerVisual>().Rpc_VanitySparkleEffect();
                        GetPlayer().GetComponentInChildren<PlayerVisual>().Rpc_PoofSmokeEffect();
                        GetPlayer().GetComponentInChildren<PlayerVisual>().Rpc_PlayTeleportEffect();
                        break;
                }
            }
        }

        public static void DropNewItem(string itemname, int amount)
        {
            try
            {
                var item = new ItemData()
                {
                    _slotNumber = 0,
                    _itemName = itemname,
                    _isEquipped = false,
                    _maxQuantity = amount
                };
                GetPlayer().GetComponentInChildren<PlayerInventory>().Add_Item(item);
                GetPlayer().GetComponentInChildren<PlayerInventory>().Cmd_DropItem(item, amount);
            }
            catch { return; }
        }

        public static void Test(float jumpforce, float forwardforce, float gravmultiplier)
        {
            try { GetPlayer().GetComponentInChildren<PlayerMove>().Init_Jump(jumpforce, forwardforce, gravmultiplier); }
            catch { return; }
        }

        private void OnGUI() { GUI.Label(new Rect(15, 10, 2000, 30), $"{Application.unityVersion}"); }
    }
}