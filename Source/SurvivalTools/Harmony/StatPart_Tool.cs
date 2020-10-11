using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using Verse;
using ToolsFramework;

namespace SurvivalTools.Harmony
{
    [HarmonyPatch(typeof(StatPart_Tool))]
    public static class Patch_StatPart_Tool_ReportText
    {
        [HarmonyPatch("ReportText")]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var instructionList = instructions.ToList();
            var TryGetValue = AccessTools.Method(typeof(ToolExtensions), nameof(ToolExtensions.TryGetValue), new Type[] { typeof(ToolComp), typeof(ToolType), typeof(StatDef), typeof(float).MakeByRefType(), typeof(float).MakeByRefType()});
            var Helper = AccessTools.Method(typeof(Patch_StatPart_Tool_ReportText), nameof(Patch_StatPart_Tool_ReportText.Helper));
            bool flag = false;
            var AppendLine = AccessTools.Method(typeof(StringBuilder), "AppendLine", new Type[] { typeof(string) });
            for (int i = 0; i < instructionList.Count; i++)
            {
                var instruction = instructionList[i];
                if (!flag && instruction.Is(OpCodes.Ldstr, "NoTool"))
                {
                    flag = true;
                    continue;
                }
                if (flag && instruction.Calls(AppendLine))
                {
                    if (instructionList[i + 1].opcode == OpCodes.Pop)
                        i++;
                    var parentStat = AccessTools.Field(typeof(StatPart_Tool), nameof(StatPart_Tool.parentStat));
                    instructionList.InsertRange(i + 1, new List<CodeInstruction> {
                        new CodeInstruction(OpCodes.Ldarg_1, null),
                        new CodeInstruction(OpCodes.Ldarg_S, 4),
                        new CodeInstruction(OpCodes.Ldarg_0, null),
                        new CodeInstruction(OpCodes.Ldfld, parentStat),
                        new CodeInstruction(OpCodes.Call, Helper),
                    });
                    return instructionList;
                }
            }
            return null;
        }
        [HarmonyPatch("FallBack")]
        public static void Prefix(StatPart_Tool __instance, ref float val, Pawn pawn, Pawn_ToolTracker tracker)
        {
            var toolType = __instance.GetToolType(pawn, tracker, out _);
            if (toolType == null)
                return;
            var stat = __instance.parentStat;
            if (Dictionaries.SurvivalToolTypes[toolType])
            {
                var values = Dictionaries.NoToolPenalty[(toolType, stat)];
                val = (val + values.offset) * values.factor;
            }
        }
        private static void Helper(ref StringBuilder builder, ToolType toolType, StatDef stat)
        {
            if (!Settings.NoToolWorkPenalty || !Dictionaries.SurvivalToolTypes[toolType])
                return;
            var values = Dictionaries.NoToolPenalty[(toolType, stat)];
            builder.Length--;
            builder.AppendLine(" (val + " + values.offset.ToStringPercent("F2") + " ) x " + values.factor.ToStringPercent("F2"));
        }
    }
}