using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

namespace api.Features.Profiles.Transports;

public abstract class TransportProfileBaseService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    protected DcdDbContext Context => context;
    protected IMapperService MapperService => mapperService;
    protected IProjectIntegrityService ProjectIntegrityService => projectIntegrityService;
    protected IRecalculationService RecalculationService => recalculationService;

    protected async Task<TDto> UpdateTransportTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile>> getProfile
    )
        where TProfile : class, ITransportTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId);

        await ProjectIntegrityService.EntityIsConnectedToProject<Transport>(projectId, existingProfile.Transport.Id);

        if (existingProfile.Transport.ProspVersion == null)
        {
            if (existingProfile.Transport.CostProfileOverride != null)
            {
                existingProfile.Transport.CostProfileOverride.Override = true;
            }
        }

        MapperService.MapToEntity(updatedProfileDto, existingProfile, transportId);

        await Context.UpdateCaseModifyTime(caseId);
        await RecalculationService.SaveChangesAndRecalculateAsync(caseId);

        return MapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
    }
}
