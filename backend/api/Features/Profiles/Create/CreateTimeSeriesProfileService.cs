using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Create;

public class CreateTimeSeriesProfileService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task<TimeSeriesCostDto> CreateTimeSeriesProfile(
        Guid projectId,
        Guid caseId,
        string profileType,
        CreateTimeSeriesCostDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        var entity = new TimeSeriesProfile
        {
            CaseId = caseItem.Id,
            ProfileType = profileType,
            StartYear = dto.StartYear,
            Values = dto.Values
        };

        context.TimeSeriesProfiles.Add(entity);

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return new TimeSeriesCostDto
        {
            Id = entity.Id,
            StartYear = entity.StartYear,
            Values = entity.Values
        };
    }

    public async Task<TimeSeriesCostOverrideDto> CreateTimeSeriesOverrideProfile(
        Guid projectId,
        Guid caseId,
        string profileType,
        CreateTimeSeriesCostOverrideDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        var entity = new TimeSeriesProfile
        {
            CaseId = caseItem.Id,
            ProfileType = profileType,
            StartYear = dto.StartYear,
            Values = dto.Values,
            Override = dto.Override
        };

        caseItem.TimeSeriesProfiles.Add(entity);

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
