using System;

namespace MechTonnageFloatingPointFixer
{
    public static class Calculator
    {
        public static bool SameTonnageWithEpsilon(float currentTonnage, float maxChassisTonnage)
        {
            var isSameish = Math.Abs(currentTonnage - maxChassisTonnage) < Core.ModSettings.Epsilon;
            Logger.Debug($"isSameish? {isSameish}\nepsilon: {Core.ModSettings.epsilon}\ncurrentTonnnage: {currentTonnage}\nmaxChassisTonnage: {maxChassisTonnage}");
            return isSameish;
        }
    }
}