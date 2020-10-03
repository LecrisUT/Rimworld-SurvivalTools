using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ToolsFramework;
using Verse;
using Verse.AI;

namespace SurvivalTools.Harmony
{
    //[HarmonyPatch(typeof(WorkGiver_Scanner))]
    //[HarmonyPatch(nameof(WorkGiver_Scanner.JobOnThing))]
    [HarmonyPatch]
    public static class Patch_WorkGiver_Scanner_JobOnThing_JobOnCell
    {
        public static List<Type> IgnoreWorkGiverList = new List<Type> { typeof(RimWorld.WorkGiver_PlantsCut) };
        public static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (var workGiver in typeof(WorkGiver_Scanner).AllSubclasses())
            {
                if (IgnoreWorkGiverList.Contains(workGiver))
                    continue;
                var JobOnThing = AccessTools.DeclaredMethod(workGiver, "JobOnThing");
                if (JobOnThing != null)
                    yield return JobOnThing;
                var JobOnCell = AccessTools.DeclaredMethod(workGiver, "JobOnCell");
                if (JobOnCell != null)
                    yield return JobOnCell;
#if DEBUG
                Log.Message($"Test 0: Patch_WorkGiver_Scanner_JobOnThing_JobOnCell: {workGiver} : {JobOnThing} : {JobOnCell}");
#endif
            }
        }
        [HarmonyPriority(1000)]
        public static void Postfix(ref Job __result, Pawn pawn)
        {
            if (__result == null || !Settings.DisableNotToolWork || !ToolsFramework.Dictionaries.jobToolType.TryGetValue(__result.def, out var toolType) || !Dictionaries.SurvivalToolTypes[toolType] || !pawn.CanUseTools(out var tracker))
                return;
            if (tracker.UsedHandler.BestTool[toolType] != null)
                return;
            var map = pawn.MapHeld;
            if (map != null)
            {
                var reservation = map.reservationManager;
                var faction = pawn.Faction;
                var assignmentFilter = tracker.ToolAssignment.filter;
                if (pawn.MapHeld.GetMapToolTracker().StoredToolInfos.Any(t => t.comp.CompProp.ToolTypes.Contains(toolType) && !t.tool.IsForbidden(pawn) && assignmentFilter.Allows(t.tool) &&
                (reservation.ReservedBy(t.tool,pawn) || !reservation.IsReservedByAnyoneOf(t.tool, faction))))
                    return;
            }
#if DEBUG
            Log.Message($"Test 1.2: No tools for {pawn} : {__result.def}");
            JobFailReason.Is($"{pawn} lacks {toolType} for {__result.def}");
            // JobFailReason.Is("ST_NoToolForJob3".Translate(pawn, toolType, __result.def));
#else
            JobFailReason.Is($"Lacks {toolType} for job");
            // JobFailReason.Is("ST_NoToolForJob1".Translate(toolType));
#endif
            __result = null;
        }
    }
}