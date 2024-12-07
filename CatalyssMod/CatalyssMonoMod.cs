using UnityEngine;

namespace CatalyssMod
{
    internal class CatalyssMonoMod : MonoBehaviour
    {
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
            if (HandleItemDropMenu) { return; }
            if (Entry.GuiTog)
            {
                Entry.GuiRect = GUI.Window(0, Entry.GuiRect, ModGUI, $"<color=magenta>Catalyss</color> <color=white>v{Entry.ModVersion}</color>");
            }
        }

        public static bool LoopGM { get; set; } = false;
        public static bool LoopStam { get; set; } = false;
        public static bool AutoRez { get; set; } = false;
        private string AutoRezText = "<color=red>OFF</color>";

        public static bool SpinBotPlayer { get; set; } = false;
        private string SpinBotText = "<color=red>OFF</color>";

        public static bool IframePlyr { get; set; } = false;
        private string IFramePText = "<color=red>OFF</color>";


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

        private int ExpPointAmount { get; set; } = 10;
        
        public static bool InfManaTog { get; set; } = false;
        public static bool HandleItemDropMenu { get; set; } = false;
        public string InfManaText = "<color=red>OFF</color>";

        public static int MenuPage = 1;

        void ModGUI(int WindowId)
        {
            switch (MenuPage)
            {
                case 1: //Page 1

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

                            if (Utils.GetPlayer() != null)
                            {
                                Utils.GetPlayer().GetComponent<PlayerMove>()._movSpeed = 150;
                            }
                        }
                        else
                        {
                            SpeedBoostText = "<color=red>OFF</color>";

                            if (Utils.GetPlayer() != null)
                            {
                                Utils.GetPlayer().GetComponent<PlayerMove>().Reset_MoveSpeed();
                            }
                        }
                    }

                    if (GUI.Button(new Rect(20, 70, 300, 30), $"Big Jump [{BigJumpText}]"))
                    {
                        IsBigJumpActive = !IsBigJumpActive;
                        if (IsBigJumpActive)
                        {
                            BigJumpText = "<color=green>ON</color>";

                            if (Utils.GetPlayer() != null)
                            {
                                Utils.GetPlayer().GetComponentInChildren<PlayerMove>()._standardJumpForce = 90;
                            }
                        }
                        else
                        {
                            BigJumpText = "<color=red>OFF</color>";

                            if (Utils.GetPlayer() != null)
                            {
                                Utils.GetPlayer().GetComponentInChildren<PlayerMove>()._standardJumpForce = 33;
                            }
                        }
                    }

                    if (GUI.Button(new Rect(20, 110, 300, 30), $"Inf Jump [{InfJumpText}]"))
                    {
                        IsInfJumpActive = !IsInfJumpActive;
                        if (IsInfJumpActive)
                        {
                            InfJumpText = "<color=green>ON</color>";

                            if (Utils.GetPlayer() != null)
                            {
                                Utils.GetPlayer().GetComponentInChildren<PlayerMove>()._maxJumps = 9999;
                            }
                        }
                        else
                        {
                            InfJumpText = "<color=red>OFF</color>";

                            if (Utils.GetPlayer() != null)
                            {
                                Utils.GetPlayer().GetComponentInChildren<PlayerMove>()._maxJumps = 2;
                            }
                        }
                    }

                    if (GUI.Button(new Rect(20, 150, 300, 30), $"Hover [{HoverText}]"))
                    {
                        IsHoverActive = !IsHoverActive;
                        if (IsHoverActive)
                        {
                            HoverText = "<color=green>ON</color>";

                            if (Utils.GetPlayer() != null)
                            {
                                Utils.GetPlayer().transform.position = new Vector3(Utils.GetPlayer().transform.position.x, 25,
                                    Utils.GetPlayer().transform.position.z);
                                Utils.GetPlayer().GetComponentInChildren<CharacterController>().height = 20;
                            }
                        }
                        else
                        {
                            HoverText = "<color=red>OFF</color>";

                            if (Utils.GetPlayer() != null)
                            {
                                Utils.GetPlayer().transform.position = new Vector3(Utils.GetPlayer().transform.position.x, 2,
                                    Utils.GetPlayer().transform.position.z);
                                Utils.GetPlayer().GetComponentInChildren<CharacterController>().height = 2;
                            }
                        }
                    }

                    if (GUI.Button(new Rect(20, 190, 300, 30), $"No Cooldowns [{NoCooldownsText}]"))
                    {
                        IsNoCDWActive = !IsNoCDWActive;
                        if (IsNoCDWActive)
                        {
                            NoCooldownsText = "<color=green>ON</color>";

                            if (Utils.GetPlayer() != null)
                            {
                                Utils.GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackMoveForce = 155f;
                                Utils.GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackActionBuffer = 0f;
                                Utils.GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackSwingCooldown = 0.2f;
                            }
                        }
                        else
                        {
                            NoCooldownsText = "<color=red>OFF</color>";

                            if (Utils.GetPlayer() != null)
                            {
                                Utils.GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackMoveForce = 30f;
                                Utils.GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackActionBuffer = 0.4f;
                                Utils.GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackSwingCooldown = 0.7f;
                            }
                        }
                    }

