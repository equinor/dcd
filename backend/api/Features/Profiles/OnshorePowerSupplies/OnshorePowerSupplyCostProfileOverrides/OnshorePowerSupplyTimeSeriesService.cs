using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.OnshorePowerSupplies.OnshorePowerSupplyCostProfileOverrides;

public class OnshorePowerSupplyTimeSeriesService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<TimeSeriesCostOverrideDto> CreateOnshorePowerSupplyCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        CreateTimeSeriesCostOverrideDto dto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<OnshorePowerSupply>(projectId, onshorePowerSupplyId);

        var onshorePowerSupply = await context.OnshorePowerSupplies.SingleAsync(x => x.Id == onshorePowerSupplyId);

        var resourceHasProfile = await context.OnshorePowerSupplies.AnyAsync(t => t.Id == onshorePowerSupplyId && t.CostProfileOverride != null);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"OnshorePowerSupply with id {onshorePowerSupplyId} already has a profile of type {nameof(OnshorePowerSupplyCostProfileOverride)}.");
        }

        OnshorePowerSupplyCostProfileOverride profile = new()
        {
            OnshorePowerSupply = onshorePowerSupply
        };

        var newProfile = mapperService.MapToEntity(dto, profile, onshorePowerSupplyId);

        context.OnshorePowerSupplyCostProfileOverride.Add(newProfile);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<OnshorePowerSupplyCostProfileOverride, TimeSeriesCostOverrideDto>(newProfile, newProfile.Id);
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateOnshorePowerSupplyCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        Guid costProfileId,
        UpdateTimeSeriesCostOverrideDto dto)
    {
        return await UpdateOnshorePowerSupplyTimeSeries<OnshorePowerSupplyCostProfileOverride, TimeSeriesCostOverrideDto, UpdateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            onshorePowerSupplyId,
            costProfileId,
            dto,
            id => context.OnshorePowerSupplyCostProfileOverride.Include(x => x.OnshorePowerSupply).SingleAsync(x => x.Id == id));
    }


    private async Task<TDto> UpdateOnshorePowerSupplyTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile>> getProfile)
        where TProfile : class, IOnshorePowerSupplyTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId);

        await projectIntegrityService.EntityIsConnectedToProject<OnshorePowerSupply>(projectId, existingProfile.OnshorePowerSupply.Id);

        if (existingProfile.OnshorePowerSupply.ProspVersion == null)
        {
            if (existingProfile.OnshorePowerSupply.CostProfileOverride != null)
            {
                existingProfile.OnshorePowerSupply.CostProfileOverride.Override = true;
            }
        }

        mapperService.MapToEntity(updatedProfileDto, existingProfile, onshorePowerSupplyId);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
    }
}
