using api.Context;
using api.Features.ProjectData.Dtos;
using api.Features.ProjectData.Dtos.AssetDtos;
using api.Features.ProjectMembers.Get;
using api.Features.Cases.GetWithAssets.AssetMappers;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ProjectData;

public class GetProjectDataRepository(DcdDbContext context)
{
    public async Task<Guid> GetOriginalProjectIdForRevision(Guid revisionId)
    {
        return await context.Projects
            .Where(x => x.Id == revisionId)
            .Select(x => x.OriginalProjectId!.Value)
            .SingleAsync();
    }

    public async Task<List<ProjectMemberDto>> GetProjectMembers(Guid projectId)
    {
        return await context.ProjectMembers
            .Where(x => x.ProjectId == projectId)
            .Select(x => new ProjectMemberDto
            {
                ProjectId = x.ProjectId,
                AzureAdUserId = x.AzureAdUserId,
                Role = x.Role,
                IsPmt = x.FromOrgChart
            })
            .ToListAsync();
    }

    public async Task<List<RevisionDetailsDto>> GetRevisionDetailsList(Guid projectId)
    {
        return await context.RevisionDetails
            .Where(r => r.Revision.OriginalProjectId == projectId)
            .Select(x => new RevisionDetailsDto
            {
                RevisionId = x.RevisionId,
                RevisionName = x.RevisionName,
                RevisionDate = x.CreatedUtc,
                Arena = x.Arena,
                Mdqc = x.Mdqc
            })
            .OrderBy(x => x.RevisionDate)
            .ToListAsync();
    }

    public async Task<RevisionDetailsDto> GetRevisionDetails(Guid revisionId)
    {
        return await context.RevisionDetails
            .Where(x => x.RevisionId == revisionId)
            .Select(x => new RevisionDetailsDto
            {
                RevisionId = x.RevisionId,
                RevisionName = x.RevisionName,
                RevisionDate = x.CreatedUtc,
                Arena = x.Arena,
                Mdqc = x.Mdqc
            })
            .SingleAsync();
    }

    public async Task<CommonProjectAndRevisionDto> GetCommonProjectAndRevisionData(Guid projectId)
    {
        return await context.Projects
            .Where(x => x.Id == projectId)
            .Select(x => new CommonProjectAndRevisionDto
            {
                UpdatedUtc = x.UpdatedUtc,
                Classification = x.Classification,
                Name = x.Name,
                FusionProjectId = x.FusionProjectId,
                ReferenceCaseId = x.ReferenceCaseId,
                Description = x.Description,
                Country = x.Country,
                Currency = x.Currency,
                PhysicalUnit = x.PhysicalUnit,
                ProjectPhase = x.ProjectPhase,
                InternalProjectPhase = x.InternalProjectPhase,
                ProjectCategory = x.ProjectCategory,
                OilPriceUsd = x.OilPriceUsd,
                GasPriceNok = x.GasPriceNok,
                NglPriceUsd = x.NglPriceUsd,
                DiscountRate = x.DiscountRate,
                ExchangeRateUsdToNok = x.ExchangeRateUsdToNok,
                NpvYear = x.NpvYear,
                SharepointSiteUrl = x.SharepointSiteUrl,
                Cases = new List<CaseOverviewDto>(),
                Wells = new List<WellOverviewDto>()
            })
            .SingleAsync();
    }

    public async Task<List<CaseOverviewDto>> GetCases(Guid projectId)
    {
        return await context.Cases
            .Where(x => x.ProjectId == projectId)
            .Select(x => new CaseOverviewDto
            {
                CaseId = x.Id,
                ProjectId = x.ProjectId,
                Name = x.Name,
                Description = x.Description,
                Archived = x.Archived,
                ProductionStrategyOverview = x.ProductionStrategyOverview,
                ArtificialLift = x.ArtificialLift,
                ProducerCount = x.ProducerCount,
                GasInjectorCount = x.GasInjectorCount,
                WaterInjectorCount = x.WaterInjectorCount,
                Npv = x.Npv,
                NpvOverride = x.NpvOverride,
                BreakEven = x.BreakEven,
                BreakEvenOverride = x.BreakEvenOverride,
                FacilitiesAvailability = x.FacilitiesAvailability,
                CapexFactorFeasibilityStudies = x.CapexFactorFeasibilityStudies,
                CapexFactorFeedStudies = x.CapexFactorFeedStudies,
                InitialYearsWithoutWellInterventionCost = x.InitialYearsWithoutWellInterventionCost,
                FinalYearsWithoutWellInterventionCost = x.FinalYearsWithoutWellInterventionCost,
                Host = x.Host,
                AverageCo2Intensity = x.AverageCo2Intensity,
                DiscountedCashflow = x.DiscountedCashflow,
                Co2RemovedFromGas = x.Co2RemovedFromGas,
                Co2EmissionFromFuelGas = x.Co2EmissionFromFuelGas,
                FlaredGasPerProducedVolume = x.FlaredGasPerProducedVolume,
                Co2EmissionsFromFlaredGas = x.Co2EmissionsFromFlaredGas,
                Co2Vented = x.Co2Vented,
                DailyEmissionFromDrillingRig = x.DailyEmissionFromDrillingRig,
                AverageDevelopmentDrillingDays = x.AverageDevelopmentDrillingDays,
                DgaDate = x.DgaDate,
                DgbDate = x.DgbDate,
                DgcDate = x.DgcDate,
                ApboDate = x.ApboDate,
                BorDate = x.BorDate,
                VpboDate = x.VpboDate,
                Dg0Date = x.Dg0Date,
                Dg1Date = x.Dg1Date,
                Dg2Date = x.Dg2Date,
                Dg3Date = x.Dg3Date,
                Dg4Date = x.Dg4Date,
                CreatedUtc = x.CreatedUtc,
                UpdatedUtc = x.UpdatedUtc,
                SurfId = x.SurfId,
                SubstructureId = x.SubstructureId,
                TopsideId = x.TopsideId,
                TransportId = x.TransportId,
                OnshorePowerSupplyId = x.OnshorePowerSupplyId,
                SharepointFileId = x.SharepointFileId,
                SharepointFileName = x.SharepointFileName,
                SharepointFileUrl = x.SharepointFileUrl,
                SharepointUpdatedTimestampUtc = x.SharepointUpdatedTimestampUtc,
                TableRanges = TableRangesMapper.MapToDto(x)
            })
            .ToListAsync();
    }

    public async Task<List<WellOverviewDto>> GetWells(Guid projectId)
    {
        return await context.Wells
            .Where(x => x.ProjectId == projectId)
            .Select(x => new WellOverviewDto
            {
                Id = x.Id,
                Name = x.Name,
                WellCategory = x.WellCategory,
                WellCost = x.WellCost,
                DrillingDays = x.DrillingDays,
                PlugingAndAbandonmentCost = x.PlugingAndAbandonmentCost,
                WellInterventionCost = x.WellInterventionCost
            })
            .ToListAsync();
    }
}
