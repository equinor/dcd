using api.Context;
using api.Context.Extensions;
using api.Features.Profiles.Dtos;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Update;

public class UpdateTimeSeriesProfileService(DcdDbContext context)
{
    public async Task<TimeSeriesCostDto> UpdateTimeSeriesProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        string profileType,
        UpdateTimeSeriesCostDto dto)
    {
        var entity = await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Case.ProjectId == projectId)
            .Where(x => x.Id == costProfileId)
            .Where(x => x.ProfileType == profileType)
            .SingleAsync();

        entity.StartYear = dto.StartYear;
        entity.Values = dto.Values;

        await context.UpdateCaseUpdatedUtc(caseId);
        await context.SaveChangesAsync();

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
        string profileType,
        UpdateTimeSeriesCostOverrideDto dto)
    {
        var entity = await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Case.ProjectId == projectId)
            .Where(x => x.Id == costProfileId)
            .Where(x => x.ProfileType == profileType)
            .SingleAsync();

        entity.StartYear = dto.StartYear;
        entity.Values = dto.Values;
        entity.Override = dto.Override;

        await context.UpdateCaseUpdatedUtc(caseId);
        await context.SaveChangesAsync();

        return new TimeSeriesCostOverrideDto
        {
            Id = entity.Id,
            StartYear = entity.StartYear,
            Values = entity.Values,
            Override = entity.Override
        };
    }
}
