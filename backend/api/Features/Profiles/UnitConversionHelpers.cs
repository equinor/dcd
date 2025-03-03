using api.Models.Enums;

using static api.Features.Profiles.VolumeConstants;

namespace api.Features.Profiles;

public static class UnitConversionHelpers
{
    private static readonly Dictionary<string, double> ConversionFactors = new()
    {
        { ProfileTypes.Co2Emissions, 1_000_000 },
        { ProfileTypes.Co2EmissionsOverride, 1_000_000 },
        { ProfileTypes.ProductionProfileNgl, 1_000_000 },
        { ProfileTypes.ProductionProfileNglOverride, 1_000_000 },
        { ProfileTypes.CondensateProduction, 1_000_000 },
        { ProfileTypes.CondensateProductionOverride, 1_000_000 },
        { ProfileTypes.ProductionProfileOil, 1_000_000 },
        { ProfileTypes.AdditionalProductionProfileOil, 1_000_000 },
        { ProfileTypes.ProductionProfileWater, 1_000_000 },
        { ProfileTypes.ProductionProfileWaterInjection, 1_000_000 },
        { ProfileTypes.ProductionProfileGas, 1_000_000_000 },
        { ProfileTypes.AdditionalProductionProfileGas, 1_000_000_000 },
        { ProfileTypes.FuelFlaringAndLosses, 1_000_000_000 },
        { ProfileTypes.FuelFlaringAndLossesOverride, 1_000_000_000 },
        { ProfileTypes.NetSalesGas, 1_000_000_000 },
        { ProfileTypes.NetSalesGasOverride, 1_000_000_000 },
        { ProfileTypes.TotalExportedVolumes, 1_000_000 },
        { ProfileTypes.TotalExportedVolumesOverride, 1_000_000 }
    };

    public static double[] ConvertValuesToDto(double[] values, PhysUnit unit, string type)
    {
        var conversionFactor = GetConversionFactor(type, unit, toDto: true);

        return Array.ConvertAll(values, x => Math.Round(x * conversionFactor, 10));
    }

    public static double[] ConvertValuesFromDto(double[]? values, PhysUnit unit, string type)
    {
        if (values == null)
        {
            return [];
        }

        var conversionFactor = GetConversionFactor(type, unit, toDto: false);

        return Array.ConvertAll(values, x => Math.Round(x * conversionFactor, 10));
    }

    private static double GetConversionFactor(string type, PhysUnit unit, bool toDto)
    {
        var returnValue = 1.0;

        if (ConversionFactors.TryGetValue(type, out var conversionFactor))
        {
            returnValue = toDto ? 1.0 / conversionFactor : conversionFactor;
        }

        if (unit == PhysUnit.OilField)
        {
            switch (type)
            {
                case ProfileTypes.ProductionProfileOil:
                case ProfileTypes.AdditionalProductionProfileOil:
                case ProfileTypes.ProductionProfileWater:
                case ProfileTypes.ProductionProfileWaterInjection:
                    return toDto
                        ? BarrelsPerCubicMeter * returnValue
                        : 1.0 / BarrelsPerCubicMeter * returnValue;
                case ProfileTypes.ProductionProfileGas:
                case ProfileTypes.AdditionalProductionProfileGas:
                case ProfileTypes.FuelFlaringAndLosses:
                case ProfileTypes.FuelFlaringAndLossesOverride:
                case ProfileTypes.DeferredOilProduction:
                case ProfileTypes.DeferredGasProduction:
                case ProfileTypes.NetSalesGas:
                case ProfileTypes.NetSalesGasOverride:
                    return toDto
                        ? CubicFeetPerCubicMeter * returnValue
                        : 1.0 / CubicFeetPerCubicMeter * returnValue;
            }
        }

        return returnValue;
    }

    public static readonly IReadOnlySet<string> ProfileTypesWithConversion = new HashSet<string>
    {
        ProfileTypes.AdditionalProductionProfileGas,
        ProfileTypes.AdditionalProductionProfileOil,
        ProfileTypes.Co2Emissions,
        ProfileTypes.Co2EmissionsOverride,
        ProfileTypes.DeferredGasProduction,
        ProfileTypes.DeferredOilProduction,
        ProfileTypes.FuelFlaringAndLosses,
        ProfileTypes.FuelFlaringAndLossesOverride,
        ProfileTypes.NetSalesGas,
        ProfileTypes.NetSalesGasOverride,
        ProfileTypes.TotalExportedVolumes,
        ProfileTypes.TotalExportedVolumesOverride,
        ProfileTypes.ProductionProfileGas,
        ProfileTypes.ProductionProfileNgl,
        ProfileTypes.ProductionProfileNglOverride,
        ProfileTypes.CondensateProduction,
        ProfileTypes.CondensateProductionOverride,
        ProfileTypes.ProductionProfileOil,
        ProfileTypes.ProductionProfileWater,
        ProfileTypes.ProductionProfileWaterInjection
    };
}
