using System.Collections.Generic;
using System.Linq;
using ToolsFramework;
using Verse;

namespace SurvivalTools
{
    public static class Utility
    {
        public static float NoToolWorkSpeed(this ToolType toolType)
            => Settings.ST_toolTypes[toolType] ? Settings.NoToolWorkSpeed : 1f;
        public static bool MeetsRequirementJobs(this WorkGiver_Extension extension, Pawn pawn)
        {
            if (!Settings.DisableNotToolWork || !pawn.CanUseTools(out var tracker))
                return true;
            var bestTools = tracker.usedHandler.BestTool;
            var mapTools = pawn.MapHeld.GetComponent<Map_ToolTracker>().StoredTools;
            foreach (var toolType in extension.toolTypes.Where(t => Settings.ST_toolTypes[t]))
            {
                if (bestTools[toolType] != null)
                    continue;
                if (mapTools.Any(t => t.ToolProperties.ToolTypes.Contains(toolType)))
                    continue;
                return false;
            }
            return true;
        }
    }
}
