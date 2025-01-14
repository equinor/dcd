using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Create;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Update;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Services;

public class OnshorePowerSupplyTimeSeriesService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<OnshorePowerSupplyCostProfileOverrideDto> CreateOnshorePowerSupplyCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        CreateOnshorePowerSupplyCostProfileOverrideDto dto
    )
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

        return mapperService.MapToDto<OnshorePowerSupplyCostProfileOverride, OnshorePowerSupplyCostProfileOverrideDto>(newProfile, newProfile.Id);
    }

    public async Task<OnshorePowerSupplyCostProfileOverrideDto> UpdateOnshorePowerSupplyCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        Guid costProfileId,
        UpdateOnshorePowerSupplyCostProfileOverrideDto dto)
    {
        return await UpdateOnshorePowerSupplyTimeSeries<OnshorePowerSupplyCostProfileOverride, OnshorePowerSupplyCostProfileOverrideDto, UpdateOnshorePowerSupplyCostProfileOverrideDto>(
            projectId,
            caseId,
            onshorePowerSupplyId,
            costProfileId,
            dto,
            id => context.OnshorePowerSupplyCostProfileOverride.Include(x => x.OnshorePowerSupply).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<OnshorePowerSupplyCostProfileDto> AddOrUpdateOnshorePowerSupplyCostProfile(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        UpdateOnshorePowerSupplyCostProfileDto dto
    )
    {
        var onshorePowerSupply = await context.OnshorePowerSupplies
            .Include(t => t.CostProfile)
            .SingleAsync(t => t.Id == onshorePowerSupplyId);

        if (onshorePowerSupply.CostProfile != null)
        {
            return await UpdateOnshorePowerSupplyCostProfile(projectId, caseId, onshorePowerSupplyId, onshorePowerSupply.CostProfile.Id, dto);
        }

        return await CreateOnshorePowerSupplyCostProfile(caseId, onshorePowerSupplyId, dto, onshorePowerSupply);
    }

    private async Task<OnshorePowerSupplyCostProfileDto> UpdateOnshorePowerSupplyCostProfile(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        Guid profileId,
        UpdateOnshorePowerSupplyCostProfileDto dto
    )
    {
        return await UpdateOnshorePowerSupplyTimeSeries<OnshorePowerSupplyCostProfile, OnshorePowerSupplyCostProfileDto, UpdateOnshorePowerSupplyCostProfileDto>(
            projectId,
            caseId,
            onshorePowerSupplyId,
            profileId,
            dto,
            id => context.OnshorePowerSupplyCostProfile.Include(x => x.OnshorePowerSupply).SingleAsync(x => x.Id == id)
        );
    }

    private async Task<OnshorePowerSupplyCostProfileDto> CreateOnshorePowerSupplyCostProfile(
        Guid caseId,
        Guid onshorePowerSupplyId,
        UpdateOnshorePowerSupplyCostProfileDto dto,
        OnshorePowerSupply onshorePowerSupply
    )
    {
        var onshorePowerSupplyCostProfile = new OnshorePowerSupplyCostProfile
        {
            OnshorePowerSupply = onshorePowerSupply
        };

        var newProfile = mapperService.MapToEntity(dto, onshorePowerSupplyCostProfile, onshorePowerSupplyId);

        if (newProfile.OnshorePowerSupply.CostProfileOverride != null)
        {
            newProfile.OnshorePowerSupply.CostProfileOverride.Override = false;
        }

        context.OnshorePowerSupplyCostProfile.Add(newProfile);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<OnshorePowerSupplyCostProfile, OnshorePowerSupplyCostProfileDto>(newProfile, newProfile.Id);
    }

    private async Task<TDto> UpdateOnshorePowerSupplyTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile>> getProfile
    )
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
