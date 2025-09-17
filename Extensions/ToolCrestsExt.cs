using HarmonyLib;
using TeamCherry.Localization;
using UnityEngine;

namespace GamblerCrest.Extensions
{
    internal static class ToolCrestsExt
    {
        public static Sprite GetCrestSprite(this ToolCrest crest)
        {
            return (Sprite)Traverse.Create(crest).Field("crestSprite").GetValue(crest);
        }

        public static void SetCrestSprite(this ToolCrest crest, Sprite sprite)
        {
            Traverse.Create(crest).Field("crestSprite").SetValue(sprite);
        }

        public static Sprite GetCrestGlow(this ToolCrest crest)
        {
            return (Sprite)Traverse.Create(crest).Field("crestGlow").GetValue(crest);
        }

        public static void SetCrestGlow(this ToolCrest crest, Sprite sprite)
        {
            Traverse.Create(crest).Field("crestGlow").SetValue(sprite);
        }

        public static Sprite GetCrestSilhouette(this ToolCrest crest)
        {
            return (Sprite)Traverse.Create(crest).Field("crestSilhouette").GetValue(crest);
        }

        public static void SetCrestSilhouette(this ToolCrest crest, Sprite sprite)
        {
            Traverse.Create(crest).Field("crestSilhouette").SetValue(sprite);
        }

        public static HeroControllerConfig GetHeroControllerConfig(this ToolCrest crest)
        {
            return (HeroControllerConfig)Traverse.Create(crest).Field("heroConfig").GetValue();
        }

        public static void SetHeroControllerConfig(this ToolCrest crest, HeroControllerConfig config)
        {
            Traverse.Create(crest).Field("heroConfig").SetValue(config);
        }

        public static void SetDisplayName(this ToolCrest crest, LocalisedString str)
        {
            Traverse.Create(crest).Field("displayName").SetValue(str);
        }

        public static void SetDisplayDesc(this ToolCrest crest, LocalisedString desc)
        {
            Traverse.Create(crest).Field("description").SetValue(desc);
        }

        public static void SetSlotInfo(this ToolCrest crest, ToolCrest.SlotInfo[] slots)
        {
            Traverse.Create(crest).Field("slots").SetValue(slots);
        }
    }
}
