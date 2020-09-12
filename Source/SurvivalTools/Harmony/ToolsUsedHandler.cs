using AutoPatcher;
using AutoPatcher.Utility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using ToolsFramework;
using Verse;

namespace SurvivalTools.Harmony
{
    [HarmonyPatch(typeof(ToolsUsedHandler), "Update")]
    public static class Patch_ToolsUsedHandler_Update
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionList = instructions.ToList();
            var GetValue = AccessTools.Method(typeof(ToolExtensions), nameof(ToolExtensions.GetValue), new Type[] { typeof(ToolsFramework.Tool), typeof(ToolType), typeof(float) });
            var NoToolWorkSpeed = AccessTools.Method(typeof(Utility), nameof(Utility.NoToolWorkSpeed));
            LocalVar currVal = null;
            LocalVar toolType = null;
            for (int i = 0; i < instructionList.Count; i++)
            {
                var instruction = instructionList[i];
                if (instruction.Calls(GetValue))
                {
                    if (!instructionList[i + 1].IsStloc() || !instructionList[i - 2].IsLdloc())
                    {
                        Log.Error("ST_BaseMessage".Translate() + "ST_Error_Harmony_ToolsUsedHandler".Translate());
                        break;
                    }
                    currVal = instructionList[i + 1].ToLocalVar();
                    toolType = instructionList[i - 2].ToLocalVar();
                    
                }
                if (currVal != null && instruction.IsLdloc(currVal) && instructionList[i + 1].LoadsConstant(1f) && instructionList[i + 2].Branches(out _))
                {
                    instructionList.RemoveAt(i + 1);
                    instructionList.InsertRange(i + 1, new List<CodeInstruction>()
                    {
                        new CodeInstruction(OpCodes.Ldloc_S, toolType.index),
                        new CodeInstruction(OpCodes.Call, NoToolWorkSpeed),
                    });
                    break;
                }
            }
            return instructionList;
        }
    }
}