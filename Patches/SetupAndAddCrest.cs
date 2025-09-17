using System.Collections.Generic;
using HarmonyLib;
using TeamCherry.Localization;
using GamblerCrest.Extensions;
using UnityEngine;
using Silksong.FsmUtil;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using System.Reflection;

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
            Sprite crestSilhouette = reaper.CrestSilhouette;

            Texture2D diceCrest = ModHelper.LoadTexFromAssembly("GamblerCrest.Resources.Images.gamblerCrest.png");
            Sprite crestSprite = Sprite.Create(diceCrest, new(0,0, diceCrest.width, diceCrest.height), new(0.5f,0.5f), 166.67f);
            HeroControllerConfig heroConfig = hunter.HeroConfig;

            ToolCrest crestData = new ToolCrest();
            crestData.name = "GAMBLER";
            crestData.SetCrestGlow(crestGlow);
            crestData.SetCrestSilhouette(crestSilhouette);
            crestData.SetCrestSprite(crestSprite);
            crestData.SetHeroControllerConfig(heroConfig);

            LocalisedString dispName = new LocalisedString() { Key = "GAMBLERCRESTNAME", Sheet = "GAMBLER" };
            LocalisedString description = new LocalisedString() { Key = "GAMBLERCRESTDESC", Sheet = "GAMBLER" };

            crestData.SetDisplayName(dispName);
            crestData.SetDisplayDesc(description);

            crestData.SetSlotInfo([
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
            ]);
            crestData.SaveData = new ToolCrestsData.Data
            {
                IsUnlocked = true,
                Slots = [],
                DisplayNewIndicator = true,
            };

            ToolItemManager.Instance.AddCrest(crestData);

            PlayMakerFSM bind = __instance.gameObject.GetFsmPreprocessed("Bind");
            FsmState[] bindStates = bind.FsmStates;
            foreach (var state in bindStates)
            {
                //ModHelper.Log(state.Name);
            }
            FsmState doBind = bind.GetState("Do Bind");
            foreach (FsmStateAction action in doBind.ActiveActions)
            {
                ModHelper.Log(action.Name);
            }
        }
    }
}