                    if (GUI.Button(new Rect(20, 230, 300, 30), $"Huge Damage [{HugeDmgText}]"))
                    {
                        IsHugeDamageActive = !IsHugeDamageActive;
                        if (IsHugeDamageActive)
                        {
                            HugeDmgText = "<color=green>ON</color>";

                            if (Utils.GetPlayer() != null)
                            {
                                Utils.GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._damagePercent = 9999f;
                            }
                        }
                        else
                        {
                            HugeDmgText = "<color=red>OFF</color>";

                            if (Utils.GetPlayer() != null)
                            {
                                Utils.GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._damagePercent = 1f;
                            }
                        }
                    }

                    if (GUI.Button(new Rect(20, 270, 300, 30), $"AOE Weapons [{AOEText}]"))
                    {
                        IsAOEActive = !IsAOEActive;
                        if (IsAOEActive)
                        {
                            AOEText = "<color=green>ON</color>";

                            if (Utils.GetPlayer() != null)
                            {
                                Utils.GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._hitboxScale = new Vector3(9999f, 9999f, 9999f);
                            }
                        }
                        else
                        {
                            AOEText = "<color=red>OFF</color>";

                            if (Utils.GetPlayer() != null)
                            {
                                Utils.GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._hitboxScale = new Vector3(1.5f, 1.5f, 1.5f);
                                Utils.GetPlayer().GetComponentInChildren<PlayerCombat>().Cmd_ResetHitboxes();
                            }
                        }
                    }

                    //Next side
                    if (GUI.Button(new Rect(340, 30, 300, 30), "10+ Level"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            Utils.GetPlayer().GetComponentInChildren<PlayerStats>()._currentLevel = 10;
                        }
                    }

                    if (GUI.Button(new Rect(340, 70, 300, 30), "20+ Level"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            Utils.GetPlayer().GetComponentInChildren<PlayerStats>()._currentLevel = 20;
                        }
                    }

                    if (GUI.Button(new Rect(340, 110, 300, 30), "Max Level"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            Utils.GetPlayer().GetComponentInChildren<PlayerStats>()._currentLevel = 25;
                        }
                    }

                    if (GUI.Button(new Rect(340, 150, 300, 30), "Give EXP"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            Utils.GetPlayer().GetComponentInChildren<PlayerStats>()._currentExp = ExpPointAmount++;
                        }
                    }

                    if (GUI.Button(new Rect(340, 190, 300, 30), "Set Points"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            Utils.GetPlayer().GetComponentInChildren<PlayerStats>()._currentAttributePoints = ExpPointAmount;
                        }
                    }

                    if (GUI.Button(new Rect(340, 230, 300, 30), "Set Tier"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            Utils.GetPlayer().GetComponentInChildren<PlayerStats>()._classTier = ExpPointAmount;
                        }
                    }

                    if (GUI.Button(new Rect(340, 270, 300, 30), $"Add Money"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            Utils.GetPlayer().GetComponentInChildren<PlayerInventory>().Add_Currency(50);
                        }
                    }

                    if (GUI.Button(new Rect(340, 310, 300, 30), $"Goon"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            Utils.GetPlayer().GetComponentInChildren<PlayerRaceModel>()._displayBoobs = true;
                            Utils.GetPlayer().GetComponentInChildren<PlayerRaceModel>()._boobWeight = 400;
                            Utils.GetPlayer().GetComponentInChildren<PlayerRaceModel>()._bottomWeight = 400;
                        }
                    }

                    if (GUI.Button(new Rect(340, 350, 300, 30), $"Fat"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            Utils.GetPlayer().GetComponentInChildren<PlayerRaceModel>()._widthWeight = 400;
                            Utils.GetPlayer().GetComponentInChildren<PlayerRaceModel>()._bellyWeight = 400;
                        }
                    }

                    if (GUI.Button(new Rect(340, 390, 300, 30), $"Hide Steam ID"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            Utils.GetPlayer().GetComponentInChildren<Player>()._steamID = "CATALYSS EVILMANE INC";
                        }
                    }

                    if (GUI.Button(new Rect(340, 430, 300, 30), $"Drop Money"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            Utils.GetPlayer().GetComponentInChildren<PlayerInventory>().Cmd_AddCurrency(500);
                            Utils.GetPlayer().GetComponentInChildren<PlayerInventory>().Cmd_DropCurrency(500);
                        }
                    }

                    //Other side
                    if (GUI.Button(new Rect(20, 310, 300, 30), $"GOD MODE [{GodModeText}]"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            IsGodModeActive = !IsGodModeActive;
                            if (IsGodModeActive)
                            {
                                GodModeText = "<color=green>ON</color>";
                                Utils.GetPlayer().GetComponentInChildren<StatusEntity>()._currentHealth = 999;
                                Utils.GetPlayer().GetComponentInChildren<StatusEntity>().Network_currentHealth = 999;
                                Utils.GetPlayer().GetComponentInChildren<StatusEntity>().Add_Health(999);
                                LoopGM = true;
                            }
                            else
                            {
                                //Ngl too lazy to make a check/cache for ur original health uhh deal with it 4 now I guess lol
                                GodModeText = "<color=red>OFF</color>";
                                Utils.GetPlayer().GetComponentInChildren<StatusEntity>()._currentHealth = 50;
                                Utils.GetPlayer().GetComponentInChildren<StatusEntity>().Network_currentHealth = 50;
                                Utils.GetPlayer().GetComponentInChildren<StatusEntity>().Add_Health(1);
                                LoopGM = false;
                            }
                        }
                    }

