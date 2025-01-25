using api.Context;
using api.Context.Extensions;
using api.Features.Profiles.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Create;

public class CreateTimeSeriesProfileService(DcdDbContext context)
{
    public async Task<TimeSeriesCostOverrideDto> CreateTimeSeriesProfile(
        Guid projectId,
        Guid caseId,
        string profileType,
        CreateTimeSeriesCostDto dto)
    {
        var entity = new TimeSeriesProfile
        {
            CaseId = caseId,
            ProfileType = profileType,
            StartYear = dto.StartYear,
            Values = dto.Values
        };

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        caseItem.TimeSeriesProfiles.Add(entity);

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

    public async Task<TimeSeriesCostOverrideDto> CreateTimeSeriesOverrideProfile(
        Guid projectId,
        Guid caseId,
        string profileType,
        CreateTimeSeriesCostOverrideDto dto)
    {
        var entity = new TimeSeriesProfile
        {
            CaseId = caseId,
            ProfileType = profileType,
            StartYear = dto.StartYear,
            Values = dto.Values,
            Override = dto.Override
        };

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        caseItem.TimeSeriesProfiles.Add(entity);

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
