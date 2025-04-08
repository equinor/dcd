using api.Models.Enums;

using static api.Features.Profiles.CalculationConstants;

namespace api.Features.Profiles;

public static class UnitConversionHelpers
{
    private static readonly Dictionary<string, double> ConversionFactors = new()
    {
        { ProfileTypes.Co2Emissions, Mega },
        { ProfileTypes.Co2EmissionsOverride, Mega },
        { ProfileTypes.ProductionProfileNgl, Mega },
        { ProfileTypes.ProductionProfileNglOverride, Mega },
        { ProfileTypes.CondensateProduction, Mega },
        { ProfileTypes.CondensateProductionOverride, Mega },
        { ProfileTypes.ProductionProfileOil, Mega },
        { ProfileTypes.AdditionalProductionProfileOil, Mega },
        { ProfileTypes.ProductionProfileWater, Mega },
        { ProfileTypes.ProductionProfileWaterInjection, Mega },
        { ProfileTypes.ProductionProfileGas, Giga },
        { ProfileTypes.AdditionalProductionProfileGas, Giga },
        { ProfileTypes.FuelFlaringAndLosses, Giga },
        { ProfileTypes.FuelFlaringAndLossesOverride, Giga },
        { ProfileTypes.NetSalesGas, Giga },
        { ProfileTypes.NetSalesGasOverride, Giga },
        { ProfileTypes.TotalExportedVolumes, Mega },
        { ProfileTypes.TotalExportedVolumesOverride, Mega },
        { ProfileTypes.CalculatedTotalGasIncomeCostProfile, Mega },
        { ProfileTypes.CalculatedTotalOilIncomeCostProfile, Mega },
        { ProfileTypes.CalculatedTotalIncomeCostProfile, Mega },
        { ProfileTypes.CalculatedTotalCashflow, Mega },
        { ProfileTypes.CalculatedTotalCostCostProfile, Mega },
        { ProfileTypes.CalculatedDiscountedCashflow, Mega }
    };

    public static double[] ConvertValuesToDto(double[] values, PhysUnit unit, Currency currency, double usdToNok, string type)
    {
        var conversionFactor = GetConversionFactor(type, unit, currency, usdToNok, toDto: true);

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
        var prefixFactor = GetPrefixFactor(type, toDto);
        var unitFactor = GetUnitFactor(unit, type, toDto);

        return prefixFactor * unitFactor;
    }

    private static double GetConversionFactor(string type, PhysUnit unit, Currency currency, double usdToNok, bool toDto)
    {
        //var currencyFactor = GetCurrencyFactor(currency, type, usdToNok);
        //return currencyFactor * GetConversionFactor(type, unit, toDto);
        return GetConversionFactor(type, unit, toDto);
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

    private static double GetPrefixFactor(string type, bool toDto)
    {
        if (ConversionFactors.TryGetValue(type, out var conversionFactor))
        {
            return toDto ? 1.0 / conversionFactor : conversionFactor;
        }
        return 1.0;
    }

    private static double GetUnitFactor(PhysUnit unit, string type, bool toDto)
    {
        if (unit == PhysUnit.OilField)
        {
            switch (type)
            {
                case ProfileTypes.ProductionProfileOil:
                case ProfileTypes.AdditionalProductionProfileOil:
                case ProfileTypes.ProductionProfileWater:
                case ProfileTypes.ProductionProfileWaterInjection:
                    return toDto
                        ? BarrelsPerCubicMeter
                        : 1.0 / BarrelsPerCubicMeter;
                case ProfileTypes.ProductionProfileGas:
                case ProfileTypes.AdditionalProductionProfileGas:
                case ProfileTypes.FuelFlaringAndLosses:
                case ProfileTypes.FuelFlaringAndLossesOverride:
                case ProfileTypes.DeferredOilProduction:
                case ProfileTypes.DeferredGasProduction:
                case ProfileTypes.NetSalesGas:
                case ProfileTypes.NetSalesGasOverride:
                    return toDto
                        ? CubicFeetPerCubicMeter
                        : 1.0 / CubicFeetPerCubicMeter;
            }
        }
        return 1;
    }

    private static double GetCurrencyFactor(Currency currency, string type, double usdToNok)
    {
        switch (currency)
        {
            case Currency.Nok:
                return type switch
                {
                    ProfileTypes.CalculatedTotalIncomeCostProfile => usdToNok,
                    ProfileTypes.CalculatedTotalOilIncomeCostProfile => usdToNok,
                    _ => 1.0
                };
            case Currency.Usd:
                return type switch
                {
                    ProfileTypes.CalculatedTotalGasIncomeCostProfile => 1 / usdToNok,
                    _ => 1.0
                };
            default:
                throw new Exception("Missing handling for currency dto conversion");
        }
    }

}
