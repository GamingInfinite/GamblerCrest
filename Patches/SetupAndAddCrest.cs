using System.Collections.Generic;
using HarmonyLib;
using TeamCherry.Localization;
using UnityEngine;
using Silksong.FsmUtil;
using HutongGames.PlayMaker;
using GenericVariableExtension;
using HutongGames.PlayMaker.Actions;
using GamblerCrest.Utils;

namespace GamblerCrest.Patches
{
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.Start))]
    internal class SetupAndAddCrest
    {
        [HarmonyPostfix]
        public static void Postfix(HeroController __instance)
        {
            List<ToolCrest> toolCrests = ToolItemManager.GetAllCrests();
            ToolCrest hunter = toolCrests[0];
            foreach (ToolCrest tool in toolCrests)
            {
                if (tool.name.Contains("Hunter") && tool.IsUnlocked && tool.IsVisible)
                {
                    hunter = tool;
                }
            }
            ToolCrest reaper = toolCrests[3];

            Sprite crestGlow = reaper.CrestGlow;

            Texture2D diceCrestSilhouette = ModHelper.LoadTexFromAssembly("GamblerCrest.Resources.Images.gamblerCrestSilhouette.png");
            Sprite crestSilhouette = Sprite.Create(diceCrestSilhouette, new(0, 0, diceCrestSilhouette.width, diceCrestSilhouette.height), new(0.5f, 0.5f), 320f);

            Texture2D diceCrest = ModHelper.LoadTexFromAssembly("GamblerCrest.Resources.Images.gamblerCrest.png");
            Sprite crestSprite = Sprite.Create(diceCrest, new(0, 0, diceCrest.width, diceCrest.height), new(0.5f, 0.5f), 166.67f);
            HeroControllerConfig heroConfig = hunter.HeroConfig;

            GamblerCrestUtils.gamblerCrestData.name = "GAMBLER";
            GamblerCrestUtils.gamblerCrestData.crestGlow = crestGlow;
            GamblerCrestUtils.gamblerCrestData.crestSilhouette = crestSilhouette;
            GamblerCrestUtils.gamblerCrestData.crestSprite = crestSprite;
            GamblerCrestUtils.gamblerCrestData.heroConfig = heroConfig;

            LocalisedString dispName = new LocalisedString() { Key = "GAMBLERCRESTNAME", Sheet = "GAMBLER" };
            LocalisedString description = new LocalisedString() { Key = "GAMBLERCRESTDESC", Sheet = "GAMBLER" };

            GamblerCrestUtils.gamblerCrestData.displayName = dispName;
            GamblerCrestUtils.gamblerCrestData.description = description;

            GamblerCrestUtils.gamblerCrestData.slots = [
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
            GamblerCrestUtils.gamblerCrestData.SaveData = GamblerCrestUtils.CrestSave;

            ToolItemManager.Instance.crestList.Add(GamblerCrestUtils.gamblerCrestData);

            #region BindFsmEdits
            PlayMakerFSM bind = __instance.gameObject.GetFsmPreprocessed("Bind");
            FsmBool gamblerEquipped = bind.AddBoolVariable("Is Gambler Equipped");
            FsmState CanBind = bind.GetState("Can Bind?");
            CanBind.AddAction(new CheckIfCrestEquipped()
            {
                Crest = GamblerCrestUtils.gamblerCrestData,
                storeValue = gamblerEquipped
            });


            FsmState BindType = bind.GetState("Bind Type");

            FsmState GamblerBind = bind.AddState("Gambler Bind");
            FsmEvent GamblerTrans = BindType.AddTransition("GAMBLER", GamblerBind.name);

            BindType.AddAction(new BoolTest()
            {
                boolVariable = gamblerEquipped,
                isTrue = GamblerTrans,
                everyFrame = false
            });

            GamblerBind.AddLambdaMethod((action) =>
            {
                bind.GetIntVariable("Bind Amount").Value = 1;
                bind.GetFloatVariable("Bind Time").Value = 1.2f;

                int randomChance = Random.Range(1, 21);
                randomChance = 20;
                GamblerCrest.logger.LogInfo($"{randomChance}");
                int healAmount = 0;

                if (ToolItemManager.Instance.toolItems.GetByName(""))
                {

                }
                healAmount = randomChance switch
                {
                    20 => PlayerData.instance.maxHealth,
                    >= 17 => 3,
                    >= 14 => 2,
                    >= 9 => 1,
                    >= 5 => 0,
                    >= 2 => -1,
                    _ => -2
                };

                if (healAmount == PlayerData.instance.maxHealth)
                {
                    GamblerCrestUtils.InFeverState = true;
                    GamblerCrestUtils.activateAura();
                } else if (healAmount < 0)
                {
                    HeroController.instance.TakeHealth(Mathf.Abs(healAmount));
                }

                bind.GetIntVariable("Heal Amount").Value = healAmount;
            });

            FsmState QuickBind = bind.GetState("Quick Bind?");
            GamblerBind.AddTransition("FINISHED", QuickBind.name);
            GamblerBind.AddLambdaMethod((action) =>
            {
                bind.SendEvent("FINISHED");
            });
            #endregion

            #region SilkArtFsmEdits 
            #endregion

            GamblerCrestUtils.poisonAura = __instance.quickeningEffectPrefab;
        }
    }
}
