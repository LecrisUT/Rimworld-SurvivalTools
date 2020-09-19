using HarmonyLib;
using ToolsFramework;
using ToolsFramework.AutoPatcher;
using Verse;
using Verse.AI;

namespace SurvivalTools.Harmony
{
    [HarmonyPatch(typeof(StatPatch))]
    [HarmonyPatch(nameof(StatPatch.GetStatValueJob_Fallback))]
    public static class Patch_StatPatch_GetStatValueJob_Fallback
    {
        public static void Postfix(ref float __result, Pawn pawn, Job job)
        {
            if (!pawn.CanUseTools() || !ToolsFramework.Dictionaries.jobToolType.TryGetValue(job.def, out var toolType))
                return;
            if (Settings.ST_toolTypes[toolType])
                __result *= Settings.NoToolWorkSpeed;
        }
    }
}