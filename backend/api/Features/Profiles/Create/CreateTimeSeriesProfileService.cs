using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Create;

public class CreateTimeSeriesProfileService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task<TimeSeriesCostDto> CreateTimeSeriesProfile(
        Guid projectId,
        Guid caseId,
        CreateTimeSeriesDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        var entity = new TimeSeriesProfile
        {
            CaseId = caseItem.Id,
            ProfileType = dto.ProfileType,
            StartYear = dto.StartYear,
            Values = await ConvertFromDto(dto.Values, dto.ProfileType, projectPk)
        };

        context.TimeSeriesProfiles.Add(entity);

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return new TimeSeriesCostDto
        {
            Id = entity.Id,
            StartYear = entity.StartYear,
            Values = dto.Values
        };
    }

    public async Task<TimeSeriesCostOverrideDto> CreateTimeSeriesOverrideProfile(
        Guid projectId,
        Guid caseId,
        CreateTimeSeriesOverrideDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        var entity = new TimeSeriesProfile
        {
            CaseId = caseItem.Id,
            ProfileType = dto.ProfileType,
            StartYear = dto.StartYear,
            Values = await ConvertFromDto(dto.Values, dto.ProfileType, projectPk),
            Override = dto.Override
        };

        caseItem.TimeSeriesProfiles.Add(entity);

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return new TimeSeriesCostOverrideDto
        {
            Id = entity.Id,
            StartYear = entity.StartYear,
            Values = dto.Values,
            Override = entity.Override
        };
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
