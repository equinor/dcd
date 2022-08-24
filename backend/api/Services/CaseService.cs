using System.Collections.Immutable;
using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Azure;

namespace api.Services
{
    public class CaseService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;
        private readonly ILogger<CaseService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CaseService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            _context = context;
            _projectService = projectService;
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<CaseService>();
        }

        public ProjectDto CreateCase(CaseDto caseDto)
        {
            var case_ = CaseAdapter.Convert(caseDto);
            if (case_.DG4Date == DateTimeOffset.MinValue)
            {
                case_.DG4Date = new DateTime(2030, 1, 1);
            }
            var project = _projectService.GetProject(case_.ProjectId);
            case_.Project = project;
            _context.Cases!.Add(case_);
            _context.SaveChanges();
            return _projectService.GetProjectDto(project.Id);
        }

        public ProjectDto DuplicateCase(Guid caseId)
        {
            var case_ = GetCase(caseId);
            case_.Id = new Guid();
            if (case_.DG4Date == DateTimeOffset.MinValue)
            {
                case_.DG4Date = new DateTime(2030, 1, 1);
            }
            var project = _projectService.GetProject(case_.ProjectId);
            case_.Project = project;

            List<Case> duplicateCaseNames = new List<Case>();
            foreach (Case c in project.Cases!)
            {
                string copyNumber = c.Name.Substring(c.Name.Length - 1, 1);
                if (c.Name.Equals(case_.Name) || c.Name.Equals(case_.Name + " - copy #" + copyNumber))
                {
                    duplicateCaseNames.Add(c);
                }
            }
            case_.Name = case_.Name + " - copy #" + duplicateCaseNames.Count();
            _context.Cases!.Add(case_);
            _context.SaveChanges();
            return _projectService.GetProjectDto(project.Id);
        }

        public ProjectDto UpdateCase(CaseDto updatedCaseDto)
        {
            var updatedCase = CaseAdapter.Convert(updatedCaseDto);
            _context.Cases!.Update(updatedCase);
            _context.SaveChanges();
            return _projectService.GetProjectDto(updatedCase.ProjectId);
        }

        public ProjectDto DeleteCase(Guid caseId)
        {
            var caseItem = GetCase(caseId);
            _context.Cases!.Remove(caseItem);
            _context.SaveChanges();
            return _projectService.GetProjectDto(caseItem.ProjectId);
        }

        public Case GetCase(Guid caseId)
        {
            var caseItem = _context.Cases!
                .FirstOrDefault(c => c.Id == caseId);
            if (caseItem == null)
            {
                throw new NotFoundInDBException(string.Format("Case {0} not found.", caseId));
            }
            return caseItem;
        }

        public GAndGAdminCostDto GenerateGAndGAdminCost(Guid caseId)
        {
            var caseItem = GetCase(caseId);
            var explorationService = (ExplorationService?)_serviceProvider.GetService(typeof(ExplorationService));
            if (explorationService == null)
            {
                return new GAndGAdminCostDto();

            }
            Exploration exploration;
            try
            {
                exploration = explorationService.GetExploration(caseItem.ExplorationLink);
            }
            catch (ArgumentException)
            {
                _logger.LogInformation("Exploration {0} not found.", caseItem.ExplorationLink);
                return new GAndGAdminCostDto();
            }
            var linkedWells = exploration.ExplorationWells?.Where(ew => ew.Well.WellCategory == WellCategory.Exploration_Well).ToList();
            if (exploration != null && linkedWells?.Count > 0)
            {
                var drillingSchedules = linkedWells.Select(lw => lw.DrillingSchedule);
                var earliestYear = drillingSchedules.Select(ds => ds?.StartYear)?.Min() + caseItem.DG4Date.Year;
                var dG1Date = caseItem.DG1Date;
                if (earliestYear != null && dG1Date.Year >= earliestYear)
                {
                    var project = _projectService.GetProject(caseItem.ProjectId);
                    var country = project.Country;
                    var countryCost = MapCountry(country);
                    var lastYear = new DateTimeOffset(dG1Date.Year, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
                    var lastYearMinutes = (dG1Date - lastYear).TotalMinutes;

                    var totalMinutesLastYear = new TimeSpan(365, 0, 0, 0).TotalMinutes;
                    var percentageOfLastYear = lastYearMinutes / totalMinutesLastYear;

                    var gAndGAdminCost = new GAndGAdminCost
                    {
                        StartYear = (int)earliestYear - caseItem.DG4Date.Year
                    };
                    var years = lastYear.Year - (int)earliestYear;
                    var values = new List<double>();
                    for (int i = 0; i < years - 1; i++)
                    {
                        values.Add(countryCost);
                    }
                    values.Add(countryCost * percentageOfLastYear);
                    gAndGAdminCost.Values = values.ToArray();
                    return ExplorationDtoAdapter.Convert(gAndGAdminCost);
                }
            }
            return new GAndGAdminCostDto();
        }

        private static double MapCountry(string country)
        {
            return country switch
            {
                "NORWAY" => 1,
                "UK" => 1,
                "BRAZIL" => 3,
                "CANADA" => 3,
                "UNITED STATES" => 3,
                _ => 7.0,
            };
        }

        private static bool IsWellProjectWell(WellCategory wellCategory) => new[] {
            WellCategory.Oil_Producer,
            WellCategory.Gas_Producer,
            WellCategory.Water_Injector,
            WellCategory.Gas_Injector
        }.Contains(wellCategory);

        public TimeSeries<double> CalculateWellInterventionCostProfile(Guid caseId)
        {
            var caseItem = GetCase(caseId);

            var drainageStrategyService = (DrainageStrategyService?)_serviceProvider.GetService(typeof(DrainageStrategyService));
            if (drainageStrategyService == null)
            {
                return new TimeSeries<double>();
            }
            var drainageStrategy = new DrainageStrategy();
            try
            {
                drainageStrategy = drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
            }
            catch (ArgumentException)
            {
                _logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
                // return new TimeSeries<double>();
            }
            // if (drainageStrategy?.ProductionProfileOil == null) { return new TimeSeries<double>(); }
            var lastYear = drainageStrategy?.ProductionProfileOil == null ? 0 : drainageStrategy.ProductionProfileOil.StartYear + drainageStrategy.ProductionProfileOil.Values.Length;

            // Calculate cumulative number of wells drilled from drilling schedule well project

            var wellProjectService = (WellProjectService?)_serviceProvider.GetService(typeof(WellProjectService));
            if (wellProjectService == null)
            {
                return new TimeSeries<double>();
            }
            WellProject wellProject;
            try
            {
                wellProject = wellProjectService.GetWellProject(caseItem.WellProjectLink);
            }
            catch (ArgumentException)
            {
                _logger.LogInformation("WellProject {0} not found.", caseItem.WellProjectLink);
                return new TimeSeries<double>();
            }
            var linkedWells = wellProject.WellProjectWells?.Where(ew => IsWellProjectWell(ew.Well.WellCategory)).ToList();
            if (linkedWells == null) { return new TimeSeries<double>(); }
            // Calculate cumulative number of wells drilled from drilling schedule well project
            // Loop over drilling schedules, create cumulated schedules
            var wellInterventionCostList = new List<TimeSeries<double>>();
            foreach (var linkedWell in linkedWells)
            {
                if (linkedWell.DrillingSchedule == null) { continue; }
                var interventionCost = wellProject.AnnualWellInterventionCost; // linkedWell.Well.WellInterventionCost;
                var cumulativeSchedule = GetCumulativeDrillingSchedule(linkedWell.DrillingSchedule);
                // multiply cumulated schedules with well intervention cost
                // var wellInterventionCostValues = Array.ConvertAll(cumulativeSchedule.Values, x => x * interventionCost);
                var wellInterventionCostValues = cumulativeSchedule.Values.Select(v => v * interventionCost).ToArray();

                var wellInterventionCost = new TimeSeries<double>
                {
                    StartYear = linkedWell.DrillingSchedule.StartYear,
                    Values = wellInterventionCostValues
                };
                wellInterventionCostList.Add(wellInterventionCost);
            }

            // Calculate new cost profile on Case : Well intervention cost as: (well intervention cost) * (cummulative number of wells drilled)
            var wellInterventionCosts = new TimeSeries<double>();
            foreach (var wi in wellInterventionCostList)
            {
                wellInterventionCosts = TimeSeriesCost.MergeCostProfiles(wellInterventionCosts, wi);
            }

            var totalValuesCount = lastYear == 0 ? wellInterventionCosts.Values.Length : lastYear - wellInterventionCosts.StartYear;
            var additionalValuesCount = totalValuesCount - wellInterventionCosts.Values.Length;

            var additionalValues = new List<double>();
            for (int i = 0; i < additionalValuesCount; i++)
            {
                additionalValues.Add(wellInterventionCosts.Values.Last());
            }

            var valuesList = wellInterventionCosts.Values.ToList();
            valuesList.AddRange(additionalValues);

            wellInterventionCosts.Values = valuesList.ToArray();

            return wellInterventionCosts;
        }

        public TimeSeries<double> CalculateOffshoreFacilitiesOperationsCostProfile(Guid caseId)
        {
            var caseItem = GetCase(caseId);

            // Find first and last year of production, from production profile (First year should be DG4 year.
            var drainageStrategyService = (DrainageStrategyService?)_serviceProvider.GetService(typeof(DrainageStrategyService));
            if (drainageStrategyService == null)
            {
                return new TimeSeries<double>();
            }
            DrainageStrategy drainageStrategy;
            try
            {
                drainageStrategy = drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
            }
            catch (ArgumentException)
            {
                _logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
                return new TimeSeries<double>();
            }
            if (drainageStrategy?.ProductionProfileOil == null) { return new TimeSeries<double>(); }
            var firstYear = drainageStrategy.ProductionProfileOil.StartYear;
            var lastYear = drainageStrategy.ProductionProfileOil.StartYear + drainageStrategy.ProductionProfileOil.Values.Length;
            // From first year of production until last year of production cost per year is Facility opex from Topside facilities
            var topsideService = (TopsideService?)_serviceProvider.GetService(typeof(TopsideService));
            if (topsideService == null)
            {
                return new TimeSeries<double>();
            }
            Topside topside;
            try
            {
                topside = topsideService.GetTopside(caseItem.TopsideLink);
            }
            catch (ArgumentException)
            {
                _logger.LogInformation("Topside {0} not found.", caseItem.TopsideLink);
                return new TimeSeries<double>();
            }
            var facilityOpex = topside.FacilityOpex;
            var values = new List<double>
            {
                (facilityOpex - 1) / 8,
                (facilityOpex - 1) / 4,
                (facilityOpex - 1) / 2
            };

            for (int i = firstYear; i < lastYear; i++)
            {
                values.Add(facilityOpex);
            }
            // Pre-opex should be added to the same cost profile for three years before first year of produciton (T) : 
            // T - 1  = ((Facility opex) -1)/2
            // T - 2  = ((Facility opex) -1)/4
            // T - 3  = ((Facility opex) -1)/8

            var offshoreFacilitiesOperationsCost = new TimeSeries<double>
            {
                StartYear = firstYear - 3,
                Values = values.ToArray()
            };
            return offshoreFacilitiesOperationsCost;
        }

        public OpexCostProfileDto CalculateOPEX(Guid caseId)
        {
            var caseItem = GetCase(caseId);

            // Calculate cumulative number of wells drilled from drilling schedule well project

            var wellInterventionCost = CalculateWellInterventionCostProfile(caseId);

            var offshoreFacilitiesOperationsCost = CalculateOffshoreFacilitiesOperationsCostProfile(caseId);

            var OPEX = TimeSeriesCost.MergeCostProfiles(wellInterventionCost, offshoreFacilitiesOperationsCost);
            if (OPEX == null) { return new OpexCostProfileDto(); }
            var opexCostProfile = new OpexCostProfile();
            opexCostProfile.StartYear = OPEX.StartYear;
            opexCostProfile.Values = OPEX.Values;
            var opexDto = CaseDtoAdapter.Convert(opexCostProfile);
            return opexDto ?? new OpexCostProfileDto();
        }

        private TimeSeries<double> GetCumulativeDrillingSchedule(TimeSeries<int> drillingSchedule)
        {
            var cumulativeSchedule = new TimeSeries<double>();
            cumulativeSchedule.StartYear = drillingSchedule.StartYear;
            var values = new List<double>();
            var sum = 0.0;
            for (int i = 0; i < drillingSchedule.Values.Length; i++)
            {
                sum += drillingSchedule.Values[i];
                values.Add(sum);
            }

            cumulativeSchedule.Values = values.ToArray();

            return cumulativeSchedule;
        }

        public TimeSeries<double> CalculateTotalFeasibilityAndConceptStudies(Guid caseId)
        {
            // Calculate total feasibility and concept studies = (sum of all cost facility + well cost) * (Capex factor feasibility studies, use default)
            // Generate cost profile from DG0 and DG2 date from schedule. Divide cost per year weighted with number of days per year.
            var caseItem = GetCase(caseId);

            var sumFacilityCost = 0.0;
            // sum of all cost facility
            var substructureService = (SubstructureService?)_serviceProvider.GetService(typeof(SubstructureService));
            if (substructureService != null)
            {
                Substructure substructure;
                try
                {
                    substructure = substructureService.GetSubstructure(caseItem.SubstructureLink);
                    if (substructure.CostProfile != null)
                    {
                        sumFacilityCost += substructure.CostProfile.Values.Sum();
                    }
                }
                catch (ArgumentException)
                {
                    _logger.LogInformation("Substructure {0} not found.", caseItem.SubstructureLink);
                }
            }
            var surfService = (SurfService?)_serviceProvider.GetService(typeof(SurfService));
            if (surfService != null)
            {
                Surf surf;
                try
                {
                    surf = surfService.GetSurf(caseItem.SurfLink);
                    if (surf.CostProfile != null)
                    {
                        sumFacilityCost += surf.CostProfile.Values.Sum();
                    }
                }
                catch (ArgumentException)
                {
                    _logger.LogInformation("Surf {0} not found.", caseItem.SurfLink);
                }
            }
            var topsideService = (TopsideService?)_serviceProvider.GetService(typeof(TopsideService));
            if (topsideService != null)
            {
                Topside topside;
                try
                {
                    topside = topsideService.GetTopside(caseItem.TopsideLink);
                    if (topside.CostProfile != null)
                    {
                        sumFacilityCost += topside.CostProfile.Values.Sum();
                    }
                }
                catch (ArgumentException)
                {
                    _logger.LogInformation("Topside {0} not found.", caseItem.TopsideLink);
                }
            }
            var transportService = (TransportService?)_serviceProvider.GetService(typeof(TransportService));
            if (transportService != null)
            {
                Transport transport;
                try
                {
                    transport = transportService.GetTransport(caseItem.TransportLink);
                    if (transport.CostProfile != null)
                    {
                        sumFacilityCost += transport.CostProfile.Values.Sum();
                    }
                }
                catch (ArgumentException)
                {
                    _logger.LogInformation("Transport {0} not found.", caseItem.TransportLink);
                }
            }


            // well cost
            var sumWellCost = 0.0;
            var wellProjectService = (WellProjectService?)_serviceProvider.GetService(typeof(WellProjectService));
            if (wellProjectService != null)
            {
                WellProject wellProject;
                try
                {
                    wellProject = wellProjectService.GetWellProject(caseItem.WellProjectLink);
                    var linkedWells = wellProject.WellProjectWells?.Where(ew => IsWellProjectWell(ew.Well.WellCategory)).ToList();

                    if (linkedWells != null && linkedWells.Count > 0)
                    {
                        foreach (var linkedWell in linkedWells)
                        {
                            if (linkedWell.DrillingSchedule == null) { continue; }
                            var totalWells = linkedWell.DrillingSchedule.Values.Sum();
                            sumWellCost += totalWells * linkedWell.Well.WellCost;
                        }
                    }

                }
                catch (ArgumentException)
                {
                    _logger.LogInformation("WellProject {0} not found.", caseItem.WellProjectLink);
                }
            }

            var explorationService = (ExplorationService?)_serviceProvider.GetService(typeof(ExplorationService));
            if (explorationService != null)
            {
                Exploration exploration;
                try
                {
                    exploration = explorationService.GetExploration(caseItem.ExplorationLink);
                    var linkedWells = exploration.ExplorationWells?.Where(ew => !IsWellProjectWell(ew.Well.WellCategory)).ToList();

                    if (linkedWells != null && linkedWells.Count > 0)
                    {
                        foreach (var linkedWell in linkedWells)
                        {
                            if (linkedWell.DrillingSchedule == null) { continue; }
                            var totalWells = linkedWell.DrillingSchedule.Values.Sum();
                            sumWellCost += totalWells * linkedWell.Well.WellCost;
                        }
                    }

                }
                catch (ArgumentException)
                {
                    _logger.LogInformation("Exploration {0} not found.", caseItem.ExplorationLink);
                }
            }

            var totalFeasibilityAndConceptStudies = (sumFacilityCost + sumWellCost) * caseItem.CapexFactorFeasibilityStudies;



            // var linkedWells = wellProject.WellProjectWells?.Where(ew => IsWellProjectWell(ew.Well.WellCategory)).ToList();
            if (linkedWells == null) { return new TimeSeries<double>(); }
            // Calculate cumulative number of wells drilled from drilling schedule well project
            // Loop over drilling schedules, create cumulated schedules
            var wellInterventionCostList = new List<TimeSeries<double>>();
            foreach (var linkedWell in linkedWells)
            {
                if (linkedWell.DrillingSchedule == null) { continue; }
                var interventionCost = wellProject.AnnualWellInterventionCost; // linkedWell.Well.WellInterventionCost;
                var cumulativeSchedule = GetCumulativeDrillingSchedule(linkedWell.DrillingSchedule);
                // multiply cumulated schedules with well intervention cost
                // var wellInterventionCostValues = Array.ConvertAll(cumulativeSchedule.Values, x => x * interventionCost);
                var wellInterventionCostValues = cumulativeSchedule.Values.Select(v => v * interventionCost).ToArray();
                var wellInterventionCost = new TimeSeries<double>
                {
                    StartYear = linkedWell.DrillingSchedule.StartYear,
                    Values = wellInterventionCostValues
                };
                wellInterventionCostList.Add(wellInterventionCost);
            }

            // Calculate new cost profile on Case : Well intervention cost as: (well intervention cost) * (cummulative number of wells drilled)
            var wellInterventionCosts = new TimeSeries<double>();
            foreach (var wi in wellInterventionCostList)
            {
                wellInterventionCosts = TimeSeriesCost.MergeCostProfiles(wellInterventionCosts, wi);
            }

            return wellInterventionCosts;
        }

    }
}
