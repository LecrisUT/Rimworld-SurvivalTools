using System.Collections.Generic;
using ToolsFramework;

namespace SurvivalTools
{
    public static class DefaultLists
    {
        public static List<ToolType> SurvivalTools = new List<ToolType>
        {
            ToolTypeDefOf.ConstructionTool,
            ToolTypeDefOf.FellingTool,
            ToolTypeDefOf.HarvestingTool,
            ToolTypeDefOf.MiningTool,
        };
    }
}
