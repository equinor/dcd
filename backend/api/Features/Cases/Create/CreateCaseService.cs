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
            DrainageStrategy = CreateDrainageStrategy(),
            Topside = CreateTopside(),
            Surf = CreateSurf(),
            Substructure = CreateSubstructure(),
            Transport = CreateTransport(),
            Exploration = CreateExploration(projectPk),
            WellProject = CreateWellProject(projectPk),
            OnshorePowerSupply = CreateOnshorePowerSupply(),
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
                    CampaignType = CampaignTypes.DevelopmentCampaign,
                    RigUpgradingCostValues = [],
                    RigMobDemobCostValues = []
                },
                new Campaign
                {
                    CampaignType = CampaignTypes.ExplorationCampaign,
                    RigUpgradingCostValues = [],
                    RigMobDemobCostValues = []
                }
            ],
        };

        context.Cases.Add(createdCase);

        await context.SaveChangesAsync();
    }

    private static DrainageStrategy CreateDrainageStrategy()
    {
        return new DrainageStrategy
        {
            Name = "Drainage Strategy",
            Description = "Drainage Strategy",

            NGLYield = 0,
            ProducerCount = 0,
            GasInjectorCount = 0,
            WaterInjectorCount = 0,
            ArtificialLift = ArtificialLift.NoArtificialLift,
            GasSolution = GasSolution.Export
        };
    }

    private static Topside CreateTopside()
    {
        return new Topside
        {
            Name = "Topside",

            DryWeight = 0,
            OilCapacity = 0,
            GasCapacity = 0,
            WaterInjectionCapacity = 0,
            ArtificialLift = ArtificialLift.NoArtificialLift,
            Maturity = Maturity.A,
            Currency = 0,
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

    private static Surf CreateSurf()
    {
        return new Surf
        {
            Name = "Surf",

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
            Currency = 0,
            LastChangedDate = null,
            CostYear = 0,
            Source = Source.ConceptApp,
            ProspVersion = null,
            ApprovedBy = "",
            DG3Date = null,
            DG4Date = null
        };
    }

    private static Substructure CreateSubstructure()
    {
        return new Substructure
        {
            Name = "Substructure",
            DryWeight = 0,
            Maturity = Maturity.A,
            Currency = 0,
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

    private static Transport CreateTransport()
    {
        return new Transport
        {
            Name = "Transport",
            GasExportPipelineLength = 0,
            OilExportPipelineLength = 0,
            Maturity = Maturity.A,
            Currency = 0,
            LastChangedDate = null,
            CostYear = 0,
            Source = Source.ConceptApp,
            ProspVersion = null,
            DG3Date = null,
            DG4Date = null
        };
    }

    private static OnshorePowerSupply CreateOnshorePowerSupply()
    {
        return new OnshorePowerSupply
        {
            Name = "OnshorePowerSupply",
            LastChangedDate = null,
            CostYear = 0,
            Source = Source.ConceptApp,
            ProspVersion = null,
            DG3Date = null,
            DG4Date = null
        };
    }

    private static Exploration CreateExploration(Guid projectPk)
    {
        return new Exploration
        {
            Name = "Exploration",
            ProjectId = projectPk,
            RigMobDemob = 0,
            Currency = 0,
            ExplorationWells = []
        };
    }

    private static WellProject CreateWellProject(Guid projectPk)
    {
        return new WellProject
        {
            Name = "Well Project",
            ProjectId = projectPk,
            ArtificialLift = ArtificialLift.NoArtificialLift,
            Currency = 0,
            DevelopmentWells = []
        };
    }
}
