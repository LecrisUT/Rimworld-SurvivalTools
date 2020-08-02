using HarmonyLib;
using Verse;
using ToolsFramework;

namespace SurvivalTools.Harmony
{
    [HarmonyPatch(typeof(RoofUtility))]
    [HarmonyPatch(nameof(RoofUtility.CanHandleBlockingThing))]
    public static class Patch_RoofUtility_CanHandleBlockingThing
    {
        public static void Postfix(ref bool __result, Thing blocker, Pawn worker)
        {
            if (blocker?.def.plant?.IsTree == true && !WorkGiverDefOf.FellTrees.GetModExtension<WorkGiver_Extension>().MeetsRequirementJobs(worker))
                __result = false;
        }
    }
}