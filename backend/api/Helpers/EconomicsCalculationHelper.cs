using api.Dtos;
using api.Models;
using api.Repositories;
using api.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class EconomicsCalculationHelper
    {
        private const int Cd = 365; // Capitalized constant name for consistency

        private readonly IStudyCostProfileService _studyCostProfileService;
        private readonly IOpexCostProfileService _opexCostProfileService;
        private readonly ICessationCostProfileService _cessationCostProfileService;
        private readonly IWellCostProfileService _wellCostProfileService; // Ensure this is the correct service
        private readonly IExplorationRepository _explorationRepository;
        private readonly ISubstructureService _substructureService;
        private readonly ISubstructureRepository _substructureRepository;

        private readonly ISurfRepository _surfRepository;
        private readonly ITopsideRepository _topsideRepository;
        private readonly ITransportRepository _transportRepository;
        private readonly IWellProjectRepository _wellProjectRepository; // Add this if it exists

        // Constructor with dependency injection
        public EconomicsCalculationHelper(
            IStudyCostProfileService studyCostProfileService,
            IOpexCostProfileService opexCostProfileService,
            ICessationCostProfileService cessationCostProfileService,
            IWellCostProfileService wellCostProfileService,
            IExplorationRepository explorationRepository,
            ISubstructureService substructureService,
            ISubstructureRepository substructureRepository,
            ISurfRepository surfRepository,
            ITopsideRepository topsideRepository,
            ITransportRepository transportRepository,
            IWellProjectRepository wellProjectRepository) // Add this to the constructor
        {
            _studyCostProfileService = studyCostProfileService ?? throw new ArgumentNullException(nameof(studyCostProfileService));
            _opexCostProfileService = opexCostProfileService ?? throw new ArgumentNullException(nameof(opexCostProfileService));
            _cessationCostProfileService = cessationCostProfileService ?? throw new ArgumentNullException(nameof(cessationCostProfileService));
            _wellCostProfileService = wellCostProfileService ?? throw new ArgumentNullException(nameof(wellCostProfileService));
            _explorationRepository = explorationRepository ?? throw new ArgumentNullException(nameof(explorationRepository));
            _substructureService = substructureService ?? throw new ArgumentNullException(nameof(substructureService));
            _substructureRepository = substructureRepository ?? throw new ArgumentNullException(nameof(substructureRepository));
            _surfRepository = surfRepository ?? throw new ArgumentNullException(nameof(surfRepository));
            _topsideRepository = topsideRepository ?? throw new ArgumentNullException(nameof(topsideRepository));
            _transportRepository = transportRepository ?? throw new ArgumentNullException(nameof(transportRepository));
            _wellProjectRepository = wellProjectRepository ?? throw new ArgumentNullException(nameof(wellProjectRepository)); // Initialize this
        }

        private static TimeSeries<double> CalculateIncome(DrainageStrategy drainageStrategy, double oilPrice, double gasPrice, double exchangeRate)
        {

            // Calculate income from oil
            var oilProfile = drainageStrategy.ProductionProfileOil?.Values ?? Array.Empty<double>();
            var additionalOilProfile = drainageStrategy.AdditionalProductionProfileOil?.Values ?? Array.Empty<double>();

            var productionProfileOil = new TimeSeries<double>
            {
                StartYear = drainageStrategy.ProductionProfileOil?.StartYear ?? 0,
                Values = oilProfile
            };

            var additionalProductionProfileOil = new TimeSeries<double>
            {
                StartYear = drainageStrategy.AdditionalProductionProfileOil?.StartYear ?? 0,
                Values = additionalOilProfile
            };

            var totalOilProduction = TimeSeriesCost.MergeCostProfiles(productionProfileOil, additionalProductionProfileOil);
            var oilIncome = new TimeSeries<double>
            {
                StartYear = totalOilProduction.StartYear,
                Values = totalOilProduction.Values.Select(v => v * oilPrice).ToArray()
            };

            // Calculate income from gas
            var gasProfile = drainageStrategy.ProductionProfileGas?.Values ?? Array.Empty<double>();
            var additionalGasProfile = drainageStrategy.AdditionalProductionProfileGas?.Values ?? Array.Empty<double>();

            var productionProfileGas = new TimeSeries<double>
            {
                StartYear = drainageStrategy.ProductionProfileGas?.StartYear ?? 0,
                Values = gasProfile
            };

            var additionalProductionProfileGas = new TimeSeries<double>
            {
                StartYear = drainageStrategy.AdditionalProductionProfileGas?.StartYear ?? 0,
                Values = additionalGasProfile
            };

            var totalGasProduction = TimeSeriesCost.MergeCostProfiles(productionProfileGas, additionalProductionProfileGas);
            var gasIncome = new TimeSeries<double>
            {
                StartYear = totalGasProduction.StartYear,
                Values = totalGasProduction.Values.Select(v => v * gasPrice * exchangeRate).ToArray()
            };

            // Merge the income profiles
            var totalIncome = TimeSeriesCost.MergeCostProfiles(oilIncome, gasIncome);

            return totalIncome;
        }

        public async Task<TimeSeries<double>> CalculateTotalCostAsync(Case caseItem)
        {
            // Calculate total study cost
            var totalStudyCost = await _studyCostProfileService.Generate(caseItem.Id);
            var studiesProfile = new TimeSeries<double>
            {
                StartYear = totalStudyCost.StudyCostProfileDto?.StartYear ?? 0,
                Values = totalStudyCost.StudyCostProfileDto?.Values ?? Array.Empty<double>()
            };

            // Calculate total Opex cost
            var totalOpexCost = await _opexCostProfileService.Generate(caseItem.Id);
            var opexProfile = new TimeSeries<double>
            {
                StartYear = totalOpexCost.OpexCostProfileDto?.StartYear ?? 0,
                Values = totalOpexCost.OpexCostProfileDto?.Values ?? Array.Empty<double>()
            };

            // Calculate total Cessation cost
            var totalCessationCost = await _cessationCostProfileService.Generate(caseItem.Id);
            var cessationProfile = new TimeSeries<double>
            {
                StartYear = totalCessationCost.CessationCostDto?.StartYear ?? 0,
                Values = totalCessationCost.CessationCostDto?.Values ?? Array.Empty<double>()
            };

            // Calculate total Offshore facility cost
            var totalOffshoreFacilityCost = await CalculateTotalOffshoreFacilityCostAsync(caseItem);
            var totalOffshoreFacilityProfile = new TimeSeries<double>
            {
                StartYear = totalOffshoreFacilityCost.StartYear,
                Values = totalOffshoreFacilityCost.Values
            };

            // Calculate total Development cost
            var totalDevelopmentCost = await CalculateTotalDevelopmentCostAsync(caseItem);
            var developmentProfile = new TimeSeries<double>
            {
                StartYear = totalDevelopmentCost.StartYear,
                Values = totalDevelopmentCost.Values
            };

            // Calculate total Exploration cost
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

            return totalCost;
        }

        public async Task<TimeSeries<double>> CalculateTotalOffshoreFacilityCostAsync(Case caseItem)
        {
            // Retrieve cost profiles for substructure, surf, topside, and transportation
            //var substructure = await _substructureService.GetSubstructure(caseItem.SubstructureLink);
            var substructure = await _substructureRepository.GetSubstructure(caseItem.SubstructureLink);
            var surf = await _surfRepository.GetSurf(caseItem.SurfLink);
            var topside = await _topsideRepository.GetTopside(caseItem.TopsideLink);
            var transport = await _transportRepository.GetTransport(caseItem.TransportLink);

            // Initialize profile variables
            TimeSeries<double> substructureProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> surfProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> topsideProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> transportProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };

            // Extract and assign profiles with overrides considered
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

            // Merge the cost profiles
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
            // Retrieve cost profiles for oil producer, gas producer, water injector, and gas injector
            var wellProject = await _wellProjectRepository.GetWellProject(caseItem.WellProjectLink);

            // Initialize profile variables
            TimeSeries<double> oilProducerProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> gasProducerProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> waterInjectorProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> gasInjectorProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };

            // Extract and assign profiles with overrides considered
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

            // Merge the cost profiles
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
            // Retrieve all relevant profiles through the explorationCost object
            var explorationCost = await _explorationRepository.GetExploration(caseItem.ExplorationLink);

            // Initialize profile variables
            TimeSeries<double> gAndGAdminCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> seismicAcquisitionAndProcessingProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> countryOfficeCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> explorationWellCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> appraisalWellCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };
            TimeSeries<double> sidetrackCostProfile = new TimeSeries<double> { StartYear = 0, Values = Array.Empty<double>() };

            // Extract and assign profiles with overrides considered
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

            // Merge the cost profiles
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



        // private static TimeSeries<double> CalculateCashFlow(TimeSeries<double> income, TimeSeries<double> totalCost)
        // {
        //     var cashFlowProfile = TimeSeriesCost.MergeCostProfiles(income, totalCost);

        //     var cashFlowValues= new TimeSeries<double>
        //     {
        //         StartYear = cashFlowProfile.StartYear,
        //         Values = cashFlowProfile.Values
        //     };
        //     return cashFlowValues;

        // }

        private static TimeSeries<double> CalculateCashFlow(TimeSeries<double> income, TimeSeries<double> totalCost)
        {
            // Determine the common start and end years
            var startYear = Math.Min(income.StartYear, totalCost.StartYear);
            var endYear = Math.Max(
                income.StartYear + income.Values.Length - 1,
                totalCost.StartYear + totalCost.Values.Length - 1
            );

            // Initialize arrays for the aligned data
            var incomeValues = new double[endYear - startYear + 1];
            var costValues = new double[endYear - startYear + 1];

            // Fill income values
            for (int i = 0; i < income.Values.Length; i++)
            {
                int yearIndex = income.StartYear + i - startYear;
                incomeValues[yearIndex] = income.Values[i];
            }

            // Fill cost values
            for (int i = 0; i < totalCost.Values.Length; i++)
            {
                int yearIndex = totalCost.StartYear + i - startYear;
                costValues[yearIndex] = totalCost.Values[i];
            }

            // Calculate cash flow by subtracting costs from income
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




        public async Task<TimeSeries<double>> CalculateProjectCashFlowAsync(Case caseItem, DrainageStrategy drainageStrategy, double oilPrice, double gasPrice, double exchangeRate)
        {
            var income = CalculateIncome(drainageStrategy, oilPrice, gasPrice, exchangeRate);
            var totalCost = await CalculateTotalCostAsync(caseItem);
            var cashFlow = CalculateCashFlow(income, totalCost);
            return cashFlow;
        }
    }
}
