using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using GamblerCrest.Patches;
using GamblerCrest.Patches.Localization;
using UnityEngine.SceneManagement;
using UnityEngine;
using GamblerCrest.Utils;

namespace GamblerCrest
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class GamblerCrest : BaseUnityPlugin
    {
        public static ManualLogSource logger;
        public static Harmony harmony;

        private void Awake()
        {
            // Put your initialization logic here
            logger = Logger;
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} has loaded!");
            harmony = new("com.example.patch");
            harmony.PatchAll(typeof(SetupAndAddCrest));
            harmony.PatchAll(typeof(BlackFlash));
            harmony.PatchAll(typeof(AlterLayering));
            harmony.PatchAll(typeof(NeedleArtWhiffPunish));
            SaveGameSavesModdedCrest.Apply(harmony);

            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void Update()
        {
            if (GamblerCrestUtils.InFeverState)
            {
                GamblerCrestUtils.HealFeverState();

                if (GamblerCrestUtils.feverTimer <= 0)
                {
                    GamblerCrestUtils.InFeverState = false;
                    GamblerCrestUtils.stopAura();
                }
            }

            if (GamblerCrestUtils.feverTimer > 0)
            {
                GamblerCrestUtils.feverTimer -= Time.deltaTime;
            }

            if (GamblerCrestUtils.bfTimer > 0)
            {
                GamblerCrestUtils.bfTimer -= Time.deltaTime * (1 + (GamblerCrestUtils.bfComboMult * GamblerCrestUtils.combo));
            }

            if (GamblerCrestUtils.bfTimer <= 0)
            {
                GamblerCrestUtils.BlackFlashChanceBonus -= 1;
            }
        }

        private void OnSceneChanged(Scene prev, Scene next)
        {
            if (next.name != "Menu_Title")
            {
                if (!Localization.LocalizationPatched)
                {
                    harmony.PatchAll(typeof(Localization));
                }
            }
        }
    }
}