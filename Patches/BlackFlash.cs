using System;
using HarmonyLib;
using UnityEngine;

namespace GamblerCrest.Patches
{
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.NailHitEnemy))]
    internal class BlackFlash
    {
        [HarmonyPostfix]
        public static void Postfix(HealthManager enemyHealth, HitInstance hitInstance)
        {
            float BFChance = UnityEngine.Random.Range(1f, 100f);
            if (PlayerData.instance.CurrentCrestID == "GAMBLER" && BFChance > 95)
            {
                int damageDealt = Mathf.RoundToInt(hitInstance.DamageDealt * hitInstance.Multiplier);
                float blackFlashDamage = (float)(Math.Pow(damageDealt, 2.5) - damageDealt);

                if (enemyHealth.sendDamageTo == null)
                {
                    enemyHealth.hp = Mathf.Max(Mathf.RoundToInt(enemyHealth.hp - blackFlashDamage), -1000);
                }
                else
                {
                    enemyHealth.sendDamageTo.hp = Mathf.Max(Mathf.RoundToInt(enemyHealth.hp - blackFlashDamage), -1000);
                }
            }
        }
    }
}
