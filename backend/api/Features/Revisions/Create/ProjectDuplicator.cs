using api.Models;

namespace api.Features.Revisions.Create;

public static class ProjectDuplicator
{
    public static Project DuplicateProject(Project existingProject, CreateRevisionDto createRevisionDto, Dictionary<Guid, Guid> caseIdMapping)
    {
        var projectId = Guid.NewGuid();

        var wellIdMapping = new Dictionary<Guid, Guid>();
        var wells = new List<Well>();

        foreach (var well in existingProject.Wells)
        {
            var newWellId = Guid.NewGuid();
            wellIdMapping.Add(well.Id, newWellId);

            wells.Add(new Well
            {
                Id = newWellId,
                ProjectId = projectId,
                Name = well.Name,
                WellCategory = well.WellCategory,
                WellCost = well.WellCost,
                DrillingDays = well.DrillingDays,
                PlugingAndAbandonmentCost = well.PlugingAndAbandonmentCost,
                WellInterventionCost = well.WellInterventionCost
            });
        }

        var newProject = new Project
        {
            Id = projectId,
            OriginalProjectId = existingProject.Id,
            IsRevision = true,
            Classification = createRevisionDto.Classification,
            RevisionDetails = new RevisionDetails
            {
                RevisionName = createRevisionDto.Name,
                Mdqc = createRevisionDto.Mdqc,
                Arena = createRevisionDto.Arena,
                Classification = createRevisionDto.Classification
            },

            Name = existingProject.Name,
            CommonLibraryId = existingProject.CommonLibraryId,
            FusionProjectId = existingProject.FusionProjectId,
            ReferenceCaseId = existingProject.ReferenceCaseId,
            CommonLibraryName = existingProject.CommonLibraryName,
            Description = existingProject.Description,
            Country = existingProject.Country,
            Currency = existingProject.Currency,
            PhysicalUnit = existingProject.PhysicalUnit,
            ProjectPhase = existingProject.ProjectPhase,
            InternalProjectPhase = existingProject.InternalProjectPhase,
            ProjectCategory = existingProject.ProjectCategory,
            SharepointSiteUrl = existingProject.SharepointSiteUrl,
            CO2RemovedFromGas = existingProject.CO2RemovedFromGas,
            CO2EmissionFromFuelGas = existingProject.CO2EmissionFromFuelGas,
            FlaredGasPerProducedVolume = existingProject.FlaredGasPerProducedVolume,
            CO2EmissionsFromFlaredGas = existingProject.CO2EmissionsFromFlaredGas,
            CO2Vented = existingProject.CO2Vented,
            DailyEmissionFromDrillingRig = existingProject.DailyEmissionFromDrillingRig,
            AverageDevelopmentDrillingDays = existingProject.AverageDevelopmentDrillingDays,
            OilPriceUSD = existingProject.OilPriceUSD,
            GasPriceNOK = existingProject.GasPriceNOK,
            DiscountRate = existingProject.DiscountRate,
            ExchangeRateUSDToNOK = existingProject.ExchangeRateUSDToNOK,
            NpvYear = existingProject.NpvYear,

            ExplorationOperationalWellCosts = new ExplorationOperationalWellCosts
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                ExplorationRigUpgrading = existingProject.ExplorationOperationalWellCosts.ExplorationRigUpgrading,
                ExplorationRigMobDemob = existingProject.ExplorationOperationalWellCosts.ExplorationRigMobDemob,
                ExplorationProjectDrillingCosts = existingProject.ExplorationOperationalWellCosts.ExplorationProjectDrillingCosts,
                AppraisalRigMobDemob = existingProject.ExplorationOperationalWellCosts.AppraisalRigMobDemob,
                AppraisalProjectDrillingCosts = existingProject.ExplorationOperationalWellCosts.AppraisalProjectDrillingCosts
            },

            DevelopmentOperationalWellCosts = new DevelopmentOperationalWellCosts
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                RigUpgrading = existingProject.DevelopmentOperationalWellCosts.RigUpgrading,
                RigMobDemob = existingProject.DevelopmentOperationalWellCosts.RigMobDemob,
                AnnualWellInterventionCostPerWell = existingProject.DevelopmentOperationalWellCosts.AnnualWellInterventionCostPerWell,
                PluggingAndAbandonment = existingProject.DevelopmentOperationalWellCosts.PluggingAndAbandonment
            },

            Wells = wells,
            Cases = existingProject.Cases.Select(caseItem => DuplicateCase(caseItem, projectId, caseIdMapping[caseItem.Id], wellIdMapping, true)).ToList(),

            // Mapped in a separate step later
            Images = [],

