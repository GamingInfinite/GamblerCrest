using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace GamblerCrest.Utils
{
    public class GamblerCrestUtils
    {
        public static ToolCrest gamblerCrestData = new();
        public static ToolCrestsData.Data defaultSave = new()
        {
            IsUnlocked = true,
            Slots = [],
            DisplayNewIndicator = true,
        };
        public static string SavePath
        {
            get
            {
                DesktopPlatform platform = Platform.Current as DesktopPlatform;
                string accId = "default";
                if (platform != null)
                {
                    if (platform.onlineSubsystem != null)
                    {
                        string userId = platform.onlineSubsystem.UserId;
                        if (!string.IsNullOrEmpty(userId))
                        {
                            accId = userId;
                        }
                    }
                }
                string gamblerCrestSaveFile = Path.Combine(Application.persistentDataPath, accId, $"user{GameManager.instance.profileID}", $"GamblerCrestData.dat");
                return gamblerCrestSaveFile;
            }
        }
        public static ToolCrestsData.Data CrestSave
        {
            get
            {
                if (GameManager.instance == null) return defaultSave;

                if (File.Exists(SavePath))
                {
                    string json = File.ReadAllText(SavePath);
                    ToolCrestsData.Data data = JsonConvert.DeserializeObject<ToolCrestsData.Data>(json);
                    return data;
                } else
                {
                    return defaultSave;
                }
            }
        }

        public static PlayParticleEffects poisonAura;
        public static PlayParticleEffects activeAura;

        public static bool _inFeverState = false;
        public static bool InFeverState
        {
            get
            {
                return _inFeverState;
            }
            set
            {
                if (value == true)
                {
                    _inFeverState = true;
                    feverTimer = feverCooldown;
                } else
                {
                    _inFeverState = false;
                }
            }
        }

        public static float feverCooldown = 30f;
        public static float feverTimer = 0f;

        public static float bfCooldown = 10f;
        public static float bfTimer = 0f;

        public static int combo = 0;
        public static float bfComboMult = 0.1f;
        public static float blackFlashChance = 1f;
        public static float _blackFlashBonus = 0f;
        public static float BlackFlashChanceBonus
        {
            get
            {
                return _blackFlashBonus;
            }
            set
            {
                if (value > 0f)
                {
                    _blackFlashBonus = value;
                    bfTimer = bfCooldown;
                    combo += 1;
                } else
                {
                    _blackFlashBonus = 0f;
                    combo = 0;
                }
            }
        }

        public static void HealFeverState()
        {
            HeroController.instance.AddHealth(PlayerData.instance.maxHealth);
        }

        public static void SaveCrestSlots()
        {
            if (GameManager.instance != null)
            {
                ModHelper.Log("Saving Modded Crest Data...");
                ToolCrestsData.Data data = gamblerCrestData.SaveData;
                string saveDataJson = JsonConvert.SerializeObject(data, Formatting.Indented);
                
                Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
                File.WriteAllText(SavePath, saveDataJson);
            }
        }

        public static void activateAura()
        {
            if (activeAura)
            {
                activeAura.Recycle<PlayParticleEffects>();
                activeAura = null;
            }
            if (poisonAura)
            {
                PlayParticleEffects newAura = poisonAura.Spawn<PlayParticleEffects>();
                newAura.transform.SetParent(HeroController.instance.transform, true);
                newAura.transform.SetLocalPosition2D(0, 0);
                activeAura = newAura;
                newAura.PlayParticleSystems();
            }
        }

        public static void stopAura()
        {
            if (activeAura)
            {
                activeAura.StopParticleSystems();
                activeAura = null;
            }
        }
    }
}
