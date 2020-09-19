using HarmonyLib;
using RimWorld;
using Verse.AI;

namespace SurvivalTools.Harmony
{
    [HarmonyPatch(typeof(WorkGiver_GrowerSow))]
    [HarmonyPatch(nameof(WorkGiver_GrowerSow.JobOnCell))]
    public static class Patch_WorkGiver_GrowerSow_JobOnCell
    {
        public static void Postfix(ref Job __result)
        {
            if (__result?.def == RimWorld.JobDefOf.CutPlant && __result.targetA.Thing.def.plant.IsTree)
                __result.def = JobDefOf.FellTree;
        }
    }
}