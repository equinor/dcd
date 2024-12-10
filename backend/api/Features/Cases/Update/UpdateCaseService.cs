using api.Context;
using api.Context.Extensions;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Update;

public class UpdateCaseService(DcdDbContext context)
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
        existingCase.ReferenceCase = updateCaseDto.ReferenceCase;
        existingCase.Archived = updateCaseDto.Archived;
        existingCase.ArtificialLift = updateCaseDto.ArtificialLift;
        existingCase.ProductionStrategyOverview = updateCaseDto.ProductionStrategyOverview;
        existingCase.ProducerCount = updateCaseDto.ProducerCount;
        existingCase.GasInjectorCount = updateCaseDto.GasInjectorCount;
        existingCase.WaterInjectorCount = updateCaseDto.WaterInjectorCount;
        existingCase.FacilitiesAvailability = updateCaseDto.FacilitiesAvailability;
        existingCase.CapexFactorFeasibilityStudies = updateCaseDto.CapexFactorFeasibilityStudies;
        existingCase.CapexFactorFEEDStudies = updateCaseDto.CapexFactorFEEDStudies;
        existingCase.NPV = updateCaseDto.NPV;
        existingCase.NPVOverride = updateCaseDto.NPVOverride;
        existingCase.BreakEven = updateCaseDto.BreakEven;
        existingCase.BreakEvenOverride = updateCaseDto.BreakEvenOverride;
        existingCase.Host = updateCaseDto.Host;
        existingCase.DGADate = updateCaseDto.DGADate;
        existingCase.DGBDate = updateCaseDto.DGBDate;
        existingCase.DGCDate = updateCaseDto.DGCDate;
        existingCase.APBODate = updateCaseDto.APBODate;
        existingCase.BORDate = updateCaseDto.BORDate;
        existingCase.VPBODate = updateCaseDto.VPBODate;
        existingCase.DG0Date = updateCaseDto.DG0Date;
        existingCase.DG1Date = updateCaseDto.DG1Date;
        existingCase.DG2Date = updateCaseDto.DG2Date;
        existingCase.DG3Date = updateCaseDto.DG3Date;
        existingCase.DG4Date = updateCaseDto.DG4Date;
        existingCase.SharepointFileId = updateCaseDto.SharepointFileId;
        existingCase.SharepointFileName = updateCaseDto.SharepointFileName;
        existingCase.SharepointFileUrl = updateCaseDto.SharepointFileUrl;
        existingCase.ModifyTime = DateTimeOffset.UtcNow;

        await context.SaveChangesAndRecalculateAsync(caseId);
    }
}
