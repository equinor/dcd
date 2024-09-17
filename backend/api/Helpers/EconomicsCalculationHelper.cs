using api.Dtos;
using api.Models;
using api.Repositories;
using api.Services;
using api.Services.GenerateCostProfiles;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class EconomicsCalculationHelper : IEconomicsCalculationHelper
    {
        private const int Cd = 365;
        private readonly IStudyCostProfileService _studyCostProfileService;
        private readonly IOpexCostProfileService _opexCostProfileService;
        private readonly ICessationCostProfileService _cessationCostProfileService;
        private readonly IExplorationRepository _explorationRepository;
        private readonly ISubstructureRepository _substructureRepository;
        private readonly ISurfRepository _surfRepository;
        private readonly ITopsideRepository _topsideRepository;
        private readonly ITransportRepository _transportRepository;
        private readonly IWellProjectRepository _wellProjectRepository;
        private readonly ICo2IntensityTotalService _co2IntensityTotalService;
        private readonly IProjectService _projectService;
        private readonly ISubstructureTimeSeriesRepository _substructureTimeSeriesRepository;


        public EconomicsCalculationHelper(
            IStudyCostProfileService studyCostProfileService,
            IOpexCostProfileService opexCostProfileService,
            ICessationCostProfileService cessationCostProfileService,
            IExplorationRepository explorationRepository,
            ISubstructureRepository substructureRepository,
            ISurfRepository surfRepository,
            ITopsideRepository topsideRepository,
            ITransportRepository transportRepository,
            IWellProjectRepository wellProjectRepository,
            ICo2IntensityTotalService co2IntensityTotalService,
            IProjectService projectService,
            ISubstructureTimeSeriesRepository substructureTimeSeriesRepository)
        {
            _studyCostProfileService = studyCostProfileService;
            _opexCostProfileService = opexCostProfileService;
            _cessationCostProfileService = cessationCostProfileService;
            _explorationRepository = explorationRepository;
            _substructureRepository = substructureRepository;
            _surfRepository = surfRepository;
            _topsideRepository = topsideRepository;
            _transportRepository = transportRepository;
            _wellProjectRepository = wellProjectRepository;
            _co2IntensityTotalService = co2IntensityTotalService;
            _projectService = projectService;
            _substructureTimeSeriesRepository = substructureTimeSeriesRepository;
        }

        public static TimeSeries<double> CalculateIncome(DrainageStrategy drainageStrategy, Project project, Case caseItem)
        {
            // Remove Project as input, use caseItem.Project instead
            var oilPrice = project.OilPriceUSD;
            var gasPrice = project.GasPriceNOK;
            var cubicMetersToBarrelsFactor = 6.29;
            var exchangeRateUSDToNOK = project.ExchangeRateUSDToNOK;
            var exchangeRateNOKToUSD = 1 / exchangeRateUSDToNOK;

            var oilProfile = drainageStrategy.ProductionProfileOil?.Values ?? Array.Empty<double>();
            var additionalOilProfile = drainageStrategy.AdditionalProductionProfileOil?.Values ?? Array.Empty<double>();

            var totalOilProductionInMegaCubics = TimeSeriesCost.MergeCostProfiles(
                new TimeSeries<double>
                {
                    StartYear = drainageStrategy.ProductionProfileOil?.StartYear ?? 0,
                    Values = oilProfile
                },
                new TimeSeries<double>
                {
                    StartYear = drainageStrategy.AdditionalProductionProfileOil?.StartYear ?? 0,
                    Values = additionalOilProfile
                }
            );

            // Convert oil production from million sm³ to barrels in millions
            var oilProductionInMillionsOfBarrels = totalOilProductionInMegaCubics.Values.Select(v => v * cubicMetersToBarrelsFactor).ToArray(); //originalt del på 1 mill

            var oilIncome = new TimeSeries<double>
            {
                StartYear = totalOilProductionInMegaCubics.StartYear,
                Values = oilProductionInMillionsOfBarrels.Select(v => v * oilPrice * exchangeRateUSDToNOK).ToArray(),
            };

            var gasProfile = drainageStrategy.ProductionProfileGas?.Values ?? Array.Empty<double>();
            var additionalGasProfile = drainageStrategy.AdditionalProductionProfileGas?.Values ?? Array.Empty<double>();

            var totalGasProductionInGigaCubics = TimeSeriesCost.MergeCostProfiles(
                new TimeSeries<double>
                {
                    StartYear = drainageStrategy.ProductionProfileGas?.StartYear ?? 0,
                    Values = gasProfile
                },
                new TimeSeries<double>
                {
                    StartYear = drainageStrategy.AdditionalProductionProfileGas?.StartYear ?? 0,
                    Values = additionalGasProfile
                }
            );

            var gasIncome = new TimeSeries<double>
            {
                StartYear = totalGasProductionInGigaCubics.StartYear,
                Values = totalGasProductionInGigaCubics.Values.Select(v => v * gasPrice * 1000).ToArray() //originally divide by 1 mill instead of multiply by 1000
            };


            var totalIncome = TimeSeriesCost.MergeCostProfiles(oilIncome, gasIncome);
            var calculatedTotalIncomeCostProfile = new CalculatedTotalIncomeCostProfile
            {
                StartYear = totalIncome.StartYear,
                Values = totalIncome.Values,
            };

            caseItem.CalculatedTotalIncomeCostProfile = calculatedTotalIncomeCostProfile;
            return totalIncome;
        }



        public async Task<TimeSeries<double>> CalculateTotalCostAsync(Case caseItem)
        {
            var totalStudyCost = await _studyCostProfileService.Generate(caseItem.Id);
            var studiesProfile = new TimeSeries<double>
            {
                StartYear = totalStudyCost.StudyCostProfileDto?.StartYear ?? 0,
                Values = totalStudyCost.StudyCostProfileDto?.Values ?? Array.Empty<double>()
            };

            var totalOpexCost = await _opexCostProfileService.Generate(caseItem.Id);
            var opexProfile = new TimeSeries<double>
            {
                StartYear = totalOpexCost.OpexCostProfileDto?.StartYear ?? 0,
                Values = totalOpexCost.OpexCostProfileDto?.Values ?? Array.Empty<double>()
            };

            var totalCessationCost = await _cessationCostProfileService.Generate(caseItem.Id);
            var cessationProfile = new TimeSeries<double>
            {
                StartYear = totalCessationCost.CessationCostDto?.StartYear ?? 0,
                Values = totalCessationCost.CessationCostDto?.Values ?? Array.Empty<double>()
            };

            var totalOffshoreFacilityCost = await CalculateTotalOffshoreFacilityCostAsync(caseItem);
            var totalOffshoreFacilityProfile = new TimeSeries<double>
            {
                StartYear = totalOffshoreFacilityCost.StartYear,
                Values = totalOffshoreFacilityCost.Values
            };

            var totalDevelopmentCost = await CalculateTotalDevelopmentCostAsync(caseItem);
            var developmentProfile = new TimeSeries<double>
            {
                StartYear = totalDevelopmentCost.StartYear,
                Values = totalDevelopmentCost.Values
            };

            var explorationCost = await CalculateTotalExplorationCostAsync(caseItem);
            var explorationProfile = new TimeSeries<double>
            {
                StartYear = explorationCost.StartYear,
                Values = explorationCost.Values
            };

            var totalCost = TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>>
            {
                studiesProfile,
                opexProfile,
                cessationProfile,
                totalOffshoreFacilityProfile,
                developmentProfile,
                explorationProfile
            });
            
            var calculatedTotalCostCostProfile = new CalculatedTotalCostCostProfile
            {
                StartYear = totalCost.StartYear,
                Values = totalCost.Values,
            };

            caseItem.CalculatedTotalCostCostProfile = calculatedTotalCostCostProfile;

            return totalCost;
        }

        public async Task<TimeSeries<double>> CalculateTotalOffshoreFacilityCostAsync(Case caseItem)
        {
            var substructure = await _substructureRepository.GetSubstructure(caseItem.SubstructureLink);
            var surf = await _surfRepository.GetSurf(caseItem.SurfLink);
            var topside = await _topsideRepository.GetTopside(caseItem.TopsideLink);
            var transport = await _transportRepository.GetTransport(caseItem.TransportLink);

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

            var totalOffshoreFacilityCost = TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>>
            {
                substructureProfile,
                surfProfile,
                topsideProfile,
                transportProfile
            });

            return totalOffshoreFacilityCost;
        }


        public async Task<TimeSeries<double>> CalculateTotalDevelopmentCostAsync(Case caseItem)
        {
            var wellProject = await _wellProjectRepository.GetWellProject(caseItem.WellProjectLink);

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

            var totalDevelopmentCost = TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>>
            {
                oilProducerProfile,
                gasProducerProfile,
                waterInjectorProfile,
                gasInjectorProfile
            });

            return totalDevelopmentCost;
        }

        public async Task<TimeSeries<double>> CalculateTotalExplorationCostAsync(Case caseItem)
        {
            var explorationCost = await _explorationRepository.GetExploration(caseItem.ExplorationLink);

            TimeSeries<double> gAndGAdminCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> seismicAcquisitionAndProcessingProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> countryOfficeCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> explorationWellCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> appraisalWellCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> sidetrackCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };

            if (explorationCost?.GAndGAdminCostOverride?.Override == true)
            {
                gAndGAdminCostProfile = new TimeSeries<double>
                {
                    StartYear = explorationCost.GAndGAdminCostOverride.StartYear,
                    Values = explorationCost.GAndGAdminCostOverride.Values ?? Array.Empty<double>()
                };
            }
            else if (explorationCost?.GAndGAdminCost != null)
            {
                gAndGAdminCostProfile = new TimeSeries<double>
                {
                    StartYear = explorationCost.GAndGAdminCost.StartYear,
                    Values = explorationCost.GAndGAdminCost.Values ?? Array.Empty<double>()
                };
            }

            if (explorationCost?.SeismicAcquisitionAndProcessing != null)
            {
                seismicAcquisitionAndProcessingProfile = new TimeSeries<double>
                {
                    StartYear = explorationCost.SeismicAcquisitionAndProcessing.StartYear,
                    Values = explorationCost.SeismicAcquisitionAndProcessing.Values ?? Array.Empty<double>()
                };
            }

            if (explorationCost?.CountryOfficeCost != null)
            {
                countryOfficeCostProfile = new TimeSeries<double>
                {
                    StartYear = explorationCost.CountryOfficeCost.StartYear,
                    Values = explorationCost.CountryOfficeCost.Values ?? Array.Empty<double>()
                };
            }

            if (explorationCost?.ExplorationWellCostProfile != null)
            {
                explorationWellCostProfile = new TimeSeries<double>
                {
                    StartYear = explorationCost.ExplorationWellCostProfile.StartYear,
                    Values = explorationCost.ExplorationWellCostProfile.Values ?? Array.Empty<double>()
                };
            }

            if (explorationCost?.AppraisalWellCostProfile != null)
            {
                appraisalWellCostProfile = new TimeSeries<double>
                {
                    StartYear = explorationCost.AppraisalWellCostProfile.StartYear,
                    Values = explorationCost.AppraisalWellCostProfile.Values ?? Array.Empty<double>()
                };
            }

            if (explorationCost?.SidetrackCostProfile != null)
            {
                sidetrackCostProfile = new TimeSeries<double>
                {
                    StartYear = explorationCost.SidetrackCostProfile.StartYear,
                    Values = explorationCost.SidetrackCostProfile.Values ?? Array.Empty<double>()
                };
            }

            var totalExplorationCost = TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>>
            {
                gAndGAdminCostProfile,
                seismicAcquisitionAndProcessingProfile,
                countryOfficeCostProfile,
                explorationWellCostProfile,
                appraisalWellCostProfile,
                sidetrackCostProfile
            });

            return totalExplorationCost;
        }

        public TimeSeries<double> CalculateCashFlow(TimeSeries<double> income, TimeSeries<double> totalCost)
        {
            var startYear = Math.Min(income.StartYear, totalCost.StartYear);
            var endYear = Math.Max(
                income.StartYear + income.Values.Length - 1,
                totalCost.StartYear + totalCost.Values.Length - 1
            );

            var incomeValues = new double[endYear - startYear + 1];
            var costValues = new double[endYear - startYear + 1];

            for (int i = 0; i < income.Values.Length; i++)
            {
                int yearIndex = income.StartYear + i - startYear;
                incomeValues[yearIndex] = income.Values[i];
            }

            for (int i = 0; i < totalCost.Values.Length; i++)
            {
                int yearIndex = totalCost.StartYear + i - startYear;
                costValues[yearIndex] = totalCost.Values[i];
            }

            var cashFlowValues = new double[incomeValues.Length];
            for (int i = 0; i < cashFlowValues.Length; i++)
            {
                cashFlowValues[i] = incomeValues[i] - costValues[i];
            }

            return new TimeSeries<double>
            {
                StartYear = startYear,
                Values = cashFlowValues
            };
        }




        public async Task<TimeSeries<double>> CalculateProjectCashFlowAsync(Case caseItem, DrainageStrategy drainageStrategy)
        {
            var project = await _projectService.GetProject(caseItem.ProjectId);
            var income = CalculateIncome(drainageStrategy, project, caseItem);
            var totalCost = await CalculateTotalCostAsync(caseItem);
            var cashFlow = CalculateCashFlow(income, totalCost);
            return cashFlow;
        }
    }
}
