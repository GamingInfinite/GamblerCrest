using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using TeamCherry.Localization;

namespace GamblerCrest.Patches.Localization;

[HarmonyPatch]
public static class Localization
{
    public static bool LocalizationPatched = false;

    [HarmonyPatch(typeof(Language), nameof(Language.SwitchLanguage), typeof(LanguageCode))]
    [HarmonyPostfix]
    private static void AddNewSheet()
    {
        Dictionary<string, Dictionary<string, string>> fullStore = Helper.GetPrivateStaticField<Dictionary<string, Dictionary<string, string>>>(typeof(Language), "_currentEntrySheets");

        fullStore.Add("GAMBLER", new Dictionary<string, string>()
        {
            { "GAMBLERCRESTNAME", "High-Stakes" },
            { "GAMBLERCRESTDESC", "Test your luck and strive in battle" }
        });

        Helper.SetPrivateStaticField(typeof(Language), "_currentEntrySheets", fullStore);
        LocalizationPatched = true;
    }
}

public static class Helper
{
    public static void SetPrivateStaticField<T>(Type type, string fieldName, T value)
    {
        var field = type.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
        if (field == null)
            throw new MissingFieldException(type.FullName, fieldName);

        field.SetValue(null, value); // null because it's static
    }

    public static T GetPrivateStaticField<T>(Type type, string fieldName)
    {
        var field = type.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
        if (field == null)
            throw new MissingFieldException(type.FullName, fieldName);

        return (T)field.GetValue(null); // null because it's static
    }
}