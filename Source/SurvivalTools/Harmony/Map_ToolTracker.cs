using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using ToolsFramework;
using Verse;

namespace SurvivalTools.Harmony
{
    [HarmonyPatch]
    public static class Patch_Map_ToolTracker_BestTool
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(Map_ToolTracker), "privateBestTool");
            yield return AccessTools.Method(typeof(Map_ToolTracker), nameof(Map_ToolTracker.ClosestToolInfo));
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionList = instructions.ToList();
            var TryGetValue = AccessTools.Method(typeof(ToolExtensions), "TryGetValue", new Type[] { typeof(ToolComp), typeof(ToolType), typeof(float).MakeByRefType()});
            var NoToolWorkSpeed = AccessTools.Method(typeof(Utility), nameof(Utility.NoToolWorkSpeed));
            LocalBuilder val = null;
            Label? brLabel = null;
            for (int i = 0; i < instructionList.Count; i++)
            {
                var instruction = instructionList[i];
                if (instruction.Calls(TryGetValue))
                {
                    for (int j = 1; j < i; j++)
                    {
                        if (instructionList[i - 1].operand is LocalBuilder builder && builder.LocalType == typeof(float))
                        {
                            val = builder;
                            break;
                        }
                    }
                    var nextInstruction = instructionList[i + 1];
                    if (!nextInstruction.Branches(out brLabel))
                    {
                        Log.Error("ST_BaseMessage".Translate() + "ST_Error_Harmony_MapToolTracker".Translate());
                        break;
                    }
                }
                if (brLabel.HasValue && instruction.labels.Contains(brLabel.Value))
                    Log.Warning("ST_BaseMessage".Translate() + "ST_Error_Harmony_MapToolTracker".Translate());
                if (instruction.IsLdloc(val) && instructionList[i+1].LoadsConstant(1f) && instructionList[i+2].Branches(out _))
                {
                    instructionList.RemoveAt(i + 1);
                    instructionList.InsertRange(i + 1, new List<CodeInstruction>()
                    {
                        new CodeInstruction(OpCodes.Ldarg_1, null),
                        new CodeInstruction(OpCodes.Call, NoToolWorkSpeed),
                    });
                    break;
                }
            }
            return instructionList;
        }
    }
}