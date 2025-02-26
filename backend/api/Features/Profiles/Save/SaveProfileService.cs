using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Save;

public class SaveProfileService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task SaveTimeSeriesList(Guid projectId, Guid caseId, SaveTimeSeriesListDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var profileTypes = dto.TimeSeries.Select(x => x.ProfileType)
            .Union(dto.OverrideTimeSeries.Select(x => x.ProfileType))
            .ToList();

        var existingEntities = await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => profileTypes.Contains(x.ProfileType))
            .ToListAsync();

        await HandleOrdinaryProfiles(caseId, dto, existingEntities, projectPk);
        await HandleOverrideProfiles(caseId, dto, existingEntities, projectPk);

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }

    private async Task HandleOrdinaryProfiles(Guid caseId, SaveTimeSeriesListDto dto, List<TimeSeriesProfile> existingEntities, Guid projectPk)
    {
        foreach (var timeSeriesDto in dto.TimeSeries)
        {
            var existingEntity = existingEntities.SingleOrDefault(x => x.ProfileType == timeSeriesDto.ProfileType);

            if (existingEntity != null)
            {
                existingEntity.StartYear = timeSeriesDto.StartYear;
                existingEntity.Values = await ConvertFromDto(timeSeriesDto.Values, timeSeriesDto.ProfileType, projectPk);

                continue;
            }

            context.TimeSeriesProfiles.Add(new TimeSeriesProfile
            {
                ProfileType = timeSeriesDto.ProfileType,
                CaseId = caseId,
                StartYear = timeSeriesDto.StartYear,
                Values = await ConvertFromDto(timeSeriesDto.Values, timeSeriesDto.ProfileType, projectPk)
            });
        }
    }

    private async Task HandleOverrideProfiles(Guid caseId, SaveTimeSeriesListDto dto, List<TimeSeriesProfile> existingEntities, Guid projectPk)
    {
        foreach (var timeSeriesDto in dto.OverrideTimeSeries)
        {
            var existingEntity = existingEntities.SingleOrDefault(x => x.ProfileType == timeSeriesDto.ProfileType);

            if (existingEntity != null)
            {
                existingEntity.StartYear = timeSeriesDto.StartYear;
                existingEntity.Values = await ConvertFromDto(timeSeriesDto.Values, timeSeriesDto.ProfileType, projectPk);
                existingEntity.Override = timeSeriesDto.Override;

                continue;
            }

            context.TimeSeriesProfiles.Add(new TimeSeriesProfile
            {
                ProfileType = timeSeriesDto.ProfileType,
                CaseId = caseId,
                StartYear = timeSeriesDto.StartYear,
                Values = await ConvertFromDto(timeSeriesDto.Values, timeSeriesDto.ProfileType, projectPk),
                Override = timeSeriesDto.Override
            });
        }
    }

    private async Task<double[]> ConvertFromDto(double[] dtoValues, string profileType, Guid projectPk)
    {
        if (!UnitConversionHelpers.ProfileTypesWithConversion.Contains(profileType))
        {
            return dtoValues;
        }

        var projectPhysicalUnit = await context.Projects
            .Where(x => x.Id == projectPk)
            .Select(x => x.PhysicalUnit)
            .SingleAsync();

        return UnitConversionHelpers.ConvertValuesFromDto(dtoValues, projectPhysicalUnit, profileType);
    }
}