                    if (GUI.Button(new Rect(20, 350, 300, 30), $"INF Stamina [{InfStamText}]"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            IsInfStamActive = !IsInfStamActive;
                            if (IsInfStamActive)
                            {
                                InfStamText = "<color=green>ON</color>";
                                Utils.GetPlayer().GetComponentInChildren<StatusEntity>().Change_Stamina(9999);
                                Utils.GetPlayer().GetComponentInChildren<StatusEntity>().Network_currentStamina = 9999;
                                Utils.GetPlayer().GetComponentInChildren<StatusEntity>()._currentStamina = 9999;
                                LoopStam = true;
                            }
                            else
                            {
                                //Same as health LOOOOL
                                InfStamText = "<color=red>OFF</color>";
                                Utils.GetPlayer().GetComponentInChildren<StatusEntity>().Change_Stamina(100);
                                Utils.GetPlayer().GetComponentInChildren<StatusEntity>().Network_currentStamina = 100;
                                Utils.GetPlayer().GetComponentInChildren<StatusEntity>()._currentStamina = 100;
                                LoopStam = false;
                            }
                        }
                    }

                    if (GUI.Button(new Rect(20, 390, 300, 30), $"Auto Parry [{AutoPText}]"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            IsAutoPActive = !IsAutoPActive;
                            if (IsAutoPActive)
                            {
                                AutoPText = "<color=green>ON</color>";
                                Utils.GetPlayer().GetComponentInChildren<StatusEntity>()._autoParry = true;
                            }
                            else
                            {
                                //Same as health LOOOOL
                                AutoPText = "<color=red>OFF</color>";
                                Utils.GetPlayer().GetComponentInChildren<StatusEntity>()._autoParry = false;
                            }
                        }
                    }

                    if (GUI.Button(new Rect(20, 430, 300, 30), $"Force Revive"))
                    {
                        if (Utils.GetPlayer() != null)
                        {
                            Utils.GetPlayer().GetComponentInChildren<StatusEntity>().Cmd_RevivePlayer(Player._mainPlayer);
                            Utils.GetPlayer().GetComponentInChildren<StatusEntity>().Cmd_ReplenishAll();
                        }
                    }

                    if (GUI.Button(new Rect(300, 470, 300, 30), $"Next Page -->"))
                    {
                        MenuPage = 2;
                    }

                    break;
                case 2: //Page 2

                    GUI.backgroundColor = Color.black;
                    GUI.contentColor = Color.magenta;
                    GUI.color = Color.magenta;

                    //40 distance

                    //Go back button
                    if (GUI.Button(new Rect(100, 470, 300, 30), $"<-- Previous Page"))
                    {
                        MenuPage = 1;
                    }

                    //40 distance
                    if (GUI.Button(new Rect(20, 30, 300, 30), $"Auto Revive [{AutoRezText}]"))
                    {
                        AutoRez = !AutoRez;
                        if(AutoRez)
                        {
                            AutoRezText = "<color=green>ON</color>";
                        }
                        else
                        {
                            AutoRezText = "<color=red>OFF</color>";
                        }
                    }

                    if (GUI.Button(new Rect(20, 70, 300, 30), $"Spinbot [{SpinBotText}]"))
                    {
                        SpinBotPlayer = !SpinBotPlayer;
                        if (SpinBotPlayer)
                        {
                            SpinBotText = "<color=green>ON</color>";
                        }
                        else
                        {
                            SpinBotText = "<color=red>OFF</color>";
                        }
                    }

                    if (GUI.Button(new Rect(20, 110, 300, 30), $"Inf Iframe [{IFramePText}]"))
                    {
                        IframePlyr = !IframePlyr;
                        if (IframePlyr)
                        {
                            IFramePText = "<color=green>ON</color>";
                        }
                        else
                        {
                            IFramePText = "<color=red>OFF</color>";
                        }
                    }

                    if (GUI.Button(new Rect(20, 150, 300, 30), $"Inf Mana [{InfManaText}]"))
                    {
                        InfManaTog = !InfManaTog;
                        if (InfManaTog)
                        {
                            InfManaText = "<color=green>ON</color>";
                        }
                        else
                        {
                            InfManaText = "<color=red>OFF</color>";
                        }
                    }

                    if (GUI.Button(new Rect(20, 190, 300, 30), "Item Drop Menu"))
                    {
                        HandleItemDropMenu = !HandleItemDropMenu;
                    }

                    if (GUI.Button(new Rect(20, 230, 300, 30), "Glam"))
                    {
                        Utils.SendFX(3); //Idk lol
                    }

                    break;
            }

            GUI.DragWindow(new Rect(0, 0, 10000, 200));
        }
    }
}