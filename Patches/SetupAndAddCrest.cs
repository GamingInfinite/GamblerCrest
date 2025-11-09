using HarmonyLib;
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
