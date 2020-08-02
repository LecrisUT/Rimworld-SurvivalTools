using ToolsFramework;
using Verse;

namespace SurvivalTools
{
    public static class Utility
    {
        public static float NoToolWorkSpeed(this ToolType toolType)
            => Settings.ST_toolTypes.Contains(toolType) ? Settings.NoToolWorkSpeed : 1f;
        public static bool MeetsRequirementJobs(this WorkGiver_Extension extension, Pawn pawn)
        {
            if (!pawn.CanUseTools(out var tracker))
                return true;
            var bestTools = tracker.usedHandler.BestTool;
            if (extension.toolTypes.Any(t => Settings.ST_toolTypes.Contains(t) && !bestTools.ContainsKey(t)))
                return false;
            return true;
        }
    }
}
