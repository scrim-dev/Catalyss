using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

namespace CatalyssMod
{
    internal class CatalyssMonoMod : MonoBehaviour
    {
        public static GameObject GetPlayer()
        {
            //Should fix multiplayer
            if (Player._mainPlayer.Network_isHostPlayer)
            {
                try
                {
                    return GameObject.Find($"[connID: 0] _player({Player._mainPlayer._nickname})");
                }
                catch { return Player._mainPlayer.gameObject; }
            }
            else
            {
                try
                {
                    return GameObject.Find($"_player({Player._mainPlayer._nickname})");
                }
                catch { return Player._mainPlayer.gameObject; }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Entry.GuiTog = !Entry.GuiTog;
            }
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(15f, 25f, 360f, 90f), $"<size=15><color=magenta>Catalyss is Loaded!</color></size>");
            if (Entry.GuiTog)
            {
                Entry.GuiRect = GUI.Window(0, Entry.GuiRect, ModGUI, $"<color=magenta>Catalyss</color> <color=white>v{Entry.ModVersion}</color>");
            }
        }

        public static bool LoopGM = false;
        public static bool LoopStam = false;

        private bool IsSpeedBoostActive { get; set; } = false;
        private string SpeedBoostText = "<color=red>OFF</color>";

        private bool IsBigJumpActive { get; set; } = false;
        private string BigJumpText = "<color=red>OFF</color>";

        private bool IsInfJumpActive { get; set; } = false;
        private string InfJumpText = "<color=red>OFF</color>";

        private bool IsHoverActive { get; set; } = false;
        private string HoverText = "<color=red>OFF</color>";

        private bool IsNoCDWActive { get; set; } = false;
        private string NoCooldownsText = "<color=red>OFF</color>";

        private bool IsHugeDamageActive { get; set; } = false;
        private string HugeDmgText = "<color=red>OFF</color>";

        private bool IsAOEActive { get; set; } = false;
        private string AOEText = "<color=red>OFF</color>";

        private bool IsGodModeActive { get; set; } = false;
        private string GodModeText = "<color=red>OFF</color>";

        private bool IsInfStamActive { get; set; } = false;
        private string InfStamText = "<color=red>OFF</color>";

        private bool IsAutoPActive { get; set; } = false;
        private string AutoPText = "<color=red>OFF</color>";

        private int ExpPointAmount { get; set; } = 20;
        public string UserItemInput { get; set; } = "";

