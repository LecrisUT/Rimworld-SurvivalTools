using HarmonyLib;
using Verse;
using Verse.AI;

namespace SurvivalTools.Harmony
{
    [HarmonyPatch(typeof(RoofUtility))]
    [HarmonyPatch(nameof(RoofUtility.HandleBlockingThingJob))]
    public static class Patch_RoofUtility_HandleBlockingThingJob
    {
        public static void Postfix(ref Job __result)
        {
            if (__result?.def == RimWorld.JobDefOf.CutPlant && __result.targetA.Thing.def.plant.IsTree)
                __result.def = JobDefOf.FellTree;
        }
    }
}