using HarmonyLib;
using RimWorld;
using ToolsFramework;
using ToolsFramework.AutoPatcher;

namespace SurvivalTools.Harmony
{
    [HarmonyPatch(typeof(StatPatch))]
    [HarmonyPatch(nameof(StatPatch.GetStatValueJob_Fallback))]
    public static class Patch_StatPatch_GetStatValueJob_Fallback
    {
        public static void Postfix(ref float __result, ToolType toolType, StatDef stat)
        {
            if (Dictionaries.SurvivalToolTypes[toolType])
            {
                var values = Dictionaries.NoToolPenalty[(toolType, stat)];
                __result = (__result + values.offset) * values.factor;
            }
        }
    }
}