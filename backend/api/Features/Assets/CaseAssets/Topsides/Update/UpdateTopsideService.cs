using api.Context;
using api.Context.Extensions;
using api.Features.Assets.CaseAssets.Topsides.Profiles.Dtos;
using api.Features.Cases.Recalculation;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Topsides.Update;

public class UpdateTopsideService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task ResetTopside(Guid projectId, Guid caseId, Guid topsideId)
    {
        var existingTopside = await context.Topsides.SingleAsync(x => x.ProjectId == projectId && x.Id == topsideId);

        existingTopside.DryWeight = 0;
        existingTopside.OilCapacity = 0;
        existingTopside.GasCapacity = 0;
        existingTopside.WaterInjectionCapacity = 0;
        existingTopside.ArtificialLift = ArtificialLift.NoArtificialLift;
        existingTopside.Currency = Currency.NOK;
        existingTopside.FuelConsumption = 0;
        existingTopside.FlaredGas = 0;
        existingTopside.ProducerCount = 0;
        existingTopside.GasInjectorCount = 0;
        existingTopside.WaterInjectorCount = 0;
        existingTopside.CO2ShareOilProfile = 0;
        existingTopside.CO2ShareGasProfile = 0;
        existingTopside.CO2ShareWaterInjectionProfile = 0;
        existingTopside.CO2OnMaxOilProfile = 0;
        existingTopside.CO2OnMaxGasProfile = 0;
        existingTopside.CO2OnMaxWaterInjectionProfile = 0;
        existingTopside.CostYear = 0;
        existingTopside.DG3Date = null;
        existingTopside.DG4Date = null;
        existingTopside.FacilityOpex = 0;
        existingTopside.PeakElectricityImported = 0;
        existingTopside.ProspVersion = null;
        existingTopside.Source = Source.ConceptApp;
        existingTopside.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task UpdateTopsideFromProsp(Guid projectId, Guid caseId, Guid topsideId, ProspUpdateTopsideDto dto)
    {
        var existingTopside = await context.Topsides.SingleAsync(x => x.ProjectId == projectId && x.Id == topsideId);

        existingTopside.DryWeight = dto.DryWeight;
        existingTopside.OilCapacity = dto.OilCapacity;
        existingTopside.GasCapacity = dto.GasCapacity;
        existingTopside.WaterInjectionCapacity = dto.WaterInjectionCapacity;
        existingTopside.ArtificialLift = dto.ArtificialLift;
        existingTopside.Currency = dto.Currency;
        existingTopside.FuelConsumption = dto.FuelConsumption;
        existingTopside.FlaredGas = dto.FlaredGas;
        existingTopside.ProducerCount = dto.ProducerCount;
        existingTopside.GasInjectorCount = dto.GasInjectorCount;
        existingTopside.WaterInjectorCount = dto.WaterInjectorCount;
        existingTopside.CO2ShareOilProfile = dto.CO2ShareOilProfile;
        existingTopside.CO2ShareGasProfile = dto.CO2ShareGasProfile;
        existingTopside.CO2ShareWaterInjectionProfile = dto.CO2ShareWaterInjectionProfile;
        existingTopside.CO2OnMaxOilProfile = dto.CO2OnMaxOilProfile;
        existingTopside.CO2OnMaxGasProfile = dto.CO2OnMaxGasProfile;
        existingTopside.CO2OnMaxWaterInjectionProfile = dto.CO2OnMaxWaterInjectionProfile;
        existingTopside.CostYear = dto.CostYear;
        existingTopside.DG3Date = dto.DG3Date;
        existingTopside.DG4Date = dto.DG4Date;
        existingTopside.FacilityOpex = dto.FacilityOpex;
        existingTopside.PeakElectricityImported = dto.PeakElectricityImported;
        existingTopside.ProspVersion = dto.ProspVersion;
        existingTopside.Source = dto.Source;
        existingTopside.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task<TopsideDto> UpdateTopside(Guid projectId, Guid caseId, Guid topsideId, UpdateTopsideDto dto)
    {
        var existingTopside = await context.Topsides.SingleAsync(x => x.ProjectId == projectId && x.Id == topsideId);

        existingTopside.DryWeight = dto.DryWeight;
        existingTopside.OilCapacity = dto.OilCapacity;
        existingTopside.GasCapacity = dto.GasCapacity;
        existingTopside.WaterInjectionCapacity = dto.WaterInjectionCapacity;
        existingTopside.ArtificialLift = dto.ArtificialLift;
        existingTopside.Currency = dto.Currency;
        existingTopside.FuelConsumption = dto.FuelConsumption;
        existingTopside.FlaredGas = dto.FlaredGas;
        existingTopside.ProducerCount = dto.ProducerCount;
        existingTopside.GasInjectorCount = dto.GasInjectorCount;
        existingTopside.WaterInjectorCount = dto.WaterInjectorCount;
        existingTopside.CO2ShareOilProfile = dto.CO2ShareOilProfile;
        existingTopside.CO2ShareGasProfile = dto.CO2ShareGasProfile;
        existingTopside.CO2ShareWaterInjectionProfile = dto.CO2ShareWaterInjectionProfile;
        existingTopside.CO2OnMaxOilProfile = dto.CO2OnMaxOilProfile;
        existingTopside.CO2OnMaxGasProfile = dto.CO2OnMaxGasProfile;
        existingTopside.CO2OnMaxWaterInjectionProfile = dto.CO2OnMaxWaterInjectionProfile;
        existingTopside.CostYear = dto.CostYear;
        existingTopside.DG3Date = dto.DG3Date;
        existingTopside.DG4Date = dto.DG4Date;
        existingTopside.FacilityOpex = dto.FacilityOpex;
        existingTopside.PeakElectricityImported = dto.PeakElectricityImported;
        existingTopside.Source = dto.Source;
        existingTopside.Maturity = dto.Maturity;
        existingTopside.ApprovedBy = dto.ApprovedBy;
        existingTopside.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return new TopsideDto
        {
            Id = existingTopside.Id,
            Name = existingTopside.Name,
            ProjectId = existingTopside.ProjectId,
            DryWeight = existingTopside.DryWeight,
            OilCapacity = existingTopside.OilCapacity,
            GasCapacity = existingTopside.GasCapacity,
            WaterInjectionCapacity = existingTopside.WaterInjectionCapacity,
            ArtificialLift = existingTopside.ArtificialLift,
            Maturity = existingTopside.Maturity,
            Currency = existingTopside.Currency,
            FuelConsumption = existingTopside.FuelConsumption,
            FlaredGas = existingTopside.FlaredGas,
            ProducerCount = existingTopside.ProducerCount,
            GasInjectorCount = existingTopside.GasInjectorCount,
            WaterInjectorCount = existingTopside.WaterInjectorCount,
            CO2ShareOilProfile = existingTopside.CO2ShareOilProfile,
            CO2ShareGasProfile = existingTopside.CO2ShareGasProfile,
            CO2ShareWaterInjectionProfile = existingTopside.CO2ShareWaterInjectionProfile,
            CO2OnMaxOilProfile = existingTopside.CO2OnMaxOilProfile,
            CO2OnMaxGasProfile = existingTopside.CO2OnMaxGasProfile,
            CO2OnMaxWaterInjectionProfile = existingTopside.CO2OnMaxWaterInjectionProfile,
            CostYear = existingTopside.CostYear,
            ProspVersion = existingTopside.ProspVersion,
            LastChangedDate = existingTopside.LastChangedDate,
            Source = existingTopside.Source,
            ApprovedBy = existingTopside.ApprovedBy,
            DG3Date = existingTopside.DG3Date,
            DG4Date = existingTopside.DG4Date,
            FacilityOpex = existingTopside.FacilityOpex,
            PeakElectricityImported = existingTopside.PeakElectricityImported
        };
    }
}
