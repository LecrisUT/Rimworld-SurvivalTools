using HarmonyLib;
using RimWorld;
using ToolsFramework;
using Verse;

namespace SurvivalTools.HarmonyPatches
{
    [HarmonyPatch(typeof(WorkGiver))]
    [HarmonyPatch(nameof(WorkGiver.MissingRequiredCapacity))]
    public static class Patch_WorkGiver_MissingRequiredCapacity
    {
        public static void Postfix(WorkGiver __instance, ref PawnCapacityDef __result, Pawn pawn)
        {
            // Bit of a weird hack for now, but I hope to make this a bit more elegant in the future
            if (__result == null)
            {
                if (__instance.def.GetModExtension<WorkGiver_Extension>() is WorkGiver_Extension ext && !ext.MeetsRequirementJobs(pawn))
                    __result = PawnCapacityDefOf.Manipulation;
            }
        }
    }
}