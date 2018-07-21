using System;
using System.Collections.Generic;
using BattleTech;
using BattleTech.Data;
using Harmony;
using static MechTonnageFloatingPointFixer.Core;

namespace MechTonnageFloatingPointFixer
{
    [HarmonyPatch(typeof(MechValidationRules), "ValidateMechTonnage")]
    static class ValidateMechTonnage_Patch
    {
        static bool Prefix(DataManager dataManager, MechDef mechDef,
            Dictionary<MechValidationType, List<string>> errorMessages)
        {
            var maxTonnage = mechDef.Chassis.Tonnage;
            var currentTonnage = 0f;
            var _throwaway = 0f;
            MechStatisticsRules.CalculateTonnage(mechDef, ref currentTonnage, ref _throwaway);
            Logger.Debug(
                $"mech: {mechDef.Name}\n" +
                $"current: {currentTonnage}\n" +
                $"max: {maxTonnage}"
            );

            if (Math.Abs(currentTonnage - maxTonnage) < ModSettings.Epsilon)
            {
                Logger.Debug("roughly the same!");
                return false;
            }

            if (currentTonnage > maxTonnage)
            {
                Logger.Debug("overweight");
                Traverse.Create(typeof(MechValidationRules)).Method("AddErrorMessage",
                    new object[]
                    {
                        errorMessages,
                        MechValidationType.Overweight,
                        string.Format("OVERWEIGHT: 'Mech weight exceeds maximum tonnage for the Chassis", new object[0])
                    });
            }

            if (currentTonnage < maxTonnage)
            {
                Logger.Debug("underweight");
                Traverse.Create(typeof(MechValidationRules)).Method("AddErrorMessage",
                    new object[]
                    {
                        errorMessages,
                        MechValidationType.Underweight,
                        string.Format("UNDERWEIGHT: 'Mech has unused tonnage", new object[0])
                    });
            }

            return false;
        }
    }
}