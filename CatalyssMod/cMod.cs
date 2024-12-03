using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

namespace CatalyssMod
{
    internal class cMod : MonoBehaviour
    {
        public GameObject GetPlayer()
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

        //private readonly float FlightSpeed = 5f;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Entry.GuiTog = !Entry.GuiTog;
            }

            if (IsFlightActive)
            {
                //Will fix later
                /*Vector3 movementDirection = Vector3.zero;

                if (GetPlayer() != null)
                {
                    //Up down
                    movementDirection -= GetPlayer().transform.up * (Input.GetKey(KeyCode.Q) ? 1 : 0);
                    movementDirection += GetPlayer().transform.up * (Input.GetKey(KeyCode.E) ? 1 : 0);

                    //Movement
                    movementDirection += GetPlayer().transform.forward * (Input.GetKey(KeyCode.W) ? 1 : 0);
                    movementDirection -= GetPlayer().transform.right * (Input.GetKey(KeyCode.A) ? 1 : 0);
                    movementDirection -= GetPlayer().transform.forward * (Input.GetKey(KeyCode.S) ? 1 : 0);
                    movementDirection += GetPlayer().transform.right * (Input.GetKey(KeyCode.D) ? 1 : 0);
                }

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    GetPlayer().transform.position += movementDirection * FlightSpeed * 2.5f * Time.deltaTime;
                }
                else
                {
                    GetPlayer().transform.position += movementDirection * FlightSpeed * Time.deltaTime;
                }*/
            }
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(15f, 25f, 160f, 90f), $"<color=magenta>Catalyss is Loaded!</color>");
            GUI.Label(new Rect(15f, 40f, 160f, 90f), $"<color=magenta>{DateTime.Now:hh:mm:ss}</color>");

            if (Entry.GuiTog)
            {
                var GuiName = $"<color=magenta>Catalyss</color> <color=white>v{Entry.ModVersion}</color>";
                Entry.GuiRect = GUI.Window(0, Entry.GuiRect, ModGUI, GuiName);
            }
        }

        private bool IsSpeedBoostActive = false;
        private string SpeedBoostText = "OFF";

        private bool IsBigJumpActive = false;
        private string BigJumpText = "OFF";

        private bool IsInfJumpActive = false;
        private string InfJumpText = "OFF";

        private bool IsHoverActive = false;
        private string HoverText = "OFF";

        private bool IsNoCDWActive = false;
        private string NoCooldownsText = "OFF";

        private bool IsHugeDamageActive = false;
        private string HugeDmgText = "OFF";

        private bool IsAOEActive = false;
        private string AOEText = "OFF";

        private bool IsFlightActive = false;
        private string FlightText = "OFF";

        private int ExpPointAmount { get; set; } = 20;

        void ModGUI(int WindowId)
        {
            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.magenta;
            GUI.color = Color.magenta;

            //40
            if (GUI.Button(new Rect(20, 30, 300, 30), $"Speed Boost [{SpeedBoostText}]"))
            {
                IsSpeedBoostActive = !IsSpeedBoostActive;
                if (IsSpeedBoostActive)
                {
                    SpeedBoostText = "ON";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponent<PlayerMove>()._movSpeed = 150;
                    }
                }
                else
                {
                    SpeedBoostText = "OFF";

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
                    BigJumpText = "ON";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerMove>()._standardJumpForce = 90;
                    }
                }
                else
                {
                    BigJumpText = "OFF";

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
                    InfJumpText = "ON";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerMove>()._maxJumps = 9999;
                    }
                }
                else
                {
                    InfJumpText = "OFF";

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
                    HoverText = "ON";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().transform.position = new Vector3(GetPlayer().transform.position.x, 25,
                            GetPlayer().transform.position.z);
                        GetPlayer().GetComponentInChildren<CharacterController>().height = 20;
                    }
                }
                else
                {
                    HoverText = "OFF";

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
                    NoCooldownsText = "ON";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackMoveForce = 155f;
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackActionBuffer = 0f;
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._attackSwingCooldown = 0.2f;
                    }
                }
                else
                {
                    NoCooldownsText = "OFF";

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
                    HugeDmgText = "ON";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._damagePercent = 9999f;
                    }
                }
                else
                {
                    HugeDmgText = "OFF";

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
                    AOEText = "ON";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._hitboxScale = new Vector3(9999f, 9999f, 9999f);
                    }
                }
                else
                {
                    AOEText = "OFF";

                    if (GetPlayer() != null)
                    {
                        GetPlayer().GetComponentInChildren<PlayerCombat>()._currentScriptableWeaponType._hitboxScale = new Vector3(1.5f, 1.5f, 1.5f);
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

            if (GUI.Button(new Rect(340, 270, 300, 30), $"Player Flight (Bugged) [{FlightText}]"))
            {
                if (GetPlayer() != null)
                {
                    IsFlightActive = !IsFlightActive;
                    if (IsFlightActive)
                    {
                        FlightText = "ON";
                    }
                    else
                    {
                        FlightText = "OFF";
                    }
                }
            }

            if (GUI.Button(new Rect(340, 310, 300, 30), "Quit Game"))
            {
                Application.Quit();
            }

            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        private void OnDisable()
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
