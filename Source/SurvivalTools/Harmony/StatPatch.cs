using HarmonyLib;
using ToolsFramework;
using ToolsFramework.AutoPatcher;

namespace SurvivalTools.Harmony
{
    [HarmonyPatch(typeof(StatPatch))]
    [HarmonyPatch(nameof(StatPatch.GetStatValueJob_Fallback))]
    public static class Patch_StatPatch_GetStatValueJob_Fallback
    {
        public static void Postfix(ref float __result, ToolType toolType)
        {
            if (Settings.ST_toolTypes[toolType])
                __result *= Settings.NoToolWorkSpeed;
        }
    }
}