using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using GamblerCrest.Patches;
using GamblerCrest.Patches.Localization;
using UnityEngine.SceneManagement;
using UnityEngine;
using GamblerCrest.Utils;
using Needleforge.Data;
using Needleforge;

namespace GamblerCrest
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class GamblerCrest : BaseUnityPlugin
    {
        public static ManualLogSource logger;
        public static Harmony harmony;
        public static CrestData hakariCrest;

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

            Texture2D diceCrestSilhouette = ModHelper.LoadTexFromAssembly("GamblerCrest.Resources.Images.gamblerCrestSilhouette.png");
            Sprite crestSilhouette = Sprite.Create(diceCrestSilhouette, new(0, 0, diceCrestSilhouette.width, diceCrestSilhouette.height), new(0.5f, 0.5f), 320f);

            Texture2D diceCrest = ModHelper.LoadTexFromAssembly("GamblerCrest.Resources.Images.gamblerCrest.png");
            Sprite crestSprite = Sprite.Create(diceCrest, new(0, 0, diceCrest.width, diceCrest.height), new(0.5f, 0.5f), 166.67f);

            hakariCrest = NeedleforgePlugin.AddCrest("GAMBLER", crestSprite, crestSilhouette);

            hakariCrest.BindEvent += (healValue, healAmount, healTime) =>
            {
                healAmount.Value = 1;
                healTime.Value = 1.2f;

                int randomChance = Random.Range(1, 21);
                randomChance = 20;
                Logger.LogInfo($"{randomChance}");

                int randomHeal = 0;

                if (ToolItemManager.Instance.toolItems.GetByName(""))
                {

                }
                randomHeal = randomChance switch
                {
                    20 => PlayerData.instance.maxHealth,
                    >= 17 => 3,
                    >= 14 => 2,
                    >= 9 => 1,
                    >= 5 => 0,
                    >= 2 => -1,
                    _ => -2
                };

                if (randomHeal == PlayerData.instance.maxHealth)
                {
                    GamblerCrestUtils.InFeverState = true;
                    GamblerCrestUtils.activateAura();
                }
                else if (randomHeal < 0)
                {
                    HeroController.instance.TakeHealth(Mathf.Abs(randomHeal));
                }

                healValue.Value = randomHeal;
            };

            hakariCrest.slots = [
                new() {
                    AttackBinding = AttackToolBinding.Neutral,
                    Type = ToolItemType.Skill,
                    Position = new(.805f, .2f),
                    IsLocked = false,
                    NavUpIndex = 1,
                    NavUpFallbackIndex = -1,
                    NavRightIndex = 4,
                    NavRightFallbackIndex = -1,
                    NavLeftIndex = 3,
                    NavLeftFallbackIndex = -1,
                    NavDownIndex = 2,
                    NavDownFallbackIndex = -1,
                },
                new() {
                    AttackBinding = AttackToolBinding.Up,
                    Type = ToolItemType.Red,
                    Position = new(-.2f, 2.05f),
                    IsLocked = false,
                    NavUpIndex = -1,
                    NavUpFallbackIndex = -1,
                    NavRightIndex = -1,
                    NavRightFallbackIndex = -1,
                    NavLeftIndex = -1,
                    NavLeftFallbackIndex = -1,
                    NavDownIndex = 0,
                    NavDownFallbackIndex = -1,
                },
                new() {
                    AttackBinding = AttackToolBinding.Down,
                    Type = ToolItemType.Skill,
                    Position = new(-.765f, -1.15f),
                    IsLocked = false,
                    NavUpIndex = 0,
                    NavUpFallbackIndex = -1,
                    NavRightIndex = -1,
                    NavRightFallbackIndex = -1,
                    NavLeftIndex = -1,
                    NavLeftFallbackIndex = -1,
                    NavDownIndex = -1,
                    NavDownFallbackIndex = -1,
                },
                new() {
                    AttackBinding = AttackToolBinding.Neutral,
                    Type = ToolItemType.Yellow,
                    Position = new(-2.6f, 3.00f),
                    IsLocked = false,
                    NavUpIndex = -1,
                    NavUpFallbackIndex = -1,
                    NavRightIndex = 0,
                    NavRightFallbackIndex = -1,
                    NavLeftIndex = -1,
                    NavLeftFallbackIndex = -1,
                    NavDownIndex = -1,
                    NavDownFallbackIndex = -1,
                },
                new() {
                    AttackBinding = AttackToolBinding.Neutral,
                    Type = ToolItemType.Yellow,
                    Position = new(1.255f, -3.125f),
                    IsLocked = false,
                    NavUpIndex = -1,
                    NavUpFallbackIndex = -1,
                    NavRightIndex = -1,
                    NavRightFallbackIndex = -1,
                    NavLeftIndex = 0,
                    NavLeftFallbackIndex = -1,
                    NavDownIndex = -1,
                    NavDownFallbackIndex = -1,
                },
            ];

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
                harmony.PatchAll(typeof(Localization));
            }
        }
    }
}