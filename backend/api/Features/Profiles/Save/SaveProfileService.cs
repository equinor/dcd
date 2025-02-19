using api.Context;
using api.Context.Extensions;
using api.Features.Cases.GetWithAssets.Dtos;
using api.Features.Cases.Recalculation;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Save;

public class SaveProfileService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task<TimeSeriesDto> SaveTimeSeriesProfile(Guid projectId, Guid caseId, SaveTimeSeriesDto dto)
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

            return new TimeSeriesDto
            {
                StartYear = existingEntity.StartYear,
                Values = dto.Values,
                UpdatedUtc = existingEntity.UpdatedUtc
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

        return new TimeSeriesDto
        {
            StartYear = newEntity.StartYear,
            Values = dto.Values,
            UpdatedUtc = newEntity.UpdatedUtc
        };
    }

    public async Task<TimeSeriesOverrideDto> SaveTimeSeriesOverrideProfile(Guid projectId, Guid caseId, SaveTimeSeriesOverrideDto dto)
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

            return new TimeSeriesOverrideDto
            {
                StartYear = existingEntity.StartYear,
                Values = dto.Values,
                Override = existingEntity.Override,
                UpdatedUtc = existingEntity.UpdatedUtc
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

        return new TimeSeriesOverrideDto
        {
            StartYear = newEntity.StartYear,
            Values = dto.Values,
            Override = newEntity.Override,
            UpdatedUtc = newEntity.UpdatedUtc
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
