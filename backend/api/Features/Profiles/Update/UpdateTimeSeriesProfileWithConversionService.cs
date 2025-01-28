using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.ModelMapping;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Update;

public class UpdateTimeSeriesProfileWithConversionService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task<TimeSeriesCostDto> UpdateTimeSeriesProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        string profileType,
        UpdateTimeSeriesCostDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var projectPhysicalUnit = await context.Projects
            .Where(x => x.Id == projectPk)
            .Select(x => x.PhysicalUnit)
            .SingleAsync();

        var entity = await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => x.Id == costProfileId)
            .Where(x => x.ProfileType == profileType)
            .SingleAsync();

        entity.StartYear = dto.StartYear;
        entity.Values = UnitConversionHelpers.ConvertValuesFromDto(dto.Values, projectPhysicalUnit, profileType);

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
        string profileType,
        UpdateTimeSeriesCostOverrideDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var projectPhysicalUnit = await context.Projects
            .Where(x => x.Id == projectPk)
            .Select(x => x.PhysicalUnit)
            .SingleAsync();

        var entity = await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => x.Id == costProfileId)
            .Where(x => x.ProfileType == profileType)
            .SingleAsync();

        entity.StartYear = dto.StartYear;
        entity.Override = dto.Override;
        entity.Values = UnitConversionHelpers.ConvertValuesFromDto(dto.Values, projectPhysicalUnit, profileType);

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
