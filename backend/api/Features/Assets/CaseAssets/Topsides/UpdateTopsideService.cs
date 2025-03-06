using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Topsides;

public class UpdateTopsideService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateTopside(Guid projectId, Guid caseId, UpdateTopsideDto updatedTopsideDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existing = await context.Topsides.SingleAsync(x => x.Case.ProjectId == projectPk && x.CaseId == caseId);

        existing.DryWeight = updatedTopsideDto.DryWeight;
        existing.OilCapacity = updatedTopsideDto.OilCapacity;
        existing.GasCapacity = updatedTopsideDto.GasCapacity;
        existing.WaterInjectionCapacity = updatedTopsideDto.WaterInjectionCapacity;
        existing.ArtificialLift = updatedTopsideDto.ArtificialLift;
        existing.FuelConsumption = updatedTopsideDto.FuelConsumption;
        existing.FlaredGas = updatedTopsideDto.FlaredGas;
        existing.ProducerCount = updatedTopsideDto.ProducerCount;
        existing.GasInjectorCount = updatedTopsideDto.GasInjectorCount;
        existing.WaterInjectorCount = updatedTopsideDto.WaterInjectorCount;
        existing.CO2ShareOilProfile = updatedTopsideDto.CO2ShareOilProfile;
        existing.CO2ShareGasProfile = updatedTopsideDto.CO2ShareGasProfile;
        existing.CO2ShareWaterInjectionProfile = updatedTopsideDto.CO2ShareWaterInjectionProfile;
        existing.CO2OnMaxOilProfile = updatedTopsideDto.CO2OnMaxOilProfile;
        existing.CO2OnMaxGasProfile = updatedTopsideDto.CO2OnMaxGasProfile;
        existing.CO2OnMaxWaterInjectionProfile = updatedTopsideDto.CO2OnMaxWaterInjectionProfile;
        existing.CostYear = updatedTopsideDto.CostYear;
        existing.FacilityOpex = updatedTopsideDto.FacilityOpex;
        existing.PeakElectricityImported = updatedTopsideDto.PeakElectricityImported;
        existing.Source = updatedTopsideDto.Source;
        existing.Maturity = updatedTopsideDto.Maturity;
        existing.ApprovedBy = updatedTopsideDto.ApprovedBy;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
