using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace GamblerCrest.Utils
{
    public class GamblerCrestUtils
    {
        public static ToolCrest gamblerCrestData = new();
        public static bool inFeverState = false;
        public static ToolCrestsData.Data defaultSave = new()
        {
            IsUnlocked = true,
            Slots = [],
            DisplayNewIndicator = true,
        };
        public static ToolCrestsData.Data crestSave
        {
            get
            {
                if (GameManager.instance == null) return defaultSave;

                string saveFolderPath = Path.Combine(Application.persistentDataPath, $"user{GameManager.instance.profileID}", $"GamblerCrestData.dat");
                if (File.Exists(saveFolderPath))
                {
                    string json = File.ReadAllText(saveFolderPath);
                    ToolCrestsData.Data data = JsonConvert.DeserializeObject<ToolCrestsData.Data>(json);
                    return data;
                } else
                {
                    return defaultSave;
                }
            }
        }

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
                string gamblerCrestSaveFile = Path.Combine(Application.persistentDataPath, $"user{GameManager.instance.profileID}", $"GamblerCrestData.dat");
                Directory.CreateDirectory(Path.GetDirectoryName(gamblerCrestSaveFile));
                File.WriteAllText(gamblerCrestSaveFile, saveDataJson);
            }
        }
    }
}
