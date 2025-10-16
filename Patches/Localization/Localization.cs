using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using TeamCherry.Localization;

namespace GamblerCrest.Patches.Localization;

[HarmonyPatch(typeof(Language), nameof(Language.SwitchLanguage), typeof(LanguageCode))]
public static class Localization
{
    [HarmonyPostfix]
    private static void AddNewSheet()
    {
        Dictionary<string, Dictionary<string, string>> fullStore = Language._currentEntrySheets;

        if (!fullStore.ContainsKey("GAMBLER"))
        {
            fullStore.Add("GAMBLER", new Dictionary<string, string>()
            {
                { "GAMBLERCRESTNAME", "High-Stakes" },
                { "GAMBLERCRESTDESC", "Test your luck and strive in battle" }
            });
        }

        Language._currentEntrySheets = fullStore;
    }
}