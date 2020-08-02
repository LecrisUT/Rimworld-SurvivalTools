using HarmonyLib;
using RimWorld;
using ToolsFramework;
using Verse;
using Verse.AI;

namespace SurvivalTools.Harmony
{
    [HarmonyPatch(typeof(WorkGiver_GrowerSow))]
    [HarmonyPatch(nameof(WorkGiver_GrowerSow.JobOnCell))]
    public static class Patch_WorkGiver_GrowerSow_JobOnCell
    {
        public static void Postfix(ref Job __result, Pawn pawn)
        {
            if (__result?.def == RimWorld.JobDefOf.CutPlant && __result.targetA.Thing.def.plant.IsTree)
            {
                if (WorkGiverDefOf.FellTrees.GetModExtension<WorkGiver_Extension>().MeetsRequirementJobs(pawn))
                    __result = new Job(JobDefOf.FellTree, __result.targetA);
                else
                    __result = null;
            }
        }
    }
}