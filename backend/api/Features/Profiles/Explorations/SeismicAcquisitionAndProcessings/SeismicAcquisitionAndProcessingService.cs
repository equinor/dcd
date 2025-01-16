using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Explorations.SeismicAcquisitionAndProcessings.Dtos;
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
    public async Task<SeismicAcquisitionAndProcessingDto> CreateSeismicAcquisitionAndProcessing(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        CreateSeismicAcquisitionAndProcessingDto createProfileDto)
    {
        return await CreateExplorationProfile<SeismicAcquisitionAndProcessing, SeismicAcquisitionAndProcessingDto, CreateSeismicAcquisitionAndProcessingDto>(
            projectId,
            caseId,
            explorationId,
            createProfileDto,
            d => d.SeismicAcquisitionAndProcessing != null);
    }

    public async Task<SeismicAcquisitionAndProcessingDto> UpdateSeismicAcquisitionAndProcessing(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateSeismicAcquisitionAndProcessingDto updateDto)
    {
        return await UpdateExplorationCostProfile<SeismicAcquisitionAndProcessing, SeismicAcquisitionAndProcessingDto, UpdateSeismicAcquisitionAndProcessingDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            id => Context.SeismicAcquisitionAndProcessing.Include(x => x.Exploration).SingleAsync(x => x.Id == id));
    }
}
