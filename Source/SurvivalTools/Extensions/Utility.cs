using ToolsFramework;

namespace SurvivalTools
{
    public static class Utility
    {
        public static float NoToolWorkSpeed(this ToolType toolType)
            => Dictionaries.SurvivalToolTypes[toolType] ? Settings.NoToolWorkFactor : 1f;
    }
}
