using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Cases.HistoricCostCostProfiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Cases.HistoricCostCostProfiles;

public class HistoricCostCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : CaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<HistoricCostCostProfileDto> CreateHistoricCostCostProfile(
        Guid projectId,
        Guid caseId,
        CreateHistoricCostCostProfileDto createProfileDto)
    {
        return await CreateCaseProfile<HistoricCostCostProfile, HistoricCostCostProfileDto, CreateHistoricCostCostProfileDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.HistoricCostCostProfile != null);
    }

    public async Task<HistoricCostCostProfileDto> UpdateHistoricCostCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateHistoricCostCostProfileDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<HistoricCostCostProfile, HistoricCostCostProfileDto, UpdateHistoricCostCostProfileDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.HistoricCostCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