        void ModGUI(int WindowId)
        {
            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.magenta;
            GUI.color = Color.magenta;

            //40 distance
            if (GUI.Button(new Rect(20, 30, 300, 30), $"Speed Boost [{SpeedBoostText}]"))
            {
                IsSpeedBoostActive = !IsSpeedBoostActive;
                if (IsSpeedBoostActive)
                {
                    SpeedBoostText = "<color=green>ON</color>";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponent<PlayerMove>()._movSpeed = 150;
                    }
                }
                else
                {
                    SpeedBoostText = "<color=red>OFF</color>";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponent<PlayerMove>().Reset_MoveSpeed();
                    }
                }
            }

            if (GUI.Button(new Rect(20, 70, 300, 30), $"Big Jump [{BigJumpText}]"))
            {
                IsBigJumpActive = !IsBigJumpActive;
                if (IsBigJumpActive)
                {
                    BigJumpText = "<color=green>ON</color>";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerMove>()._standardJumpForce = 90;
                    }
                }
                else
                {
                    BigJumpText = "<color=red>OFF</color>";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerMove>()._standardJumpForce = 33;
                    }
                }
            }

            if (GUI.Button(new Rect(20, 110, 300, 30), $"Inf Jump [{InfJumpText}]"))
            {
                IsInfJumpActive = !IsInfJumpActive;
                if (IsInfJumpActive)
                {
                    InfJumpText = "<color=green>ON</color>";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerMove>()._maxJumps = 9999;
                    }
                }
                else
                {
                    InfJumpText = "<color=red>OFF</color>";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerMove>()._maxJumps = 2;
                    }
                }
            }

            if (GUI.Button(new Rect(20, 150, 300, 30), $"Hover [{HoverText}]"))
            {
                IsHoverActive = !IsHoverActive;
                if (IsHoverActive)
                {
                    HoverText = "<color=green>ON</color>";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().transform.position = new Vector3(GetPlayer().transform.position.x, 25,
                            GetPlayer().transform.position.z);
                        GetPlayer().GetComponentInChildren<CharacterController>().height = 20;
                    }
                }
                else
                {
                    HoverText = "<color=red>OFF</color>";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().transform.position = new Vector3(GetPlayer().transform.position.x, 2,
                            GetPlayer().transform.position.z);
                        GetPlayer().GetComponentInChildren<CharacterController>().height = 2;
                    }
                }
            }

            if (GUI.Button(new Rect(20, 190, 300, 30), $"No Cooldowns [{NoCooldownsText}]"))
            {
                IsNoCDWActive = !IsNoCDWActive;
                if (IsNoCDWActive)
                {
                    NoCooldownsText = "<color=green>ON</color>";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackMoveForce = 155f;
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackActionBuffer = 0f;
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackSwingCooldown = 0.2f;
                    }
                }
                else
                {
                    NoCooldownsText = "<color=red>OFF</color>";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackMoveForce = 30f;
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackActionBuffer = 0.4f;
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackSwingCooldown = 0.7f;
                    }
                }
            }

            if (GUI.Button(new Rect(20, 230, 300, 30), $"Huge Damage [{HugeDmgText}]"))
            {
                IsHugeDamageActive = !IsHugeDamageActive;
                if (IsHugeDamageActive)
                {
                    HugeDmgText = "<color=green>ON</color>";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._damagePercent = 9999f;
                    }
                }
                else
                {
                    HugeDmgText = "<color=red>OFF</color>";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._damagePercent = 1f;
                    }
                }
            }

            if (GUI.Button(new Rect(20, 270, 300, 30), $"AOE Weapons [{AOEText}]"))
            {
                IsAOEActive = !IsAOEActive;
                if (IsAOEActive)
                {
                    AOEText = "<color=green>ON</color>";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._hitboxScale = new Vector3(9999f, 9999f, 9999f);
                    }
                }
                else
                {
                    AOEText = "<color=red>OFF</color>";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._hitboxScale = new Vector3(1.5f, 1.5f, 1.5f);
                        GetPlayer().GetComponentInChildren<PlayerCombat>().Cmd_ResetHitboxes();
                    }
                }
            }

            //Next side
            if (GUI.Button(new Rect(340, 30, 300, 30), "10+ Level"))
            {
                if (GetPlayer() != null)
                {
                    GetPlayer().GetComponentInChildren<PlayerStats>()._currentLevel = 10;
                }
            }

            if (GUI.Button(new Rect(340, 70, 300, 30), "20+ Level"))
            {
                if (GetPlayer() != null)
                {
                    GetPlayer().GetComponentInChildren<PlayerStats>()._currentLevel = 20;
                }
            }

            if (GUI.Button(new Rect(340, 110, 300, 30), "Max Level"))
            {
                if (GetPlayer() != null)
                {
                    GetPlayer().GetComponentInChildren<PlayerStats>()._currentLevel = 25;
                }
            }

            if (GUI.Button(new Rect(340, 150, 300, 30), "Give EXP"))
            {
                if (GetPlayer() != null)
                {
                    GetPlayer().GetComponentInChildren<PlayerStats>()._currentExp = ExpPointAmount++;
                }
            }

            if (GUI.Button(new Rect(340, 190, 300, 30), "Set Points"))
            {
                if (GetPlayer() != null)
                {
                    GetPlayer().GetComponentInChildren<PlayerStats>()._currentAttributePoints = ExpPointAmount;
                }
            }

            if (GUI.Button(new Rect(340, 230, 300, 30), "Set Tier"))
            {
                if (GetPlayer() != null)
                {
                    GetPlayer().GetComponentInChildren<PlayerStats>()._classTier = ExpPointAmount;
                }
            }

            if (GUI.Button(new Rect(340, 270, 300, 30), $"Add Money"))
            {
                if (GetPlayer() != null)
                {
                    GetPlayer().GetComponentInChildren<PlayerInventory>().Add_Currency(50);
                }
            }

            if (GUI.Button(new Rect(340, 310, 300, 30), $"Goon"))
            {
                if (GetPlayer() != null)
                {
                    GetPlayer().GetComponentInChildren<PlayerRaceModel>()._displayBoobs = true;
                    GetPlayer().GetComponentInChildren<PlayerRaceModel>()._boobWeight = 400;
                    GetPlayer().GetComponentInChildren<PlayerRaceModel>()._bottomWeight = 400;
                }
            }

            if (GUI.Button(new Rect(340, 350, 300, 30), $"Fat"))
            {
                if (GetPlayer() != null)
                {
                    GetPlayer().GetComponentInChildren<PlayerRaceModel>()._widthWeight = 400;
                    GetPlayer().GetComponentInChildren<PlayerRaceModel>()._bellyWeight = 400;
                }
            }

            if (GUI.Button(new Rect(340, 390, 300, 30), $"Hide Steam ID"))
            {
                if (GetPlayer() != null)
                {
                    GetPlayer().GetComponentInChildren<Player>()._steamID = "CATALYSS EVILMANE INC";
                }
            }

            if (GUI.Button(new Rect(340, 430, 300, 30), $"Drop Money"))
            {
                if (GetPlayer() != null)
                {
                    GetPlayer().GetComponentInChildren<PlayerInventory>().Cmd_AddCurrency(500);
                    GetPlayer().GetComponentInChildren<PlayerInventory>().Cmd_DropCurrency(500);
                }
            }

            //Other side
            if (GUI.Button(new Rect(20, 310, 300, 30), $"GOD MODE [{GodModeText}]"))
            {
                if (GetPlayer() != null)
                {
                    IsGodModeActive = !IsGodModeActive;
                    if (IsGodModeActive)
                    {
                        GodModeText = "<color=green>ON</color>";
                        GetPlayer().GetComponentInChildren<StatusEntity>()._currentHealth = 999;
                        GetPlayer().GetComponentInChildren<StatusEntity>().Network_currentHealth = 999;
                        GetPlayer().GetComponentInChildren<StatusEntity>().Add_Health(999);
                        LoopGM = true;
                    }
                    else
                    {
                        //Ngl too lazy to make a check/cache for ur original health uhh deal with it 4 now I guess lol
                        GodModeText = "<color=red>OFF</color>";
                        GetPlayer().GetComponentInChildren<StatusEntity>()._currentHealth = 50;
                        GetPlayer().GetComponentInChildren<StatusEntity>().Network_currentHealth = 50;
                        GetPlayer().GetComponentInChildren<StatusEntity>().Add_Health(1);
                        LoopGM = false;
                    }
                }
            }

            if (GUI.Button(new Rect(20, 350, 300, 30), $"INF Stamina [{InfStamText}]"))
            {
                if (GetPlayer() != null)
                {
                    IsInfStamActive = !IsInfStamActive;
                    if (IsInfStamActive)
                    {
                        InfStamText = "<color=green>ON</color>";
                        GetPlayer().GetComponentInChildren<StatusEntity>().Change_Stamina(9999);
                        GetPlayer().GetComponentInChildren<StatusEntity>().Network_currentStamina = 9999;
                        GetPlayer().GetComponentInChildren<StatusEntity>()._currentStamina = 9999;
                        LoopStam = true;
                    }
                    else
                    {
                        //Same as health LOOOOL
                        InfStamText = "<color=red>OFF</color>";
                        GetPlayer().GetComponentInChildren<StatusEntity>().Change_Stamina(100);
                        GetPlayer().GetComponentInChildren<StatusEntity>().Network_currentStamina = 100;
                        GetPlayer().GetComponentInChildren<StatusEntity>()._currentStamina = 100;
                        LoopStam = false;
                    }
                }
            }

            if (GUI.Button(new Rect(20, 390, 300, 30), $"Auto Parry [{AutoPText}]"))
            {
                if (GetPlayer() != null)
                {
                    IsAutoPActive = !IsAutoPActive;
                    if (IsAutoPActive)
                    {
                        AutoPText = "<color=green>ON</color>";
                        GetPlayer().GetComponentInChildren<StatusEntity>()._autoParry = true;
                    }
                    else
                    {
                        //Same as health LOOOOL
                        AutoPText = "<color=red>OFF</color>";
                        GetPlayer().GetComponentInChildren<StatusEntity>()._autoParry = false;
                    }
                }
            }

            if (GUI.Button(new Rect(20, 430, 300, 30), $"Force Revive"))
            {
                if (GetPlayer() != null)
                {
                    GetPlayer().GetComponentInChildren<StatusEntity>().Cmd_RevivePlayer(Player._mainPlayer);
                    GetPlayer().GetComponentInChildren<StatusEntity>().Cmd_ReplenishAll();
                }
            }

            if (GUI.Button(new Rect(300, 470, 300, 30), $"Next Page -->"))
            {
                //For next update
            }

            /*UserItemInput = GUI.TextField(new Rect(20, 350, 200, 40), "ItemName");

            if (GUI.Button(new Rect(20, 360, 300, 30), "Give Item"))
            {
                if (GetPlayer() != null)
                {
                    var aItem = new ItemData()
                    {
                        _itemName = UserItemInput,
                        _quantity = 69, //niceeee
                        _isEquipped = false,
                        _slotNumber = 1,
                    };
                    GetPlayer().GetComponentInChildren<PlayerInventory>().Add_Item(aItem);
                }
            }*/

            GUI.DragWindow(new Rect(0, 0, 10000, 200));
        }
    }
}