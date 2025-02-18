using api.Context;
using api.Context.Extensions;
using api.Features.Profiles;
using api.Models;
using api.Models.Enums;

namespace api.Features.Cases.Create;

public class CreateCaseService(DcdDbContext context)
{
    public async Task CreateCase(Guid projectId, CreateCaseDto createCaseDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var topsideId = Guid.NewGuid();
        var drainageStrategyId = Guid.NewGuid();
        var surfId = Guid.NewGuid();
        var substructureId = Guid.NewGuid();
        var transportId = Guid.NewGuid();
        var explorationId = Guid.NewGuid();
        var wellProjectId = Guid.NewGuid();
        var onshorePowerSupplyId = Guid.NewGuid();

        var createdCase = new Case
        {
            ProjectId = projectPk,
            Name = createCaseDto.Name,
            Description = createCaseDto.Description,
            ProductionStrategyOverview = createCaseDto.ProductionStrategyOverview,
            ProducerCount = createCaseDto.ProducerCount,
            GasInjectorCount = createCaseDto.GasInjectorCount,
            WaterInjectorCount = createCaseDto.WaterInjectorCount,
            DG4Date = createCaseDto.DG4Date == DateTime.MinValue ? new DateTime(2030, 1, 1) : createCaseDto.DG4Date,
            CapexFactorFeasibilityStudies = 0.015,
            CapexFactorFEEDStudies = 0.015,

            DrainageStrategyId = drainageStrategyId,
            DrainageStrategy = CreateDrainageStrategy(drainageStrategyId),

            TopsideId = topsideId,
            Topside = CreateTopside(topsideId),

            SurfId = surfId,
            Surf = CreateSurf(surfId),

            SubstructureId = substructureId,
            Substructure = CreateSubstructure(substructureId),

            TransportId = transportId,
            Transport = CreateTransport(transportId),

            ExplorationId = explorationId,
            Exploration = CreateExploration(explorationId),

            WellProjectId = wellProjectId,
            WellProject = CreateWellProject(wellProjectId),

            OnshorePowerSupplyId = onshorePowerSupplyId,
            OnshorePowerSupply = CreateOnshorePowerSupply(onshorePowerSupplyId),

            TimeSeriesProfiles =
            [
                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.TopsideCostProfileOverride,
                    Override = true,
                    Values = []
                },
                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.TransportCostProfileOverride,
                    Override = true,
                    Values = []
                },
                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.SurfCostProfileOverride,
                    Override = true,
                    Values = []
                },
                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.SubstructureCostProfileOverride,
                    Override = true,
                    Values = []
                },
                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.OnshorePowerSupplyCostProfileOverride,
                    Override = true,
                    Values = []
                }
            ],
            Campaigns =
            [
                new Campaign
                {
                    CampaignType = CampaignType.DevelopmentCampaign,
                    RigUpgradingCostValues = [],
                    RigMobDemobCostValues = []
                },
                new Campaign
                {
                    CampaignType = CampaignType.ExplorationCampaign,
                    RigUpgradingCostValues = [],
                    RigMobDemobCostValues = []
                }
            ],
        };

        context.Cases.Add(createdCase);

        await context.SaveChangesAsync();
    }

    private static DrainageStrategy CreateDrainageStrategy(Guid id)
    {
        return new DrainageStrategy
        {
            Id = id,
            Description = "Drainage Strategy",

            NGLYield = 0,
            ProducerCount = 0,
            GasInjectorCount = 0,
            WaterInjectorCount = 0,
            ArtificialLift = ArtificialLift.NoArtificialLift,
            GasSolution = GasSolution.Export
        };
    }

    private static Topside CreateTopside(Guid id)
    {
        return new Topside
        {
            Id = id,
            DryWeight = 0,
            OilCapacity = 0,
            GasCapacity = 0,
            WaterInjectionCapacity = 0,
            ArtificialLift = ArtificialLift.NoArtificialLift,
            Maturity = Maturity.A,
            FuelConsumption = 0,
            FlaredGas = 0,
            ProducerCount = 0,
            GasInjectorCount = 0,
            WaterInjectorCount = 0,
            CO2ShareOilProfile = 0,
            CO2ShareGasProfile = 0,
            CO2ShareWaterInjectionProfile = 0,
            CO2OnMaxOilProfile = 0,
            CO2OnMaxGasProfile = 0,
            CO2OnMaxWaterInjectionProfile = 0,
            CostYear = 0,
            ProspVersion = null,
            LastChangedDate = null,
            Source = Source.ConceptApp,
            ApprovedBy = "",
            DG3Date = null,
            DG4Date = null,
            FacilityOpex = 0,
            PeakElectricityImported = 0
        };
    }

    private static Surf CreateSurf(Guid id)
    {
        return new Surf
        {
            Id = id,
            CessationCost = 0,
            Maturity = Maturity.A,
            InfieldPipelineSystemLength = 0,
            UmbilicalSystemLength = 0,
            ArtificialLift = ArtificialLift.NoArtificialLift,
            RiserCount = 0,
            TemplateCount = 0,
            ProducerCount = 0,
            GasInjectorCount = 0,
            WaterInjectorCount = 0,
            ProductionFlowline = ProductionFlowline.No_production_flowline,
            LastChangedDate = null,
            CostYear = 0,
            Source = Source.ConceptApp,
            ProspVersion = null,
            ApprovedBy = "",
            DG3Date = null,
            DG4Date = null
        };
    }

    private static Substructure CreateSubstructure(Guid id)
    {
        return new Substructure
        {
            Id = id,
            DryWeight = 0,
            Maturity = Maturity.A,
            ApprovedBy = "",
            CostYear = 0,
            ProspVersion = null,
            Source = Source.ConceptApp,
            LastChangedDate = null,
            Concept = Concept.NO_CONCEPT,
            DG3Date = null,
            DG4Date = null
        };
    }

    private static Transport CreateTransport(Guid id)
    {
        return new Transport
        {
            Id = id,
            GasExportPipelineLength = 0,
            OilExportPipelineLength = 0,
            Maturity = Maturity.A,
            LastChangedDate = null,
            CostYear = 0,
            Source = Source.ConceptApp,
            ProspVersion = null,
            DG3Date = null,
            DG4Date = null
        };
    }

    private static OnshorePowerSupply CreateOnshorePowerSupply(Guid id)
    {
        return new OnshorePowerSupply
        {
            Id = id,
            LastChangedDate = null,
            CostYear = 0,
            Source = Source.ConceptApp,
            ProspVersion = null,
            DG3Date = null,
            DG4Date = null
        };
    }

    private static Exploration CreateExploration(Guid id)
    {
        return new Exploration
        {
            Id = id,
            ExplorationWells = []
        };
    }

    private static WellProject CreateWellProject(Guid id)
    {
        return new WellProject
        {
            Id = id,
            DevelopmentWells = []
        };
    }
}
