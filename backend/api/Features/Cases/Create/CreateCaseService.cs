using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Update;
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
        var onshorePowerSupplyId = Guid.NewGuid();

        var dgDates = CalculateDgDates(createCaseDto.Dg4Date);

        var createdCase = new Case
        {
            ProjectId = projectPk,
            Name = createCaseDto.Name,
            Description = createCaseDto.Description,
            ProductionStrategyOverview = createCaseDto.ProductionStrategyOverview,
            ProducerCount = createCaseDto.ProducerCount,
            GasInjectorCount = createCaseDto.GasInjectorCount,
            WaterInjectorCount = createCaseDto.WaterInjectorCount,
            Dg4Date = dgDates.dg4,
            Dg3Date = dgDates.dg3,
            Dg2Date = dgDates.dg2,
            Dg1Date = dgDates.dg1,
            Dg0Date = dgDates.dg0,
            CapexFactorFeasibilityStudies = 0.015,
            CapexFactorFeedStudies = 0.015,
            InitialYearsWithoutWellInterventionCost = 0,
            FinalYearsWithoutWellInterventionCost = 0,
            Co2EmissionFromFuelGas = 2.34,
            FlaredGasPerProducedVolume = 1.122765,
            Co2EmissionsFromFlaredGas = 3.73,
            Co2Vented = 1.96,
            DailyEmissionFromDrillingRig = 100,
            AverageDevelopmentDrillingDays = 50,
            FacilitiesAvailability = 90,

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
            ]
        };

        context.Cases.Add(createdCase);

        await context.SaveChangesAsync();
    }

    private static DrainageStrategy CreateDrainageStrategy(Guid id)
    {
        return new DrainageStrategy
        {
            Id = id,
            NglYield = 0,
            CondensateYield = 0,
            GasShrinkageFactor = 0,
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
            Co2ShareOilProfile = 0,
            Co2ShareGasProfile = 0,
            Co2ShareWaterInjectionProfile = 0,
            Co2OnMaxOilProfile = 0,
            Co2OnMaxGasProfile = 0,
            Co2OnMaxWaterInjectionProfile = 0,
            CostYear = 0,
            ProspVersion = null,
            Source = Source.ConceptApp,
            ApprovedBy = "",
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
            ProductionFlowline = ProductionFlowline.NoProductionFlowline,
            CostYear = 0,
            Source = Source.ConceptApp,
            ProspVersion = null,
            ApprovedBy = ""
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
            Concept = Concept.NoConcept
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
            CostYear = 0,
            Source = Source.ConceptApp,
            ProspVersion = null
        };
    }

    private static OnshorePowerSupply CreateOnshorePowerSupply(Guid id)
    {
        return new OnshorePowerSupply
        {
            Id = id,
            CostYear = 0,
            Source = Source.ConceptApp,
            ProspVersion = null
        };
    }

    public static (DateTime dg4, DateTime? dg3, DateTime? dg2, DateTime? dg1, DateTime? dg0) CalculateDgDates(DateTime dg4date)
    {
        if (dg4date <= UpdateCaseDtoValidator.MinAllowedDgDate.AddMonths(72))
        {
            return (dg4date, null, null, null, null);
        }

        var dg3Date = dg4date.AddMonths(-36);
        var dg2Date = dg3Date.AddMonths(-12);
        var dg1Date = dg2Date.AddMonths(-12);
        var dg0Date = dg1Date.AddMonths(-12);

        return (dg4date, dg3Date, dg2Date, dg1Date, dg0Date);
    }
}
