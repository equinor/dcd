using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Update;

public class UpdateCaseService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateCase(Guid projectId, Guid caseId, UpdateCaseDto updateCaseDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existingCase = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        existingCase.Name = updateCaseDto.Name;
        existingCase.Description = updateCaseDto.Description;
        existingCase.Archived = updateCaseDto.Archived;
        existingCase.ArtificialLift = updateCaseDto.ArtificialLift;
        existingCase.ProductionStrategyOverview = updateCaseDto.ProductionStrategyOverview;
        existingCase.ProducerCount = updateCaseDto.ProducerCount;
        existingCase.GasInjectorCount = updateCaseDto.GasInjectorCount;
        existingCase.WaterInjectorCount = updateCaseDto.WaterInjectorCount;
        existingCase.FacilitiesAvailability = updateCaseDto.FacilitiesAvailability;
        existingCase.CapexFactorFeasibilityStudies = updateCaseDto.CapexFactorFeasibilityStudies / 100;
        existingCase.CapexFactorFeedStudies = updateCaseDto.CapexFactorFeedStudies / 100;
        existingCase.InitialYearsWithoutWellInterventionCost = updateCaseDto.InitialYearsWithoutWellInterventionCost;
        existingCase.FinalYearsWithoutWellInterventionCost = updateCaseDto.FinalYearsWithoutWellInterventionCost;
        existingCase.Npv = updateCaseDto.Npv;
        existingCase.NpvOverride = updateCaseDto.NpvOverride;
        existingCase.BreakEven = updateCaseDto.BreakEven;
        existingCase.BreakEvenOverride = updateCaseDto.BreakEvenOverride;
        existingCase.Host = updateCaseDto.Host;
        existingCase.AverageCo2Intensity = updateCaseDto.AverageCo2Intensity;
        existingCase.DiscountedCashflow = updateCaseDto.DiscountedCashflow;
        existingCase.DgaDate = updateCaseDto.DgaDate;
        existingCase.DgbDate = updateCaseDto.DgbDate;
        existingCase.DgcDate = updateCaseDto.DgcDate;
        existingCase.ApboDate = updateCaseDto.ApboDate;
        existingCase.BorDate = updateCaseDto.BorDate;
        existingCase.VpboDate = updateCaseDto.VpboDate;
        existingCase.Dg0Date = updateCaseDto.Dg0Date;
        existingCase.Dg1Date = updateCaseDto.Dg1Date;
        existingCase.Dg2Date = updateCaseDto.Dg2Date;
        existingCase.Dg3Date = updateCaseDto.Dg3Date;
        existingCase.Dg4Date = updateCaseDto.Dg4Date;
        existingCase.SharepointFileId = updateCaseDto.SharepointFileId;
        existingCase.SharepointFileName = updateCaseDto.SharepointFileName;
        existingCase.SharepointFileUrl = updateCaseDto.SharepointFileUrl;

        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
