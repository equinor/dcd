using System.Linq.Expressions;

using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class CaseService(
    DcdDbContext context,
    IProjectService projectService,
    ILogger<CaseService> logger,
    ICaseRepository repository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService)
    : ICaseService
{
    public async Task<ProjectWithAssetsDto> DeleteCase(Guid projectId, Guid caseId)
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Case>(projectId, caseId);

        var caseItem = await GetCase(caseId);

        context.Cases!.Remove(caseItem);

        await context.SaveChangesAsync();

        return await projectService.GetProjectDto(caseItem.ProjectId);
    }

    public async Task<Case> GetCase(Guid caseId)
    {
        var caseItem = await context.Cases!
            .Include(c => c.TotalFeasibilityAndConceptStudies)
            .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(c => c.TotalFEEDStudies)
            .Include(c => c.TotalFEEDStudiesOverride)
            .Include(c => c.TotalOtherStudiesCostProfile)
            .Include(c => c.HistoricCostCostProfile)
            .Include(c => c.WellInterventionCostProfile)
            .Include(c => c.WellInterventionCostProfileOverride)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfile)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
            .Include(c => c.OnshoreRelatedOPEXCostProfile)
            .Include(c => c.AdditionalOPEXCostProfile)
            .Include(c => c.CessationWellsCost)
            .Include(c => c.CessationWellsCostOverride)
            .Include(c => c.CessationOffshoreFacilitiesCost)
            .Include(c => c.CessationOffshoreFacilitiesCostOverride)
            .Include(c => c.CessationOnshoreFacilitiesCostProfile)
            .Include(c => c.CalculatedTotalIncomeCostProfile)
            .Include(c => c.CalculatedTotalCostCostProfile)
            .FirstOrDefaultAsync(c => c.Id == caseId)
        ?? throw new NotFoundInDBException($"Case {caseId} not found.");

        return caseItem;
    }

    public async Task<Case> GetCaseWithIncludes(Guid caseId, params Expression<Func<Case, object>>[] includes)
    {
        return await repository.GetCaseWithIncludes(caseId, includes)
            ?? throw new NotFoundInDBException($"Case with id {caseId} not found.");
    }

    // TODO: Delete this method
    public async Task<IEnumerable<Case>> GetAll()
    {
        return await context.Cases.ToListAsync();
    }

    public async Task<CaseDto> UpdateCase<TDto>(
        Guid projectId,
        Guid caseId,
        TDto updatedCaseDto
    )
        where TDto : BaseUpdateCaseDto
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Case>(projectId, caseId);

        var existingCase = await repository.GetCase(caseId)
            ?? throw new NotFoundInDBException($"Case with id {caseId} not found.");

        mapperService.MapToEntity(updatedCaseDto, existingCase, caseId);

        existingCase.ModifyTime = DateTimeOffset.UtcNow;

        try
        {
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to update case with id {caseId}.", caseId);
            throw;
        }

        await repository.UpdateModifyTime(caseId);
        var dto = mapperService.MapToDto<Case, CaseDto>(existingCase, caseId);
        return dto;
    }
}
