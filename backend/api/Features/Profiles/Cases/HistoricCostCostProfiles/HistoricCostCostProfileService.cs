using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
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
    public async Task<TimeSeriesCostDto> CreateHistoricCostCostProfile(
        Guid projectId,
        Guid caseId,
        CreateTimeSeriesCostDto createProfileDto)
    {
        return await CreateCaseProfile<HistoricCostCostProfile, TimeSeriesCostDto, CreateTimeSeriesCostDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.HistoricCostCostProfile != null);
    }

    public async Task<TimeSeriesCostDto> UpdateHistoricCostCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTimeSeriesCostDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<HistoricCostCostProfile, TimeSeriesCostDto, UpdateTimeSeriesCostDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.HistoricCostCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
