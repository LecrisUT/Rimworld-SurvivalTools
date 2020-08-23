using HugsLib;
using System.Collections.Generic;
using System.Linq;
using ToolsFramework;

namespace SurvivalTools
{
    internal class Controller : ModBase
    {
        public Settings settings;
        public static List<ToolType> defaultSurvivalTools = new List<ToolType>
        {
            ToolTypeDefOf.ConstructionTool,
            ToolTypeDefOf.FellingTool,
            ToolTypeDefOf.HarvestingTool,
            ToolTypeDefOf.MiningTool,
        };
        public override void DefsLoaded()
        {
            settings = SurvivalTools.thisMod.GetSettings<Settings>();
        }
    }
}
