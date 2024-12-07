using api.Context;
using api.Features.ProjectData.Dtos;
using api.Features.ProjectData.Dtos.AssetDtos;
using api.Features.ProjectMembers.Get;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ProjectData;

public class GetProjectDataRepository(DcdDbContext context)
{
    public async Task<Guid> GetProjectIdFromFusionId(Guid projectId)
    {
        return await context.Projects
            .Where(p => (p.Id.Equals(projectId) || p.FusionProjectId.Equals(projectId)) && !p.IsRevision)
            .Select(x => x.Id)
            .FirstAsync();
    }

    public async Task<List<ProjectMemberDto>> GetProjectMembers(Guid projectId)
    {
        return await context.ProjectMembers
            .Where(x => x.ProjectId == projectId)
            .Select(x => new ProjectMemberDto
            {
                ProjectId = x.ProjectId,
                UserId = x.UserId,
                Role = x.Role
            })
            .ToListAsync();
    }

    public async Task<List<RevisionDetailsDto>> GetRevisionDetailsList(Guid projectId)
    {
        return await context.RevisionDetails
            .Where(r => r.OriginalProjectId == projectId)
            .Select(x => new RevisionDetailsDto
            {
                Id = x.Id,
                OriginalProjectId = x.OriginalProjectId,
                RevisionId = x.RevisionId,
                RevisionName = x.RevisionName,
                RevisionDate = x.RevisionDate,
                Arena = x.Arena,
                Mdqc = x.Mdqc,
                Classification = x.Classification
            })
            .ToListAsync();
    }

    public async Task<RevisionDetailsDto> GetRevisionDetails(Guid revisionId)
    {
        return await context.RevisionDetails
            .Where(x => x.RevisionId == revisionId)
            .Select(x => new RevisionDetailsDto
            {
                Id = x.Id,
                OriginalProjectId = x.OriginalProjectId,
                RevisionId = x.RevisionId,
                RevisionName = x.RevisionName,
                RevisionDate = x.RevisionDate,
                Arena = x.Arena,
                Mdqc = x.Mdqc,
                Classification = x.Classification
            })
            .SingleAsync();
    }

    public async Task<CommonProjectAndRevisionDto> GetCommonProjectAndRevisionData(Guid projectId)
    {
        return await context.Projects
            .Where(x => x.Id == projectId)
            .Select(x => new CommonProjectAndRevisionDto
            {
                ModifyTime = x.ModifyTime,
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
                CO2RemovedFromGas = x.CO2RemovedFromGas,
                CO2EmissionFromFuelGas = x.CO2EmissionFromFuelGas,
                FlaredGasPerProducedVolume = x.FlaredGasPerProducedVolume,
                CO2EmissionsFromFlaredGas = x.CO2EmissionsFromFlaredGas,
                CO2Vented = x.CO2Vented,
                DailyEmissionFromDrillingRig = x.DailyEmissionFromDrillingRig,
                AverageDevelopmentDrillingDays = x.AverageDevelopmentDrillingDays,
                OilPriceUSD = x.OilPriceUSD,
                GasPriceNOK = x.GasPriceNOK,
                DiscountRate = x.DiscountRate,
                SharepointSiteUrl = x.SharepointSiteUrl,
                ExplorationOperationalWellCosts = new ExplorationOperationalWellCostsOverviewDto
                {
                    ExplorationRigUpgrading = x.ExplorationOperationalWellCosts!.ExplorationRigUpgrading,
                    ExplorationRigMobDemob = x.ExplorationOperationalWellCosts.ExplorationRigMobDemob,
                    ExplorationProjectDrillingCosts = x.ExplorationOperationalWellCosts.ExplorationProjectDrillingCosts,
                    AppraisalRigMobDemob = x.ExplorationOperationalWellCosts.AppraisalRigMobDemob,
                    AppraisalProjectDrillingCosts = x.ExplorationOperationalWellCosts.AppraisalProjectDrillingCosts,
                },
                DevelopmentOperationalWellCosts = new DevelopmentOperationalWellCostsOverviewDto
                {
                    RigUpgrading = x.DevelopmentOperationalWellCosts!.RigUpgrading,
                    RigMobDemob = x.DevelopmentOperationalWellCosts.RigMobDemob,
                    AnnualWellInterventionCostPerWell = x.DevelopmentOperationalWellCosts.AnnualWellInterventionCostPerWell,
                    PluggingAndAbandonment = x.DevelopmentOperationalWellCosts.PluggingAndAbandonment
                },
                Cases = new List<CaseOverviewDto>(),
                Wells = new List<WellOverviewDto>(),
                Surfs = new List<SurfOverviewDto>(),
                Substructures = new List<SubstructureOverviewDto>(),
                Topsides = new List<TopsideOverviewDto>(),
                Transports = new List<TransportOverviewDto>(),
                DrainageStrategies = new List<DrainageStrategyOverviewDto>()
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
                NPV = x.NPV,
                NPVOverride = x.NPVOverride,
                BreakEven = x.BreakEven,
                BreakEvenOverride = x.BreakEvenOverride,
                FacilitiesAvailability = x.FacilitiesAvailability,
                CapexFactorFeasibilityStudies = x.CapexFactorFeasibilityStudies,
                CapexFactorFEEDStudies = x.CapexFactorFEEDStudies,
                Host = x.Host,
                DG0Date = x.DG0Date,
                DG1Date = x.DG1Date,
                DG2Date = x.DG2Date,
                DG3Date = x.DG3Date,
                DG4Date = x.DG4Date,
                CreateTime = x.CreateTime,
                ModifyTime = x.ModifyTime,
                SurfLink = x.SurfLink,
                SubstructureLink = x.SubstructureLink,
                TopsideLink = x.TopsideLink,
                TransportLink = x.TransportLink,
                SharepointFileId = x.SharepointFileId,
                SharepointFileName = x.SharepointFileName,
                SharepointFileUrl = x.SharepointFileUrl
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
                DrillingDays = x.DrillingDays
            })
            .ToListAsync();
    }

