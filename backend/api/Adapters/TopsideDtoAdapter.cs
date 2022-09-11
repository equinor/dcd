using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class TopsideDtoAdapter
{
    public static TopsideDto Convert(Topside topside)
    {
        var topsideDto = new TopsideDto
        {
            Id = topside.Id,
            Name = topside.Name,
            ProjectId = topside.ProjectId,
            DryWeight = topside.DryWeight,
            OilCapacity = topside.OilCapacity,
            GasCapacity = topside.GasCapacity,
            FacilitiesAvailability = topside.FacilitiesAvailability,
            ArtificialLift = topside.ArtificialLift,
            Maturity = topside.Maturity,
            Currency = topside.Currency,
            CostProfile = Convert(topside.CostProfile),
            CessationCostProfile = Convert(topside.CessationCostProfile),
            FuelConsumption = topside.FuelConsumption,
            FlaredGas = topside.FlaredGas,
            ProducerCount = topside.ProducerCount,
            GasInjectorCount = topside.GasInjectorCount,
            WaterInjectorCount = topside.WaterInjectorCount,
            CO2ShareOilProfile = topside.CO2ShareOilProfile,
            CO2ShareGasProfile = topside.CO2ShareGasProfile,
            CO2ShareWaterInjectionProfile = topside.CO2ShareWaterInjectionProfile,
            CO2OnMaxOilProfile = topside.CO2OnMaxOilProfile,
            CO2OnMaxGasProfile = topside.CO2OnMaxGasProfile,
            CO2OnMaxWaterInjectionProfile = topside.CO2OnMaxWaterInjectionProfile,
            CostYear = topside.CostYear,
            ProspVersion = topside.ProspVersion,
            LastChangedDate = topside.LastChangedDate,
            Source = topside.Source,
            ApprovedBy = topside.ApprovedBy,
            DG3Date = topside.DG3Date,
            DG4Date = topside.DG4Date,
            FacilityOpex = topside.FacilityOpex
        };
        return topsideDto;
    }

    private static TopsideCostProfileDto? Convert(TopsideCostProfile? costProfile)
    {
        if (costProfile == null)
        {
            return null;
        }

        var topsideCostProfile = new TopsideCostProfileDto
        {
            Id = costProfile.Id,
            Currency = costProfile.Currency,
            EPAVersion = costProfile.EPAVersion,
            Values = costProfile.Values,
            StartYear = costProfile.StartYear
        };
        return topsideCostProfile;
    }

    private static TopsideCessationCostProfileDto? Convert(TopsideCessationCostProfile? topsideCessationCostProfile)
    {
        if (topsideCessationCostProfile == null)
        {
            return null;
        }

        var topsideCostProfile = new TopsideCessationCostProfileDto
        {
            Id = topsideCessationCostProfile.Id,
            Currency = topsideCessationCostProfile.Currency,
            EPAVersion = topsideCessationCostProfile.EPAVersion,
            Values = topsideCessationCostProfile.Values,
            StartYear = topsideCessationCostProfile.StartYear
        };
        return topsideCostProfile;
    }
}
