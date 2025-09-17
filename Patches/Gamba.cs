using HarmonyLib;
using UnityEngine;

namespace GamblerCrest.Patches
{
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.BindCompleted))]
    internal class Gamba
    {
        [HarmonyPostfix]
        public static void Postfix(HeroController __instance)
        {
            GamblerCrest.logger.LogInfo("Is Testing");
            if (PlayerData.instance.CurrentCrestID == "GAMBLER")
            {
                int randomChance = Random.Range(1,21);
                HeroController.instance.AddHealth(-3);
                GamblerCrest.logger.LogInfo($"{randomChance}");
                __instance.SetDamageMode(GlobalEnums.DamageMode.NO_DAMAGE);
                if (randomChance == 20)
                {
                    PlayerData.instance.health = PlayerData.instance.maxHealth;
                }
            }
        }
    }
}
