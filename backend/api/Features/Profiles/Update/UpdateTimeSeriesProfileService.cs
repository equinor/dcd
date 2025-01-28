using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.ModelMapping;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Update;

public class UpdateTimeSeriesProfileService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task<TimeSeriesCostDto> UpdateTimeSeriesProfile(
        Guid projectId,
        Guid caseId,
        Guid profileId,
        UpdateTimeSeriesDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var entity = await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => x.Id == profileId)
            .Where(x => x.ProfileType == dto.ProfileType)
            .SingleAsync();

        entity.StartYear = dto.StartYear;
        entity.Values = await ConvertFromDto(dto.Values, dto.ProfileType, projectPk);

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
        Guid profileId,
        UpdateTimeSeriesOverrideDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var entity = await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => x.Id == profileId)
            .Where(x => x.ProfileType == dto.ProfileType)
            .SingleAsync();

        entity.StartYear = dto.StartYear;
        entity.Values = await ConvertFromDto(dto.Values, dto.ProfileType, projectPk);
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

    private async Task<double[]> ConvertFromDto(double[] input, string profileType, Guid projectPk)
    {
        if (!UnitConversionHelpers.ProfileTypesWithConversion.Contains(profileType))
        {
            return input;
        }

        var projectPhysicalUnit = await context.Projects
            .Where(x => x.Id == projectPk)
            .Select(x => x.PhysicalUnit)
            .SingleAsync();

        return UnitConversionHelpers.ConvertValuesFromDto(input, projectPhysicalUnit, profileType);
    }
}
