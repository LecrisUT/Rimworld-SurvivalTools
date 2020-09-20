using RimWorld;
using System.Collections.Generic;
using System.Linq;
using ToolsFramework;
using Verse;

namespace SurvivalTools
{
    public static class Dictionaries
    {
        public static Dictionary<ToolType, bool> SurvivalToolTypes = new Dictionary<ToolType, bool>();
        public static Dictionary<(ToolType, StatDef), (float factor, float offset)> NoToolPenalty = new Dictionary<(ToolType, StatDef), (float, float)>();
        public static void SetDictionaries()
        {
            foreach (var toolType in DefDatabase<ToolType>.AllDefs)
            {
                if (!SurvivalToolTypes.ContainsKey(toolType))
                    SurvivalToolTypes.Add(toolType, DefaultLists.SurvivalTools.Contains(toolType));
                foreach (var stat in toolType.workStatFactors.Select(t => t.stat))
                {
                    var val = Settings.NoToolWorkFactor;
                    if (NoToolPenalty.TryGetValue((toolType, stat), out var values))
                    {
                        values.factor = val;
                        NoToolPenalty[(toolType, stat)] = values;
                    }
                    else
                        NoToolPenalty.Add((toolType, stat), (val, 0f));
                }
                foreach (var stat in toolType.workStatOffset.Select(t => t.stat))
                {
                    var val = Settings.NoToolWorkOffset;
                    if (NoToolPenalty.TryGetValue((toolType, stat), out var values))
                    {
                        values.offset = val;
                        NoToolPenalty[(toolType, stat)] = values;
                    }
                    else
                        NoToolPenalty.Add((toolType, stat), (1f, val));
                }
            }
        }
    }
}