            // Intentionally not mapped for revisions
            Revisions = [],
            ProjectMembers = []
        };

        return newProject;
    }

    public static Case DuplicateCase(Case existingCaseItem, Guid projectId, Guid caseId)
    {
        return DuplicateCase(existingCaseItem, projectId, caseId, null, false);
    }

    private static Case DuplicateCase(Case existingCaseItem, Guid projectId, Guid caseId, Dictionary<Guid, Guid>? wellIdMapping, bool linkToOriginalCase)
    {
        var drainageStrategyId = Guid.NewGuid();
        var surfId = Guid.NewGuid();
        var substructureId = Guid.NewGuid();
        var topsideId = Guid.NewGuid();
        var transportId = Guid.NewGuid();
        var onshorePowerSupplyId = Guid.NewGuid();

        var newCase = new Case
        {
            Id = caseId,
            ProjectId = projectId,
            Name = existingCaseItem.Name,
            Description = existingCaseItem.Description,
            ReferenceCase = existingCaseItem.ReferenceCase,
            Archived = existingCaseItem.Archived,
            SharepointFileId = existingCaseItem.SharepointFileId,
            SharepointFileName = existingCaseItem.SharepointFileName,
            SharepointFileUrl = existingCaseItem.SharepointFileUrl,
            DGADate = existingCaseItem.DGADate,
            DGBDate = existingCaseItem.DGBDate,
            DGCDate = existingCaseItem.DGCDate,
            APBODate = existingCaseItem.APBODate,
            BORDate = existingCaseItem.BORDate,
            VPBODate = existingCaseItem.VPBODate,
            DG0Date = existingCaseItem.DG0Date,
            DG1Date = existingCaseItem.DG1Date,
            DG2Date = existingCaseItem.DG2Date,
            DG3Date = existingCaseItem.DG3Date,
            DG4Date = existingCaseItem.DG4Date,
            ArtificialLift = existingCaseItem.ArtificialLift,
            ProductionStrategyOverview = existingCaseItem.ProductionStrategyOverview,
            ProducerCount = existingCaseItem.ProducerCount,
            GasInjectorCount = existingCaseItem.GasInjectorCount,
            WaterInjectorCount = existingCaseItem.WaterInjectorCount,
            FacilitiesAvailability = existingCaseItem.FacilitiesAvailability,
            CapexFactorFeasibilityStudies = existingCaseItem.CapexFactorFeasibilityStudies,
            CapexFactorFEEDStudies = existingCaseItem.CapexFactorFEEDStudies,
            NPV = existingCaseItem.NPV,
            NPVOverride = existingCaseItem.NPVOverride,
            BreakEven = existingCaseItem.BreakEven,
            BreakEvenOverride = existingCaseItem.BreakEvenOverride,
            Host = existingCaseItem.Host,
            OriginalCaseId = linkToOriginalCase ? existingCaseItem.Id : null,

            DrainageStrategyId = drainageStrategyId,
            DrainageStrategy = new DrainageStrategy
            {
                Id = drainageStrategyId,
                CaseId = caseId,
                NGLYield = existingCaseItem.DrainageStrategy.NGLYield,
                GasShrinkageFactor = existingCaseItem.DrainageStrategy.GasShrinkageFactor,
                ProducerCount = existingCaseItem.DrainageStrategy.ProducerCount,
                GasInjectorCount = existingCaseItem.DrainageStrategy.GasInjectorCount,
                WaterInjectorCount = existingCaseItem.DrainageStrategy.WaterInjectorCount,
                ArtificialLift = existingCaseItem.DrainageStrategy.ArtificialLift,
                GasSolution = existingCaseItem.DrainageStrategy.GasSolution
            },

            SurfId = surfId,
            Surf = new Surf
            {
                Id = surfId,
                CaseId = caseId,
                CessationCost = existingCaseItem.Surf.CessationCost,
                Maturity = existingCaseItem.Surf.Maturity,
                InfieldPipelineSystemLength = existingCaseItem.Surf.InfieldPipelineSystemLength,
                UmbilicalSystemLength = existingCaseItem.Surf.UmbilicalSystemLength,
                ArtificialLift = existingCaseItem.Surf.ArtificialLift,
                RiserCount = existingCaseItem.Surf.RiserCount,
                TemplateCount = existingCaseItem.Surf.TemplateCount,
                ProducerCount = existingCaseItem.Surf.ProducerCount,
                GasInjectorCount = existingCaseItem.Surf.GasInjectorCount,
                WaterInjectorCount = existingCaseItem.Surf.WaterInjectorCount,
                ProductionFlowline = existingCaseItem.Surf.ProductionFlowline,
                CostYear = existingCaseItem.Surf.CostYear,
                Source = existingCaseItem.Surf.Source,
                ProspVersion = existingCaseItem.Surf.ProspVersion,
                ApprovedBy = existingCaseItem.Surf.ApprovedBy,
                DG3Date = existingCaseItem.Surf.DG3Date,
                DG4Date = existingCaseItem.Surf.DG4Date
            },

            SubstructureId = substructureId,
            Substructure = new Substructure
            {
                Id = substructureId,
                CaseId = caseId,
                DryWeight = existingCaseItem.Substructure.DryWeight,
                Maturity = existingCaseItem.Substructure.Maturity,
                ApprovedBy = existingCaseItem.Substructure.ApprovedBy,
                CostYear = existingCaseItem.Substructure.CostYear,
                ProspVersion = existingCaseItem.Substructure.ProspVersion,
                Source = existingCaseItem.Substructure.Source,
                Concept = existingCaseItem.Substructure.Concept,
                DG3Date = existingCaseItem.Substructure.DG3Date,
                DG4Date = existingCaseItem.Substructure.DG4Date
            },

            TopsideId = topsideId,
            Topside = new Topside
            {
                Id = topsideId,
                CaseId = caseId,
                DryWeight = existingCaseItem.Topside.DryWeight,
                OilCapacity = existingCaseItem.Topside.OilCapacity,
                GasCapacity = existingCaseItem.Topside.GasCapacity,
                WaterInjectionCapacity = existingCaseItem.Topside.WaterInjectionCapacity,
                ArtificialLift = existingCaseItem.Topside.ArtificialLift,
                Maturity = existingCaseItem.Topside.Maturity,
                FuelConsumption = existingCaseItem.Topside.FuelConsumption,
                FlaredGas = existingCaseItem.Topside.FlaredGas,
                ProducerCount = existingCaseItem.Topside.ProducerCount,
                GasInjectorCount = existingCaseItem.Topside.GasInjectorCount,
                WaterInjectorCount = existingCaseItem.Topside.WaterInjectorCount,
                CO2ShareOilProfile = existingCaseItem.Topside.CO2ShareOilProfile,
                CO2ShareGasProfile = existingCaseItem.Topside.CO2ShareGasProfile,
                CO2ShareWaterInjectionProfile = existingCaseItem.Topside.CO2ShareWaterInjectionProfile,
                CO2OnMaxOilProfile = existingCaseItem.Topside.CO2OnMaxOilProfile,
                CO2OnMaxGasProfile = existingCaseItem.Topside.CO2OnMaxGasProfile,
                CO2OnMaxWaterInjectionProfile = existingCaseItem.Topside.CO2OnMaxWaterInjectionProfile,
                CostYear = existingCaseItem.Topside.CostYear,
                ProspVersion = existingCaseItem.Topside.ProspVersion,
                Source = existingCaseItem.Topside.Source,
                ApprovedBy = existingCaseItem.Topside.ApprovedBy,
                DG3Date = existingCaseItem.Topside.DG3Date,
                DG4Date = existingCaseItem.Topside.DG4Date,
                FacilityOpex = existingCaseItem.Topside.FacilityOpex,
                PeakElectricityImported = existingCaseItem.Topside.PeakElectricityImported
            },

            TransportId = transportId,
            Transport = new Transport
            {
                Id = transportId,
                CaseId = caseId,
                GasExportPipelineLength = existingCaseItem.Transport.GasExportPipelineLength,
                OilExportPipelineLength = existingCaseItem.Transport.OilExportPipelineLength,
                Maturity = existingCaseItem.Transport.Maturity,
                CostYear = existingCaseItem.Transport.CostYear,
                Source = existingCaseItem.Transport.Source,
                ProspVersion = existingCaseItem.Transport.ProspVersion,
                DG3Date = existingCaseItem.Transport.DG3Date,
                DG4Date = existingCaseItem.Transport.DG4Date
            },

            OnshorePowerSupplyId = onshorePowerSupplyId,
            OnshorePowerSupply = new OnshorePowerSupply
            {
                Id = onshorePowerSupplyId,
                CaseId = caseId,
                CostYear = existingCaseItem.OnshorePowerSupply.CostYear,
                Source = existingCaseItem.OnshorePowerSupply.Source,
                ProspVersion = existingCaseItem.OnshorePowerSupply.ProspVersion,
                DG3Date = existingCaseItem.OnshorePowerSupply.DG3Date,
                DG4Date = existingCaseItem.OnshorePowerSupply.DG4Date
            },

            TimeSeriesProfiles = existingCaseItem.TimeSeriesProfiles
                .Select(x => new TimeSeriesProfile
                {
                    Id = Guid.NewGuid(),
                    CaseId = caseId,
                    ProfileType = x.ProfileType,
                    StartYear = x.StartYear,
                    Override = x.Override,
                    Values = x.Values
                })
                .ToList(),

            Campaigns = existingCaseItem.Campaigns
                .Select(x => new Campaign
                {
                    Id = Guid.NewGuid(),
                    CaseId = caseId,
                    RigUpgradingCostStartYear = x.RigUpgradingCostStartYear,
                    RigUpgradingCostValues = x.RigUpgradingCostValues,
                    RigMobDemobCostStartYear = x.RigMobDemobCostStartYear,
                    RigMobDemobCostValues = x.RigMobDemobCostValues,
                    CampaignType = x.CampaignType,
                    RigUpgradingCost = x.RigUpgradingCost,
                    RigMobDemobCost = x.RigMobDemobCost,
                    CampaignWells = x.CampaignWells.Select(y => new CampaignWell
                    {
                        Id = Guid.NewGuid(),
                        WellId = wellIdMapping == null ? y.WellId : wellIdMapping[y.WellId],
                        StartYear = y.StartYear,
                        Values = y.Values
                    })
                        .ToList()
                })
                .ToList()
        };

        return newCase;
    }
}
