using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using BattleTech;
using Harmony;

namespace MechTonnageFloatingPointFixer
{
    [HarmonyPatch(typeof(MechValidationRules), "ValidateMechTonnage")]
    static class ValidateMechTonnage_Patch
    {
        // drop  a call to Calculator.SameTonnageWithEpsilon and early return early from method if things are same. 
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionList = instructions.ToList();
            var calculateMethod = AccessTools.Method(typeof(MechStatisticsRules), "CalculateTonnage");
            var insertionIndex = instructionList.FindIndex(instruction =>
                                     instruction.opcode == OpCodes.Call && instruction.operand == calculateMethod) + 1;
            var instructionsToInsert = new List<CodeInstruction>();
            instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldloc_0));
            instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldarg_1));
            var chassisMethod = AccessTools.Method(typeof(MechDef), "get_Chassis");
            instructionsToInsert.Add(new CodeInstruction(OpCodes.Callvirt, chassisMethod));
            var tonnageMethod = AccessTools.Method(typeof(ChassisDef), "get_Tonnage");
            instructionsToInsert.Add(new CodeInstruction(OpCodes.Callvirt, tonnageMethod));
            var comparisonMethod = AccessTools.Method(typeof(Calculator), "SameTonnageWithEpsilon",
                new Type[] {typeof(float), typeof(float)});
            instructionsToInsert.Add(new CodeInstruction(OpCodes.Call, comparisonMethod));
            var returnIndex = instructionList.FindIndex(instruction => instruction.opcode == OpCodes.Ret);
            instructionsToInsert.Add(new CodeInstruction(OpCodes.Brtrue, instructionList[returnIndex].labels[0]));
            instructionList.InsertRange(insertionIndex, instructionsToInsert);
            return instructionList;
        }
    }
}