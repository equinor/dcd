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
        private readonly ExplorationService _explorationService;
        private readonly ILogger<CaseService> _logger;

        public CaseService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory, ExplorationService explorationService)
        {
            _context = context;
            _projectService = projectService;
            _explorationService = explorationService;
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

        public ProjectDto GenerateGAndGAdminCost(Guid caseId)
        {
            var caseItem = GetCase(caseId);
            var exploration = _explorationService.GetExploration(caseItem.ExplorationLink);
            // Find linked exploration wells
            var linkedWells = exploration.ExplorationWells;
            // find earliest date for well
            if (linkedWells?.Count > 0)
            {
                var drillingSchedules = linkedWells.Select(lw => lw.DrillingSchedule);
                var earliestYear = drillingSchedules.Select(ds => ds?.StartYear)?.Min();
                // find DG1 date
                var dG1Date = caseItem.DG1Date;
                // Find country => cost per year
                var project = _projectService.GetProject(caseItem.ProjectId);
                var country = project.Country;
                var countryCost = MapCountry(country);
                // generate cost profile
                var lastYear = new DateTimeOffset(dG1Date.Year, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
                var lastYearMinutes = (dG1Date - lastYear).Minutes;

                var totalMinutesLastYear = new TimeSpan(365, 0, 0, 0).TotalMinutes;
                var percentageOfLastYear = lastYearMinutes / totalMinutesLastYear;

                var gAndGAdminCost = new GAndGAdminCost
                {
                    StartYear = (int)earliestYear
                };
                var years = lastYear.Year - (int)earliestYear;
                var values = new List<double>();
                for (int i = 0; i < years; i++) {
                    values = (List<double>)values.Append(countryCost);
                }
                values = (List<double>)values.Append(countryCost * percentageOfLastYear);
                gAndGAdminCost.Values = values.ToArray();
            }
            return _projectService.GetProjectDto(exploration.ProjectId);
        }

        private static double MapCountry(string country) {
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
