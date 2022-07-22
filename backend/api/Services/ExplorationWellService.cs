using System.Security.Principal;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class ExplorationWellService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;
        private readonly ExplorationService _explorationService;
        private readonly ILogger<CaseService> _logger;

        public ExplorationWellService(DcdDbContext context, ProjectService projectService, ExplorationService explorationService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _projectService = projectService;
            _logger = loggerFactory.CreateLogger<CaseService>();
            _explorationService = explorationService;
        }

        public ProjectDto CreateExplorationWell(ExplorationWellDto explorationWellDto)
        {
            var explorationWell = ExplorationWellAdapter.Convert(explorationWellDto);
            _context.ExplorationWell!.Add(explorationWell);
            _context.SaveChanges();
            var projectId = _context.Explorations!.FirstOrDefault(c => c.Id == explorationWellDto.ExplorationId)?.ProjectId;
            if (projectId != null)
            {
                return _projectService.GetProjectDto((Guid)projectId);
            }
            throw new NotFoundInDBException();
        }

        public ProjectDto UpdateExplorationWell(ExplorationWellDto updatedExplorationWellDto)
        {
            var existing = GetExplorationWell(updatedExplorationWellDto.WellId, updatedExplorationWellDto.ExplorationId);
            ExplorationWellAdapter.ConvertExisting(existing, updatedExplorationWellDto);
            if (updatedExplorationWellDto.DrillingSchedule == null && existing.DrillingSchedule != null)
            {
                _context.DrillingSchedule!.Remove(existing.DrillingSchedule);
            }

            // Generate exploration costprofile
            var exploration = _context.Explorations!.Include(wp => wp.CostProfile).Include(wp => wp.ExplorationWells).ThenInclude(wpw => wpw.DrillingSchedule).FirstOrDefault(wp => wp.Id == existing.ExplorationId);
            if (exploration?.CostProfile?.Override != true)
            {
                var project = _context.Projects!.Include(p => p.Wells).FirstOrDefault(p => p.Id == exploration.ProjectId);
                if (exploration.ExplorationWells != null && project?.Wells != null
                    && (exploration.ExplorationWells.Any(wpw => wpw.DrillingSchedule != null
                    && wpw.WellId != updatedExplorationWellDto.WellId) || updatedExplorationWellDto.DrillingSchedule != null))
                {
                    GenerateCostProfileFromDrillingSchedules(exploration, exploration.ExplorationWells.ToList(), project.Wells.ToList());
                    var explorationDto = ExplorationDtoAdapter.Convert(exploration);
                    _explorationService.UpdateExploration(explorationDto);
                }
                else if (exploration.ExplorationWells != null && project?.Wells != null)
                {
                    exploration.CostProfile = null;
                    var explorationDto = ExplorationDtoAdapter.Convert(exploration);
                    _explorationService.UpdateExploration(explorationDto);
                }
            }

            _context.ExplorationWell!.Update(existing);
            _context.SaveChanges();
            var projectId = _context.Explorations!.FirstOrDefault(c => c.Id == updatedExplorationWellDto.ExplorationId)?.ProjectId;
            if (projectId != null)
            {
                return _projectService.GetProjectDto((Guid)projectId);
            }
            throw new NotFoundInDBException();
        }

        private static ExplorationCostProfile? MergeCostProfiles(ExplorationCostProfile t1, ExplorationCostProfile t2)
        {
            var t1Year = t1.StartYear;
            var t2Year = t2.StartYear;
            var t1Values = t1.Values;
            var t2Values = t2.Values;
            if (t1Values.Length == 0)
            {
                if (t2.Values.Length == 0)
                {
                    return null;
                }
                return t2;
            }
            if (t2Values.Length == 0)
            {
                return t1;
            }

            var offset = t1Year < t2Year ? t2Year - t1Year : t1Year - t2Year;

            List<double> values;
            if (t1Year < t2Year)
            {
                values = MergeTimeSeries(t1Values.ToList(), t2Values.ToList(), offset);
            }
            else
            {
                values = MergeTimeSeries(t2Values.ToList(), t1Values.ToList(), offset);
            }

            var timeSeries = new ExplorationCostProfile();
            timeSeries.StartYear = Math.Min(t1Year, t2Year);
            timeSeries.Values = values.ToArray();
            return timeSeries;
        }

        private static List<double> MergeTimeSeries(List<double> t1, List<double> t2, int offset)
        {
            var doubleList = new List<double>();
            if (offset > t1.Count)
            {
                doubleList.AddRange(t1);
                var zeros = offset - t1.Count;
                var zeroList = Enumerable.Repeat(0.0, zeros);
                doubleList.AddRange(zeroList);
                doubleList.AddRange(t2);
                return doubleList;
            }
            doubleList.AddRange(t1.Take(offset));
            if (t1.Count - offset == t2.Count)
            {
                doubleList.AddRange(t1.TakeLast(t1.Count - offset).Zip(t2, (x, y) => x + y));
            }
            else if (t1.Count - offset > t2.Count)
            {
                doubleList.AddRange(t1.TakeLast(t1.Count - offset).Zip(t2, (x, y) => x + y));
                var remaining = t1.Count - offset - t2.Count;
                doubleList.AddRange(t1.TakeLast(remaining));
            }
            else
            {
                doubleList.AddRange(t1.TakeLast(t1.Count - offset).Zip(t2, (x, y) => x + y));
                var remaining = t2.Count - (t1.Count - offset);
                doubleList.AddRange(t2.TakeLast(remaining));
            }
            return doubleList;
        }

        public void GenerateCostProfileFromDrillingSchedules(Exploration exploration, List<ExplorationWell> explorationWells, List<Well> wells)
        {
            ExplorationCostProfile costProfile;
            var costProfiles = new List<ExplorationCostProfile>();
            foreach (var explorationWell in explorationWells)
            {
                if (explorationWell.DrillingSchedule == null) { continue; }

                var well = wells.Find(w => w.Id == explorationWell.WellId);
                if (well == null) { continue; }

                var wellCostProfile = new ExplorationCostProfile();
                wellCostProfile.StartYear = explorationWell.DrillingSchedule.StartYear;
                var values = new List<double>();
                foreach (var value in explorationWell.DrillingSchedule.Values)
                {
                    values.Add(value * well.WellCost);
                }
                wellCostProfile.Values = values.ToArray();
                costProfiles.Add(wellCostProfile);
            }

            var tempCostProfile = new ExplorationCostProfile();
            foreach (var profile in costProfiles)
            {
                tempCostProfile = MergeCostProfiles(tempCostProfile, profile);
            }

            costProfile = tempCostProfile;
            if (exploration.CostProfile != null)
            {
                exploration.CostProfile.StartYear = costProfile.StartYear;
                exploration.CostProfile.Values = costProfile.Values;
            }
            else
            {
                exploration.CostProfile = costProfile;
            }
        }

        public ExplorationWell GetExplorationWell(Guid wellId, Guid caseId)
        {
            var explorationWell = _context.ExplorationWell!
                        .Include(wpw => wpw.DrillingSchedule)
                        .FirstOrDefault(w => w.WellId == wellId && w.ExplorationId == caseId);
            if (explorationWell == null)
            {
                throw new ArgumentException(string.Format("ExplorationWell {0} not found.", wellId));
            }
            return explorationWell;
        }

        public ExplorationWellDto GetExplorationWellDto(Guid wellId, Guid caseId)
        {
            var explorationWell = GetExplorationWell(wellId, caseId);
            var explorationWellDto = ExplorationWellDtoAdapter.Convert(explorationWell);

            return explorationWellDto;
        }

        public IEnumerable<ExplorationWell> GetAll()
        {
            if (_context.ExplorationWell != null)
            {
                return _context.ExplorationWell;
            }
            else
            {
                _logger.LogInformation("No ExplorationWells existing");
                return new List<ExplorationWell>();
            }
        }

        public IEnumerable<ExplorationWellDto> GetAllDtos()
        {
            var explorationWells = GetAll();
            if (explorationWells.Any())
            {
                var explorationWellDtos = new List<ExplorationWellDto>();
                foreach (ExplorationWell explorationWell in explorationWells)
                {
                    var explorationWellDto = ExplorationWellDtoAdapter.Convert(explorationWell);
                    explorationWellDtos.Add(explorationWellDto);
                }

                return explorationWellDtos;
            }
            else
            {
                return new List<ExplorationWellDto>();
            }
        }
    }
}