    public async Task<List<SurfOverviewDto>> GetSurfs(Guid projectId)
    {
        return await context.Surfs
            .Where(x => x.ProjectId == projectId)
            .Select(x => new SurfOverviewDto
            {
                Id = x.Id,
                Maturity = x.Maturity,
                Source = x.Source
            })
            .ToListAsync();
    }

    public async Task<List<SubstructureOverviewDto>> GetSubstructures(Guid projectId)
    {
        return await context.Substructures
            .Where(x => x.ProjectId == projectId)
            .Select(x => new SubstructureOverviewDto
            {
                Id = x.Id,
                Source = x.Source
            })
            .ToListAsync();
    }

    public async Task<List<TopsideOverviewDto>> GetTopsides(Guid projectId)
    {
        return await context.Topsides
            .Where(x => x.ProjectId == projectId)
            .Select(x => new TopsideOverviewDto
            {
                Id = x.Id,
                FuelConsumption = x.FuelConsumption,
                CO2ShareOilProfile = x.CO2ShareOilProfile,
                CO2ShareGasProfile = x.CO2ShareGasProfile,
                CO2ShareWaterInjectionProfile = x.CO2ShareWaterInjectionProfile,
                CO2OnMaxOilProfile = x.CO2OnMaxOilProfile,
                CO2OnMaxGasProfile = x.CO2OnMaxGasProfile,
                CO2OnMaxWaterInjectionProfile = x.CO2OnMaxWaterInjectionProfile,
                Source = x.Source
            })
            .ToListAsync();
    }

    public async Task<List<TransportOverviewDto>> GetTransports(Guid projectId)
    {
        return await context.Transports
            .Where(x => x.ProjectId == projectId)
            .Select(x => new TransportOverviewDto
            {
                Id = x.Id,
                Source = x.Source
            })
            .ToListAsync();
    }

    public async Task<List<DrainageStrategyOverviewDto>> GetDrainageStrategies(Guid projectId)
    {
        return await context.DrainageStrategies
            .Where(x => x.ProjectId == projectId)
            .Select(x => new DrainageStrategyOverviewDto
            {
                Id = x.Id
            })
            .ToListAsync();
    }
}
