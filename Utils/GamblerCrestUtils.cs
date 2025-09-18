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
        public static ToolCrestsData.Data crestSave
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

        public static bool _inFeverState = false;
        public static bool inFeverState
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

        public static void HealFeverState()
        {
            PlayerData.instance.health = PlayerData.instance.maxHealth;
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
    }
}
