using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.OnshorePowerSupplies.OnshorePowerSupplyCostProfiles;

public class OnshorePowerSupplyCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IRecalculationService recalculationService)
{
    public async Task AddOrUpdateOnshorePowerSupplyCostProfile(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        UpdateTimeSeriesCostDto dto)
    {
        var onshorePowerSupply = await context.OnshorePowerSupplies
            .Include(t => t.CostProfile)
            .SingleAsync(t => t.Id == onshorePowerSupplyId);

        if (onshorePowerSupply.CostProfile != null)
        {
            await UpdateOnshorePowerSupplyTimeSeries(projectId, caseId, onshorePowerSupplyId, onshorePowerSupply.CostProfile.Id, dto);
            return;
        }

        await CreateOnshorePowerSupplyCostProfile(caseId, onshorePowerSupplyId, dto, onshorePowerSupply);
    }

    private async Task CreateOnshorePowerSupplyCostProfile(
        Guid caseId,
        Guid onshorePowerSupplyId,
        UpdateTimeSeriesCostDto dto,
        OnshorePowerSupply onshorePowerSupply)
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
    }

    private async Task UpdateOnshorePowerSupplyTimeSeries(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        Guid profileId,
        UpdateTimeSeriesCostDto updatedProfileDto)
    {
        var existingProfile = await context.OnshorePowerSupplyCostProfile
            .Include(x => x.OnshorePowerSupply).ThenInclude(onshorePowerSupply => onshorePowerSupply.CostProfileOverride)
            .SingleAsync(x => x.OnshorePowerSupply.ProjectId == projectId && x.Id == profileId);

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
    }
}
