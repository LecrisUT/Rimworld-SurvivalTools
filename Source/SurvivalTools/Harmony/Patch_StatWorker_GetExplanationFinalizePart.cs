using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using ToolsFramework;
using ToolsFramework.Harmony;

namespace SurvivalTools.Harmony
{
    [HarmonyPatch(typeof(Patch_StatWorker_GetExplanationFinalizePart), "ReportText")]
    public static class Patch_Patch_StatWorker_GetExplanationFinalizePart
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var instructionList = instructions.ToList();
            var TryGetValue = AccessTools.Method(typeof(ToolExtensions), nameof(ToolExtensions.TryGetValue), new Type[] { typeof(Tool), typeof(ToolType), typeof(StatDef), typeof(float).MakeByRefType(), typeof(float).MakeByRefType() });
            var Helper = AccessTools.Method(typeof(Patch_Patch_StatWorker_GetExplanationFinalizePart), nameof(Patch_Patch_StatWorker_GetExplanationFinalizePart.Helper));
            for (int i = 0; i < instructionList.Count; i++)
            {
                var instruction = instructionList[i];
                if (instruction.Calls(TryGetValue))
                {
                    var label = generator.DefineLabel();
                    if (instructionList[i + 1].opcode == OpCodes.Pop)
                        instructionList.RemoveAt(i + 1);
                    instructionList[i + 1].labels.Add(label);

                    instructionList.InsertRange(i + 1, new List<CodeInstruction>
                    {
                        new CodeInstruction(OpCodes.Brtrue_S, label),
                        instructionList[i-4],
                        instructionList[i-3],
                        instructionList[i-2],
                        instructionList[i-1],
                        new CodeInstruction(OpCodes.Call, Helper),
                    });
                }
            }
            return instructionList;
        }
        private static void Helper(ToolType toolType, StatDef stat, ref float factor, ref float offset)
        {
            if (!Settings.NoToolWorkPenalty || !Dictionaries.SurvivalToolTypes[toolType])
                return;
            var values = Dictionaries.NoToolPenalty[(toolType, stat)];
            factor = values.factor;
            offset = values.offset;
        }
    }
}