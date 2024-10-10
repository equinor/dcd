using api.Models;
using api.Services;

namespace EconomicsServices
{
    public class CalculateTotalCostService : ICalculateTotalCostService
    {
        private readonly ICaseService _caseService;
        private readonly ISubstructureService _substructureService;
        private readonly ISurfService _surfService;
        private readonly ITopsideService _topsideService;
        private readonly ITransportService _transportService;
        private readonly IWellProjectService _wellProjectService;
        private readonly IExplorationService _explorationService;

        public CalculateTotalCostService(
            ICaseService caseService,
            ISubstructureService substructureService,
            ISurfService surfService,
            ITopsideService topsideService,
            ITransportService transportService,
            IWellProjectService wellProjectService,
            IExplorationService explorationService)
        {
            _caseService = caseService;
            _substructureService = substructureService;
            _surfService = surfService;
            _topsideService = topsideService;
            _transportService = transportService;
            _explorationService = explorationService;
            _wellProjectService = wellProjectService;
        }


        public async Task CalculateTotalCost(Guid caseId)
        {
            var caseItem = await _caseService.GetCaseWithIncludes(
                            caseId,
                            c => c.TotalFeasibilityAndConceptStudies!,
                            c => c.TotalFeasibilityAndConceptStudiesOverride!,
                            c => c.TotalFEEDStudies!,
                            c => c.TotalFEEDStudiesOverride!,
                            c => c.TotalOtherStudiesCostProfile!,
                            c => c.HistoricCostCostProfile!,
                            c => c.WellInterventionCostProfile!,
                            c => c.WellInterventionCostProfileOverride!,
                            c => c.OffshoreFacilitiesOperationsCostProfile!,
                            c => c.OffshoreFacilitiesOperationsCostProfileOverride!,
                            c => c.OnshoreRelatedOPEXCostProfile!,
                            c => c.AdditionalOPEXCostProfile!,
                            c => c.CessationWellsCost!,
                            c => c.CessationWellsCostOverride!,
                            c => c.CessationOffshoreFacilitiesCost!,
                            c => c.CessationOffshoreFacilitiesCostOverride!,
                            c => c.CessationOnshoreFacilitiesCostProfile!
                        );
            var totalStudyCost = CalculateStudyCost(caseItem);

            var studiesProfile = new TimeSeries<double>
            {
                StartYear = totalStudyCost.StartYear,
                Values = totalStudyCost.Values ?? Array.Empty<double>()
            };

            var totalOpexCost = CalculateOpexCost(caseItem);
            var opexProfile = new TimeSeries<double>
            {
                StartYear = totalOpexCost.StartYear,
                Values = totalOpexCost.Values ?? Array.Empty<double>()
            };

            var totalCessationCost = CalculateCessationCost(caseItem);
            var cessationProfile = new TimeSeries<double>
            {
                StartYear = totalCessationCost.StartYear,
                Values = totalCessationCost.Values ?? Array.Empty<double>()
            };

            var substructure = await _substructureService.GetSubstructureWithIncludes(
                caseItem.SubstructureLink,
                s => s.CostProfileOverride!,
                s => s.CostProfile!
            );

            var surf = await _surfService.GetSurfWithIncludes(
                caseItem.SurfLink,
                s => s.CostProfileOverride!,
                s => s.CostProfile!
            );

            var topside = await _topsideService.GetTopsideWithIncludes(
                caseItem.TopsideLink,
                t => t.CostProfileOverride!,
                t => t.CostProfile!
            );

            var transport = await _transportService.GetTransportWithIncludes(
                caseItem.TransportLink,
                t => t.CostProfileOverride!,
                t => t.CostProfile!
            );

            var totalOffshoreFacilityCost = CalculateTotalOffshoreFacilityCost(substructure, surf, topside, transport);
            var totalOffshoreFacilityProfile = new TimeSeries<double>
            {
                StartYear = totalOffshoreFacilityCost.StartYear,
                Values = totalOffshoreFacilityCost.Values
            };


            var wellProject = await _wellProjectService.GetWellProjectWithIncludes(
                caseItem.WellProjectLink,
                w => w.OilProducerCostProfileOverride!,
                w => w.OilProducerCostProfile!,
                w => w.GasProducerCostProfileOverride!,
                w => w.GasProducerCostProfile!,
                w => w.WaterInjectorCostProfileOverride!,
                w => w.WaterInjectorCostProfile!,
                w => w.GasInjectorCostProfileOverride!,
                w => w.GasInjectorCostProfile!
            );

            var totalDevelopmentCost = CalculateTotalDevelopmentCost(wellProject);
            var developmentProfile = new TimeSeries<double>
            {
                StartYear = totalDevelopmentCost.StartYear,
                Values = totalDevelopmentCost.Values
            };

            var exploration = await _explorationService.GetExplorationWithIncludes(
                caseItem.ExplorationLink,
                e => e.GAndGAdminCost!,
                e => e.CountryOfficeCost!,
                e => e.SeismicAcquisitionAndProcessing!,
                e => e.ExplorationWellCostProfile!,
                e => e.AppraisalWellCostProfile!,
                e => e.SidetrackCostProfile!);

            var explorationCost = CalculateTotalExplorationCost(exploration);
            var explorationProfile = new TimeSeries<double>
            {
                StartYear = explorationCost.StartYear,
                Values = explorationCost.Values
            };

            var totalCost = TimeSeriesCost.MergeCostProfilesList(
            [
                studiesProfile,
                opexProfile,
                cessationProfile,
                totalOffshoreFacilityProfile,
                developmentProfile,
                explorationProfile
            ]);

            if (caseItem.CalculatedTotalCostCostProfile != null)
            {
                caseItem.CalculatedTotalCostCostProfile.Values = totalCost.Values;
                caseItem.CalculatedTotalCostCostProfile.StartYear = totalCost.StartYear;
            }
            else
            {
                caseItem.CalculatedTotalCostCostProfile = new CalculatedTotalCostCostProfile
                {
                    Values = totalCost.Values,
                    StartYear = totalCost.StartYear
                };
            }

            return;
        }

