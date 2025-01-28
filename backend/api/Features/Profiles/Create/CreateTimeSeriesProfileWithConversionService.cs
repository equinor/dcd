using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Create;

public class CreateTimeSeriesProfileWithConversionService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task<TimeSeriesCostDto> CreateTimeSeriesProfile(
        Guid projectId,
        Guid caseId,
        string profileType,
        CreateTimeSeriesCostDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var projectPhysicalUnit = await context.Projects
            .Where(x => x.Id == projectPk)
            .Select(x => x.PhysicalUnit)
            .SingleAsync();

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        var entity = new TimeSeriesProfile
        {
            CaseId = caseItem.Id,
            ProfileType = profileType,
            StartYear = dto.StartYear,
            Values = UnitConversionHelpers.ConvertValuesFromDto(dto.Values, projectPhysicalUnit, profileType)
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

        var projectPhysicalUnit = await context.Projects
            .Where(x => x.Id == projectPk)
            .Select(x => x.PhysicalUnit)
            .SingleAsync();

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        var entity = new TimeSeriesProfile
        {
            CaseId = caseItem.Id,
            ProfileType = profileType,
            StartYear = dto.StartYear,
            Values = UnitConversionHelpers.ConvertValuesFromDto(dto.Values, projectPhysicalUnit, profileType),
            Override = dto.Override
        };

        context.TimeSeriesProfiles.Add(entity);

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
