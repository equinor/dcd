using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

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
            var explorationService = (ExplorationService)_serviceProvider.GetService(typeof(ExplorationService));
            var exploration = explorationService.GetExploration(caseItem.ExplorationLink);
            var linkedWells = exploration.ExplorationWells;
            if (linkedWells?.Count > 0)
            {
                var drillingSchedules = linkedWells.Select(lw => lw.DrillingSchedule);
                var earliestYear = drillingSchedules.Select(ds => ds?.StartYear)?.Min() + caseItem.DG4Date.Year;
                if (earliestYear != null)
                {
                    var dG1Date = caseItem.DG1Date;
                    var project = _projectService.GetProject(caseItem.ProjectId);
                    var country = project.Country;
                    var countryCost = MapCountry(country);
                    var lastYear = new DateTimeOffset(dG1Date.Year, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
                    var lastYearMinutes = (dG1Date - lastYear).TotalMinutes;

                    var totalMinutesLastYear = new TimeSpan(365, 0, 0, 0).TotalMinutes;
                    var percentageOfLastYear = lastYearMinutes / totalMinutesLastYear;

                    var gAndGAdminCost = new GAndGAdminCost
                    {
                        StartYear = (int)earliestYear
                    };
                    var years = lastYear.Year - (int)earliestYear;
                    var values = new List<double>();
                    for (int i = 0; i < years; i++)
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
                "USA" => 3,
                _ => 7.0,
            };
        }
    }
}
