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
    }
}
