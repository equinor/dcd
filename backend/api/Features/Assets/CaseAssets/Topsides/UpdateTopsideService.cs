using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Topsides;

public class UpdateTopsideService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateTopside(Guid projectId, Guid caseId, UpdateTopsideDto updatedTopsideDto)
    {
        var existingTopside = await context.Topsides.SingleAsync(x => x.Case.ProjectId == projectId && x.CaseId == caseId);

        existingTopside.DryWeight = updatedTopsideDto.DryWeight;
        existingTopside.OilCapacity = updatedTopsideDto.OilCapacity;
        existingTopside.GasCapacity = updatedTopsideDto.GasCapacity;
        existingTopside.WaterInjectionCapacity = updatedTopsideDto.WaterInjectionCapacity;
        existingTopside.ArtificialLift = updatedTopsideDto.ArtificialLift;
        existingTopside.Currency = updatedTopsideDto.Currency;
        existingTopside.FuelConsumption = updatedTopsideDto.FuelConsumption;
        existingTopside.FlaredGas = updatedTopsideDto.FlaredGas;
        existingTopside.ProducerCount = updatedTopsideDto.ProducerCount;
        existingTopside.GasInjectorCount = updatedTopsideDto.GasInjectorCount;
        existingTopside.WaterInjectorCount = updatedTopsideDto.WaterInjectorCount;
        existingTopside.CO2ShareOilProfile = updatedTopsideDto.CO2ShareOilProfile;
        existingTopside.CO2ShareGasProfile = updatedTopsideDto.CO2ShareGasProfile;
        existingTopside.CO2ShareWaterInjectionProfile = updatedTopsideDto.CO2ShareWaterInjectionProfile;
        existingTopside.CO2OnMaxOilProfile = updatedTopsideDto.CO2OnMaxOilProfile;
        existingTopside.CO2OnMaxGasProfile = updatedTopsideDto.CO2OnMaxGasProfile;
        existingTopside.CO2OnMaxWaterInjectionProfile = updatedTopsideDto.CO2OnMaxWaterInjectionProfile;
        existingTopside.CostYear = updatedTopsideDto.CostYear;
        existingTopside.DG3Date = updatedTopsideDto.DG3Date;
        existingTopside.DG4Date = updatedTopsideDto.DG4Date;
        existingTopside.FacilityOpex = updatedTopsideDto.FacilityOpex;
        existingTopside.PeakElectricityImported = updatedTopsideDto.PeakElectricityImported;
        existingTopside.Source = updatedTopsideDto.Source;
        existingTopside.Maturity = updatedTopsideDto.Maturity;
        existingTopside.ApprovedBy = updatedTopsideDto.ApprovedBy;
        existingTopside.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }

    public async Task UpdateTopside(Guid projectId, Guid caseId, ProspUpdateTopsideDto updatedTopsideDto)
    {
        var existingTopside = await context.Topsides.SingleAsync(x => x.Case.ProjectId == projectId && x.CaseId == caseId);

        existingTopside.DryWeight = updatedTopsideDto.DryWeight;
        existingTopside.OilCapacity = updatedTopsideDto.OilCapacity;
        existingTopside.GasCapacity = updatedTopsideDto.GasCapacity;
        existingTopside.WaterInjectionCapacity = updatedTopsideDto.WaterInjectionCapacity;
        existingTopside.ArtificialLift = updatedTopsideDto.ArtificialLift;
        existingTopside.Currency = updatedTopsideDto.Currency;
        existingTopside.FuelConsumption = updatedTopsideDto.FuelConsumption;
        existingTopside.FlaredGas = updatedTopsideDto.FlaredGas;
        existingTopside.ProducerCount = updatedTopsideDto.ProducerCount;
        existingTopside.GasInjectorCount = updatedTopsideDto.GasInjectorCount;
        existingTopside.WaterInjectorCount = updatedTopsideDto.WaterInjectorCount;
        existingTopside.CO2ShareOilProfile = updatedTopsideDto.CO2ShareOilProfile;
        existingTopside.CO2ShareGasProfile = updatedTopsideDto.CO2ShareGasProfile;
        existingTopside.CO2ShareWaterInjectionProfile = updatedTopsideDto.CO2ShareWaterInjectionProfile;
        existingTopside.CO2OnMaxOilProfile = updatedTopsideDto.CO2OnMaxOilProfile;
        existingTopside.CO2OnMaxGasProfile = updatedTopsideDto.CO2OnMaxGasProfile;
        existingTopside.CO2OnMaxWaterInjectionProfile = updatedTopsideDto.CO2OnMaxWaterInjectionProfile;
        existingTopside.CostYear = updatedTopsideDto.CostYear;
        existingTopside.DG3Date = updatedTopsideDto.DG3Date;
        existingTopside.DG4Date = updatedTopsideDto.DG4Date;
        existingTopside.FacilityOpex = updatedTopsideDto.FacilityOpex;
        existingTopside.PeakElectricityImported = updatedTopsideDto.PeakElectricityImported;
        existingTopside.Source = updatedTopsideDto.Source;
        existingTopside.ProspVersion = updatedTopsideDto.ProspVersion;
        existingTopside.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
