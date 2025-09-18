using System.Reflection;
using System.Linq;
using GamblerCrest.Utils;
using HarmonyLib;

namespace GamblerCrest.Patches
{
    [HarmonyPatch]
    internal class SaveGameSavesModdedCrest
    {
        public static void Apply(Harmony harmony)
        {
            var targetType = typeof(GameManager); // <-- replace with your actual class
            var saveGameMethods = targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(m => m.Name == "SaveGame");

            foreach (var method in saveGameMethods)
            {
                harmony.Patch(method,
                    postfix: new HarmonyMethod(typeof(SaveGameSavesModdedCrest).GetMethod(nameof(Postfix))));
            }
        }

        [HarmonyPostfix]
        public static void Postfix()
        {
            GamblerCrestUtils.SaveCrestSlots();
        }
    }
}
