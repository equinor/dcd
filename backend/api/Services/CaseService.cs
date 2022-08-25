using System.Collections.Immutable;
using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using DocumentFormat.OpenXml.InkML;

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
                case_.DG4Date = new DateTimeOffset(2030, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
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
                case_.DG4Date = new DateTimeOffset(2030, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
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
            }
            var lastYear = drainageStrategy?.ProductionProfileOil == null ? 0 : drainageStrategy.ProductionProfileOil.StartYear + drainageStrategy.ProductionProfileOil.Values.Length;

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

            var wellInterventionCosts = new TimeSeries<double>();
            foreach (var wi in linkedWells)
            {
                if (wi.DrillingSchedule == null) { continue; }

                var timeSeries = new TimeSeries<double>();
                timeSeries.StartYear = wi.DrillingSchedule.StartYear;
                timeSeries.Values = wi.DrillingSchedule.Values.Select(v => (double)v).ToArray();
                wellInterventionCosts = TimeSeriesCost.MergeCostProfiles(wellInterventionCosts, timeSeries);
            }

            var tempSeries = new TimeSeries<int>
            {
                StartYear = wellInterventionCosts.StartYear,
                Values = wellInterventionCosts.Values.Select(v => (int)v).ToArray()
            };
            var cumulativeDrillingSchedule = GetCumulativeDrillingSchedule(tempSeries);
            cumulativeDrillingSchedule.StartYear = tempSeries.StartYear;

            var interventionCost = wellProject.AnnualWellInterventionCost;

            var wellInterventionCostValues = cumulativeDrillingSchedule.Values.Select(v => v * interventionCost).ToArray();

            wellInterventionCosts.Values = wellInterventionCostValues;
            wellInterventionCosts.StartYear = cumulativeDrillingSchedule.StartYear;

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

        public double SumAllCostFacility(Guid caseId)
        {
            var caseItem = GetCase(caseId);

            var sumFacilityCost = 0.0;

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

            return sumFacilityCost;
        }

        public double SumWellCost(Guid caseId)
        {
            var caseItem = GetCase(caseId);

            var sumWellCost = 0.0;
            var wellProjectService = (WellProjectService?)_serviceProvider.GetService(typeof(WellProjectService));
            if (wellProjectService != null)
            {
                var wellProject = new WellProject();
                try
                {
                    wellProject = wellProjectService.GetWellProject(caseItem.WellProjectLink);
                    if (wellProject?.CostProfile != null)
                    {
                        sumWellCost = wellProject.CostProfile.Values.Sum();
                    }
                }
                catch (ArgumentException)
                {
                    _logger.LogInformation("WellProject {0} not found.", caseItem.WellProjectLink);
                }
            }

            return sumWellCost;
        }

        public TimeSeries<double> CalculateTotalFeasibilityAndConceptStudies(Guid caseId)
        {
            var caseItem = GetCase(caseId);

            var sumFacilityCost = SumAllCostFacility(caseId);
            var sumWellCost = SumWellCost(caseId);

            var totalFeasibilityAndConceptStudies = (sumFacilityCost + sumWellCost) * caseItem.CapexFactorFeasibilityStudies;

            var dg0 = caseItem.DG0Date;
            var dg2 = caseItem.DG2Date;

            if (dg0.Year == 1 || dg2.Year == 1) { return new TimeSeries<double>(); }
            if (dg2.DayOfYear == 1) { dg2 = dg2.AddDays(-1); } // Treat the 1st of January as the 31st of December

            var totalDays = (dg2 - dg0).Days + 1;

            var firstYearDays = (new DateTimeOffset(dg0.Year, 12, 31, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero) - dg0).Days + 1;
            var firstYearPercentage = firstYearDays / (double)totalDays;

            var lastYearDays = dg2.DayOfYear;
            var lastYearPercentage = lastYearDays / (double)totalDays;

            var percentageOfYearList = new List<double>();
            percentageOfYearList.Add(firstYearPercentage);
            for (int i = dg0.Year + 1; i < dg2.Year; i++)
            {
                var days = DateTime.IsLeapYear(i) ? 366 : 365;
                var percentage = days / (double)totalDays;
                percentageOfYearList.Add(percentage);
            }
            percentageOfYearList.Add(lastYearPercentage);

            var valuesList = percentageOfYearList.ConvertAll(x => x * totalFeasibilityAndConceptStudies);

            var feasibilityAndConceptStudiesCost = new TimeSeries<double>
            {
                StartYear = dg0.Year - caseItem.DG4Date.Year,
                Values = valuesList.ToArray()
            };

            return feasibilityAndConceptStudiesCost;
        }

        public TimeSeries<double> CalculateTotalFEEDStudies(Guid caseId)
        {
            var caseItem = GetCase(caseId);

            var sumFacilityCost = SumAllCostFacility(caseId);
            var sumWellCost = SumWellCost(caseId);

            var totalFeasibilityAndConceptStudies = (sumFacilityCost + sumWellCost) * caseItem.CapexFactorFEEDStudies;

            var dg2 = caseItem.DG2Date;
            var dg3 = caseItem.DG3Date;

            if (dg2.Year == 1 || dg3.Year == 1) { return new TimeSeries<double>(); }
            if (dg3.DayOfYear == 1) { dg3 = dg3.AddDays(-1); } // Treat the 1st of January as the 31st of December

            var totalDays = (dg3 - dg2).Days + 1;

            var firstYearDays = (new DateTimeOffset(dg2.Year, 12, 31, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero) - dg2).Days + 1;
            var firstYearPercentage = firstYearDays / (double)totalDays;

            var lastYearDays = dg3.DayOfYear;
            var lastYearPercentage = lastYearDays / (double)totalDays;

            var percentageOfYearList = new List<double>();
            percentageOfYearList.Add(firstYearPercentage);
            for (int i = dg2.Year + 1; i < dg3.Year; i++)
            {
                var days = DateTime.IsLeapYear(i) ? 366 : 365;
                var percentage = days / (double)totalDays;
                percentageOfYearList.Add(percentage);
            }
            percentageOfYearList.Add(lastYearPercentage);

            var valuesList = percentageOfYearList.ConvertAll(x => x * totalFeasibilityAndConceptStudies);

            var feasibilityAndConceptStudiesCost = new TimeSeries<double>
            {
                StartYear = dg2.Year - caseItem.DG4Date.Year,
                Values = valuesList.ToArray()
            };

            return feasibilityAndConceptStudiesCost;
        }

        public StudyCostProfileDto CalculateStudyCost(Guid caseId)
        {
            var feasibility = CalculateTotalFeasibilityAndConceptStudies(caseId);
            var feed = CalculateTotalFEEDStudies(caseId);

            if (feasibility == null && feed == null)
            {
                return new StudyCostProfileDto();
            }
            var cost = TimeSeriesCost.MergeCostProfiles(feasibility, feed);
            if (cost == null) { return new StudyCostProfileDto(); }
            var studyCost = new StudyCostProfile
            {
                StartYear = cost.StartYear,
                Values = cost.Values
            };
            var dto = CaseDtoAdapter.Convert(studyCost);
            return dto;
        }
    }
}
