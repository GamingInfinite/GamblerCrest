using System;
using GamblerCrest.Utils;
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
            if (PlayerData.instance.CurrentCrestID == "GAMBLER")
            {
                float BFChance = UnityEngine.Random.Range(1f, 100f);
                float BFTopChance = 100 - (GamblerCrestUtils.blackFlashChance + GamblerCrestUtils.BlackFlashChanceBonus);
                ModHelper.Log($"{BFChance}/{BFTopChance}");
                if (BFChance >= BFTopChance && hitInstance.AttackType == AttackTypes.Heavy)
                {
                    ModHelper.Log("DID BLACK FLASH");
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
                else
                {
                    GamblerCrestUtils.BlackFlashChanceBonus += 1f;
                }
            }
        }
    }

    [HarmonyPatch(typeof(HeroController), nameof(HeroController.NeedleArtRecovery))]
    public class NeedleArtWhiffPunish
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            GamblerCrestUtils.BlackFlashChanceBonus = 0;
        }
    }
}
