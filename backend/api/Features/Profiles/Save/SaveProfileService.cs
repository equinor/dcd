using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Save;

public class SaveProfileService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task<TimeSeriesCostDto> SaveTimeSeriesProfile(Guid projectId, Guid caseId, SaveTimeSeriesDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existingEntity = await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => x.ProfileType == dto.ProfileType)
            .SingleOrDefaultAsync();

        if (existingEntity != null)
        {
            existingEntity.StartYear = dto.StartYear;
            existingEntity.Values = await ConvertFromDto(dto.Values, dto.ProfileType, projectPk);

            await context.UpdateCaseUpdatedUtc(caseId);
            await recalculationService.SaveChangesAndRecalculateCase(caseId);

            return new TimeSeriesCostDto
            {
                Id = existingEntity.Id,
                StartYear = existingEntity.StartYear,
                Values = dto.Values
            };
        }

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        var newEntity = new TimeSeriesProfile
        {
            ProfileType = dto.ProfileType,
            CaseId = caseItem.Id,
            StartYear = dto.StartYear,
            Values = await ConvertFromDto(dto.Values, dto.ProfileType, projectPk)
        };

        context.TimeSeriesProfiles.Add(newEntity);

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);

        return new TimeSeriesCostDto
        {
            Id = newEntity.Id,
            StartYear = newEntity.StartYear,
            Values = dto.Values
        };
    }

    public async Task<TimeSeriesCostOverrideDto> SaveTimeSeriesOverrideProfile(Guid projectId, Guid caseId, SaveTimeSeriesOverrideDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existingEntity = await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => x.ProfileType == dto.ProfileType)
            .SingleOrDefaultAsync();

        if (existingEntity != null)
        {
            existingEntity.StartYear = dto.StartYear;
            existingEntity.Values = await ConvertFromDto(dto.Values, dto.ProfileType, projectPk);
            existingEntity.Override = dto.Override;

            await context.UpdateCaseUpdatedUtc(caseId);
            await recalculationService.SaveChangesAndRecalculateCase(caseId);

            return new TimeSeriesCostOverrideDto
            {
                Id = existingEntity.Id,
                StartYear = existingEntity.StartYear,
                Values = dto.Values,
                Override = existingEntity.Override
            };
        }

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        var newEntity = new TimeSeriesProfile
        {
            ProfileType = dto.ProfileType,
            CaseId = caseItem.Id,
            StartYear = dto.StartYear,
            Values = await ConvertFromDto(dto.Values, dto.ProfileType, projectPk),
            Override = dto.Override
        };

        context.TimeSeriesProfiles.Add(newEntity);

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);

        return new TimeSeriesCostOverrideDto
        {
            Id = newEntity.Id,
            StartYear = newEntity.StartYear,
            Values = dto.Values,
            Override = newEntity.Override
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
