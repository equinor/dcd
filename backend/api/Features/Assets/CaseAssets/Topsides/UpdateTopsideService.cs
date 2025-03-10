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
        existing.Co2ShareOilProfile = updatedTopsideDto.Co2ShareOilProfile;
        existing.Co2ShareGasProfile = updatedTopsideDto.Co2ShareGasProfile;
        existing.Co2ShareWaterInjectionProfile = updatedTopsideDto.Co2ShareWaterInjectionProfile;
        existing.Co2OnMaxOilProfile = updatedTopsideDto.Co2OnMaxOilProfile;
        existing.Co2OnMaxGasProfile = updatedTopsideDto.Co2OnMaxGasProfile;
        existing.Co2OnMaxWaterInjectionProfile = updatedTopsideDto.Co2OnMaxWaterInjectionProfile;
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
