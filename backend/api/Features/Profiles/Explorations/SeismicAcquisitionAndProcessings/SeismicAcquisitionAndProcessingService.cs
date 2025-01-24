using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Explorations.SeismicAcquisitionAndProcessings;

public class SeismicAcquisitionAndProcessingService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService) : ExplorationProfileBaseService(context, mapperService, projectIntegrityService, recalculationService)
{
    public async Task<TimeSeriesCostDto> CreateSeismicAcquisitionAndProcessing(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        CreateTimeSeriesCostDto createProfileDto)
    {
        return await CreateExplorationProfile<SeismicAcquisitionAndProcessing, TimeSeriesCostDto, CreateTimeSeriesCostDto>(
            projectId,
            caseId,
            explorationId,
            createProfileDto,
            d => d.SeismicAcquisitionAndProcessing != null);
    }

    public async Task<TimeSeriesCostDto> UpdateSeismicAcquisitionAndProcessing(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateTimeSeriesCostDto updateDto)
    {
        return await UpdateExplorationCostProfile<SeismicAcquisitionAndProcessing, TimeSeriesCostDto, UpdateTimeSeriesCostDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            id => Context.SeismicAcquisitionAndProcessing.Include(x => x.Exploration).SingleAsync(x => x.Id == id));
    }
}
