using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Update;

public class UpdateTimeSeriesProfileService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task<TimeSeriesCostDto> UpdateTimeSeriesProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTimeSeriesDto dto)
    {
        return await UpdateTimeSeriesProfile(projectId, caseId, costProfileId, dto.ProfileType, new UpdateTimeSeriesCostDto
        {
            StartYear = dto.StartYear,
            Values = dto.Values
        });
    }

    public async Task<TimeSeriesCostDto> UpdateTimeSeriesProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        string profileType,
        UpdateTimeSeriesCostDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var entity = await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => x.Id == costProfileId)
            .Where(x => x.ProfileType == profileType)
            .SingleAsync();

        entity.StartYear = dto.StartYear;
        entity.Values = dto.Values;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return new TimeSeriesCostDto
        {
            Id = entity.Id,
            StartYear = entity.StartYear,
            Values = entity.Values
        };
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateTimeSeriesOverrideProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTimeSeriesOverrideDto dto)
    {
        return await UpdateTimeSeriesOverrideProfile(projectId,
            caseId,
            costProfileId,
            dto.ProfileType,
            new UpdateTimeSeriesCostOverrideDto
            {
                StartYear = dto.StartYear,
                Values = dto.Values,
                Override = dto.Override
            });
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateTimeSeriesOverrideProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        string profileType,
        UpdateTimeSeriesCostOverrideDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var entity = await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => x.Id == costProfileId)
            .Where(x => x.ProfileType == profileType)
            .SingleAsync();

        entity.StartYear = dto.StartYear;
        entity.Values = dto.Values;
        entity.Override = dto.Override;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return new TimeSeriesCostOverrideDto
        {
            Id = entity.Id,
            StartYear = entity.StartYear,
            Values = entity.Values,
            Override = entity.Override
        };
    }
}