        public TimeSeries<double> CalculateStudyCost(Case caseItem)
        {
            TimeSeries<double> feasibilityProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> feedProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> otherStudiesProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };

            if (caseItem.TotalFeasibilityAndConceptStudiesOverride?.Override == true)
            {
                feasibilityProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.TotalFeasibilityAndConceptStudiesOverride.StartYear,
                    Values = caseItem.TotalFeasibilityAndConceptStudiesOverride.Values ?? Array.Empty<double>()
                };
            }
            else if (caseItem.TotalFeasibilityAndConceptStudies != null)
            {
                feasibilityProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.TotalFeasibilityAndConceptStudies.StartYear,
                    Values = caseItem.TotalFeasibilityAndConceptStudies.Values ?? Array.Empty<double>()
                };
            }

            if (caseItem.TotalFEEDStudiesOverride?.Override == true)
            {
                feedProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.TotalFEEDStudiesOverride.StartYear,
                    Values = caseItem.TotalFEEDStudiesOverride.Values ?? Array.Empty<double>()
                };
            }
            else if (caseItem.TotalFEEDStudies != null)
            {
                feedProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.TotalFEEDStudies.StartYear,
                    Values = caseItem.TotalFEEDStudies.Values ?? Array.Empty<double>()
                };
            }

            if (caseItem.TotalOtherStudiesCostProfile != null)
            {
                otherStudiesProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.TotalOtherStudiesCostProfile.StartYear,
                    Values = caseItem.TotalOtherStudiesCostProfile.Values ?? Array.Empty<double>()
                };
            }

            var totalStudyCost = TimeSeriesCost.MergeCostProfilesList(
            [
                feasibilityProfile,
                feedProfile,
                otherStudiesProfile
            ]);
            return totalStudyCost;
        }

        public TimeSeries<double> CalculateOpexCost(Case caseItem)
        {
            TimeSeries<double> historicCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> wellInterventionProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> offshoreFacilitiesProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> onshoreRelatedOpexProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> additionalOpexProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };

            if (caseItem.HistoricCostCostProfile != null)
            {
                historicCostProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.HistoricCostCostProfile.StartYear,
                    Values = caseItem.HistoricCostCostProfile.Values ?? Array.Empty<double>()
                };
            }

            if (caseItem.WellInterventionCostProfileOverride?.Override == true)
            {
                wellInterventionProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.WellInterventionCostProfileOverride.StartYear,
                    Values = caseItem.WellInterventionCostProfileOverride.Values ?? Array.Empty<double>()
                };
            }
            else if (caseItem.WellInterventionCostProfile != null)
            {
                wellInterventionProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.WellInterventionCostProfile.StartYear,
                    Values = caseItem.WellInterventionCostProfile.Values ?? Array.Empty<double>()
                };
            }

            if (caseItem.OffshoreFacilitiesOperationsCostProfileOverride?.Override == true)
            {
                offshoreFacilitiesProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.OffshoreFacilitiesOperationsCostProfileOverride.StartYear,
                    Values = caseItem.OffshoreFacilitiesOperationsCostProfileOverride.Values ?? Array.Empty<double>()
                };
            }
            else if (caseItem.OffshoreFacilitiesOperationsCostProfile != null)
            {
                offshoreFacilitiesProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.OffshoreFacilitiesOperationsCostProfile.StartYear,
                    Values = caseItem.OffshoreFacilitiesOperationsCostProfile.Values ?? Array.Empty<double>()
                };
            }

            if (caseItem.OnshoreRelatedOPEXCostProfile != null)
            {
                onshoreRelatedOpexProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.OnshoreRelatedOPEXCostProfile.StartYear,
                    Values = caseItem.OnshoreRelatedOPEXCostProfile.Values ?? Array.Empty<double>()
                };
            }

            if (caseItem.AdditionalOPEXCostProfile != null)
            {
                additionalOpexProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.AdditionalOPEXCostProfile.StartYear,
                    Values = caseItem.AdditionalOPEXCostProfile.Values ?? Array.Empty<double>()
                };
            }

            var totalOpexCost = TimeSeriesCost.MergeCostProfilesList(
            [
                historicCostProfile,
                wellInterventionProfile,
                offshoreFacilitiesProfile,
                onshoreRelatedOpexProfile,
                additionalOpexProfile
            ]);
            return totalOpexCost;
        }

        public TimeSeries<double> CalculateCessationCost(Case caseItem)
        {
            TimeSeries<double> cessationWellsProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> cessationOffshoreFacilitiesProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> cessationOnshoreFacilitiesProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };

            if (caseItem.CessationWellsCostOverride?.Override == true)
            {
                cessationWellsProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.CessationWellsCostOverride.StartYear,
                    Values = caseItem.CessationWellsCostOverride.Values ?? Array.Empty<double>()
                };
            }
            else if (caseItem.CessationWellsCost != null)
            {
                cessationWellsProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.CessationWellsCost.StartYear,
                    Values = caseItem.CessationWellsCost.Values ?? Array.Empty<double>()
                };
            }

            if (caseItem.CessationOffshoreFacilitiesCostOverride?.Override == true)
            {
                cessationOffshoreFacilitiesProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.CessationOffshoreFacilitiesCostOverride.StartYear,
                    Values = caseItem.CessationOffshoreFacilitiesCostOverride.Values ?? Array.Empty<double>()
                };
            }
            else if (caseItem.CessationOffshoreFacilitiesCost != null)
            {
                cessationOffshoreFacilitiesProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.CessationOffshoreFacilitiesCost.StartYear,
                    Values = caseItem.CessationOffshoreFacilitiesCost.Values ?? Array.Empty<double>()
                };
            }

            if (caseItem.CessationOnshoreFacilitiesCostProfile != null)
            {
                cessationOnshoreFacilitiesProfile = new TimeSeries<double>
                {
                    StartYear = caseItem.CessationOnshoreFacilitiesCostProfile.StartYear,
                    Values = caseItem.CessationOnshoreFacilitiesCostProfile.Values ?? Array.Empty<double>()
                };
            }

            var totalCessationCost = TimeSeriesCost.MergeCostProfilesList(
            [
                cessationWellsProfile,
                cessationOffshoreFacilitiesProfile,
                cessationOnshoreFacilitiesProfile
            ]);
            return totalCessationCost;
        }


        public static TimeSeries<double> CalculateTotalOffshoreFacilityCost(
            Substructure? substructure,
            Surf? surf,
            Topside? topside,
            Transport? transport)
        {
            TimeSeries<double> substructureProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> surfProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> topsideProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> transportProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };

            if (substructure?.CostProfileOverride?.Override == true)
            {
                substructureProfile = new TimeSeries<double>
                {
                    StartYear = substructure.CostProfileOverride.StartYear,
                    Values = substructure.CostProfileOverride.Values ?? Array.Empty<double>()
                };
            }
            else if (substructure?.CostProfile != null)
            {
                substructureProfile = new TimeSeries<double>
                {
                    StartYear = substructure.CostProfile.StartYear,
                    Values = substructure.CostProfile.Values ?? Array.Empty<double>()
                };
            }

            if (surf?.CostProfileOverride?.Override == true)
            {
                surfProfile = new TimeSeries<double>
                {
                    StartYear = surf.CostProfileOverride.StartYear,
                    Values = surf.CostProfileOverride.Values ?? Array.Empty<double>()
                };
            }
            else if (surf?.CostProfile != null)
            {
                surfProfile = new TimeSeries<double>
                {
                    StartYear = surf.CostProfile.StartYear,
                    Values = surf.CostProfile.Values ?? Array.Empty<double>()
                };
            }

            if (topside?.CostProfileOverride?.Override == true)
            {
                topsideProfile = new TimeSeries<double>
                {
                    StartYear = topside.CostProfileOverride.StartYear,
                    Values = topside.CostProfileOverride.Values ?? Array.Empty<double>()
                };
            }
            else if (topside?.CostProfile != null)
            {
                topsideProfile = new TimeSeries<double>
                {
                    StartYear = topside.CostProfile.StartYear,
                    Values = topside.CostProfile.Values ?? Array.Empty<double>()
                };
            }

            if (transport?.CostProfileOverride?.Override == true)
            {
                transportProfile = new TimeSeries<double>
                {
                    StartYear = transport.CostProfileOverride.StartYear,
                    Values = transport.CostProfileOverride.Values ?? Array.Empty<double>()
                };
            }
            else if (transport?.CostProfile != null)
            {
                transportProfile = new TimeSeries<double>
                {
                    StartYear = transport.CostProfile.StartYear,
                    Values = transport.CostProfile.Values ?? Array.Empty<double>()
                };
            }

            var totalOffshoreFacilityCost = TimeSeriesCost.MergeCostProfilesList(
            [
                substructureProfile,
                surfProfile,
                topsideProfile,
                transportProfile
            ]);

            return totalOffshoreFacilityCost;
        }


        public static TimeSeries<double> CalculateTotalDevelopmentCost(WellProject wellProject)
        {

            TimeSeries<double> oilProducerProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> gasProducerProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> waterInjectorProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> gasInjectorProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };

            if (wellProject?.OilProducerCostProfileOverride?.Override == true)
            {
                oilProducerProfile = new TimeSeries<double>
                {
                    StartYear = wellProject.OilProducerCostProfileOverride?.StartYear ?? 0,
                    Values = wellProject.OilProducerCostProfileOverride?.Values ?? Array.Empty<double>()
                };
            }
            else if (wellProject?.OilProducerCostProfile != null)
            {
                oilProducerProfile = new TimeSeries<double>
                {
                    StartYear = wellProject.OilProducerCostProfile?.StartYear ?? 0,
                    Values = wellProject.OilProducerCostProfile?.Values ?? Array.Empty<double>()
                };
            }
            if (wellProject?.GasProducerCostProfileOverride?.Override == true)
            {
                gasProducerProfile = new TimeSeries<double>
                {
                    StartYear = wellProject.GasProducerCostProfileOverride?.StartYear ?? 0,
                    Values = wellProject.GasProducerCostProfileOverride?.Values ?? Array.Empty<double>()
                };
            }
            else if (wellProject?.GasProducerCostProfile != null)
            {
                gasProducerProfile = new TimeSeries<double>
                {
                    StartYear = wellProject.GasProducerCostProfile?.StartYear ?? 0,
                    Values = wellProject.GasProducerCostProfile?.Values ?? Array.Empty<double>()
                };
            }

            if (wellProject?.WaterInjectorCostProfileOverride?.Override == true)
            {
                waterInjectorProfile = new TimeSeries<double>
                {
                    StartYear = wellProject.WaterInjectorCostProfileOverride?.StartYear ?? 0,
                    Values = wellProject.WaterInjectorCostProfileOverride?.Values ?? Array.Empty<double>()
                };
            }
            else if (wellProject?.WaterInjectorCostProfile != null)
            {
                waterInjectorProfile = new TimeSeries<double>
                {
                    StartYear = wellProject.WaterInjectorCostProfile?.StartYear ?? 0,
                    Values = wellProject.WaterInjectorCostProfile?.Values ?? Array.Empty<double>()
                };
            }

            if (wellProject?.GasInjectorCostProfileOverride?.Override == true)
            {
                gasInjectorProfile = new TimeSeries<double>
                {
                    StartYear = wellProject.GasInjectorCostProfileOverride?.StartYear ?? 0,
                    Values = wellProject.GasInjectorCostProfileOverride?.Values ?? Array.Empty<double>()
                };
            }
            else if (wellProject?.GasInjectorCostProfile != null)
            {
                gasInjectorProfile = new TimeSeries<double>
                {
                    StartYear = wellProject.GasInjectorCostProfile?.StartYear ?? 0,
                    Values = wellProject.GasInjectorCostProfile?.Values ?? Array.Empty<double>()
                };
            }

            var totalDevelopmentCost = TimeSeriesCost.MergeCostProfilesList(
            [
                oilProducerProfile,
                gasProducerProfile,
                waterInjectorProfile,
                gasInjectorProfile
            ]);

            return totalDevelopmentCost;
        }

        public static TimeSeries<double> CalculateTotalExplorationCost(Exploration exploration)
        {


            TimeSeries<double> gAndGAdminCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> seismicAcquisitionAndProcessingProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> countryOfficeCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> explorationWellCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> appraisalWellCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> sidetrackCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };

            if (exploration?.GAndGAdminCostOverride?.Override == true)
            {
                gAndGAdminCostProfile = new TimeSeries<double>
                {
                    StartYear = exploration.GAndGAdminCostOverride.StartYear,
                    Values = exploration.GAndGAdminCostOverride.Values ?? Array.Empty<double>()
                };
            }
            else if (exploration?.GAndGAdminCost != null)
            {
                gAndGAdminCostProfile = new TimeSeries<double>
                {
                    StartYear = exploration.GAndGAdminCost.StartYear,
                    Values = exploration.GAndGAdminCost.Values ?? Array.Empty<double>()
                };
            }

            if (exploration?.SeismicAcquisitionAndProcessing != null)
            {
                seismicAcquisitionAndProcessingProfile = new TimeSeries<double>
                {
                    StartYear = exploration.SeismicAcquisitionAndProcessing.StartYear,
                    Values = exploration.SeismicAcquisitionAndProcessing.Values ?? Array.Empty<double>()
                };
            }

            if (exploration?.CountryOfficeCost != null)
            {
                countryOfficeCostProfile = new TimeSeries<double>
                {
                    StartYear = exploration.CountryOfficeCost.StartYear,
                    Values = exploration.CountryOfficeCost.Values ?? Array.Empty<double>()
                };
            }

            if (exploration?.ExplorationWellCostProfile != null)
            {
                explorationWellCostProfile = new TimeSeries<double>
                {
                    StartYear = exploration.ExplorationWellCostProfile.StartYear,
                    Values = exploration.ExplorationWellCostProfile.Values ?? Array.Empty<double>()
                };
            }

            if (exploration?.AppraisalWellCostProfile != null)
            {
                appraisalWellCostProfile = new TimeSeries<double>
                {
                    StartYear = exploration.AppraisalWellCostProfile.StartYear,
                    Values = exploration.AppraisalWellCostProfile.Values ?? Array.Empty<double>()
                };
            }

            if (exploration?.SidetrackCostProfile != null)
            {
                sidetrackCostProfile = new TimeSeries<double>
                {
                    StartYear = exploration.SidetrackCostProfile.StartYear,
                    Values = exploration.SidetrackCostProfile.Values ?? Array.Empty<double>()
                };
            }

            var totalexploration = TimeSeriesCost.MergeCostProfilesList(
            [
                gAndGAdminCostProfile,
                seismicAcquisitionAndProcessingProfile,
                countryOfficeCostProfile,
                explorationWellCostProfile,
                appraisalWellCostProfile,
                sidetrackCostProfile
            ]);

            return totalexploration;
        }
    }
}
