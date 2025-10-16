﻿using System.Collections.Generic;
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
            GamblerCrestUtils.poisonAura = __instance.quickeningEffectPrefab;
        }
    }
}
