using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace GamblerCrest.Patches
{
    [HarmonyPatch(typeof(InventoryToolCrest), nameof(InventoryToolCrest.Setup))]
    internal class AlterLayering
    {
        [HarmonyPostfix]
        public static void Postfix(InventoryToolCrest __instance)
        {
            __instance.transform.GetChild(0).transform.SetPositionZ(3.0f);
        }
    }
}
