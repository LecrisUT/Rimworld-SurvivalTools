using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using ToolsFramework;
using ToolsFramework.Harmony;
using Verse;

namespace SurvivalTools.Harmony
{
    [HarmonyPatch(typeof(Patch_StatWorker_GetExplanationFinalizePart), "ReportText")]
    public static class Patch_Patch_StatWorker_GetExplanationFinalizePart
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionList = instructions.ToList();
            var GetStatValue = AccessTools.Method(typeof(ToolExtensions), "GetStatValue");
            var NoToolWorkSpeed = AccessTools.Method(typeof(Utility), nameof(Utility.NoToolWorkSpeed));
            for (int i = 0; i < instructionList.Count; i++)
            {
                var instruction = instructionList[i];
                if (instruction.Calls(GetStatValue) && instructionList[i - 1].LoadsConstant(1f))
                {
                    instructionList.RemoveAt(i - 1);
                    instructionList.InsertRange(i - 1, new List<CodeInstruction>
                    {
                        new CodeInstruction(OpCodes.Ldarg, 4),
                        new CodeInstruction(OpCodes.Call, NoToolWorkSpeed),
                    });
                }
            }
            return instructionList;
        }
    }
}