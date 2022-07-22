using System.Security.Principal;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class WellProjectWellService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;
        private readonly WellProjectService _wellProjectService;
        private readonly ILogger<CaseService> _logger;

        public WellProjectWellService(DcdDbContext context, ProjectService projectService, WellProjectService wellProjectService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _projectService = projectService;
            _logger = loggerFactory.CreateLogger<CaseService>();
            _wellProjectService = wellProjectService;
        }

        public ProjectDto CreateWellProjectWell(WellProjectWellDto wellProjectWellDto)
        {
            var wellProjectWell = WellProjectWellAdapter.Convert(wellProjectWellDto);
            _context.WellProjectWell!.Add(wellProjectWell);
            _context.SaveChanges();
            var projectId = _context.WellProjects!.FirstOrDefault(c => c.Id == wellProjectWellDto.WellProjectId)?.ProjectId;
            if (projectId != null)
            {
                return _projectService.GetProjectDto((Guid)projectId);
            }
            throw new NotFoundInDBException();
        }

        public ProjectDto UpdateWellProjectWell(WellProjectWellDto updatedWellProjectWellDto)
        {
            var existing = GetWellProjectWell(updatedWellProjectWellDto.WellId, updatedWellProjectWellDto.WellProjectId);
            WellProjectWellAdapter.ConvertExisting(existing, updatedWellProjectWellDto);
            if (updatedWellProjectWellDto.DrillingSchedule == null && existing.DrillingSchedule != null)
            {
                _context.DrillingSchedule!.Remove(existing.DrillingSchedule);
            }

            // Generate wellproject costprofile
            var wellProject = _context.WellProjects!.Include(wp => wp.CostProfile).Include(wp => wp.WellProjectWells).ThenInclude(wpw => wpw.DrillingSchedule).FirstOrDefault(wp => wp.Id == existing.WellProjectId);
            if (wellProject?.CostProfile?.Override != true)
            {
                var project = _context.Projects!.Include(p => p.Wells).FirstOrDefault(p => p.Id == wellProject.ProjectId);
                if (wellProject.WellProjectWells != null && project?.Wells != null
                    && (wellProject.WellProjectWells.Any(wpw => wpw.DrillingSchedule != null
                    && wpw.WellId != updatedWellProjectWellDto.WellId) || updatedWellProjectWellDto.DrillingSchedule != null))
                {
                    GenerateCostProfileFromDrillingSchedules(wellProject, wellProject.WellProjectWells.ToList(), project.Wells.ToList());
                    var wellProjectDto = WellProjectDtoAdapter.Convert(wellProject);
                    _wellProjectService.UpdateWellProject(wellProjectDto);
                }
                else if (wellProject.WellProjectWells != null && project?.Wells != null)
                {
                    wellProject.CostProfile = null;
                    var wellProjectDto = WellProjectDtoAdapter.Convert(wellProject);
                    _wellProjectService.UpdateWellProject(wellProjectDto);
                }
            }

            _context.WellProjectWell!.Update(existing);
            _context.SaveChanges();
            var projectId = _context.WellProjects!.FirstOrDefault(c => c.Id == updatedWellProjectWellDto.WellProjectId)?.ProjectId;
            if (projectId != null)
            {
                return _projectService.GetProjectDto((Guid)projectId);
            }
            throw new NotFoundInDBException();
        }

        private static WellProjectCostProfile? MergeCostProfiles(WellProjectCostProfile t1, WellProjectCostProfile t2)
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

            var values = new List<double>();
            if (t1Year < t2Year)
            {
                values = MergeTimeSeries(t1Values.ToList(), t2Values.ToList(), offset);
            }
            else
            {
                values = MergeTimeSeries(t2Values.ToList(), t1Values.ToList(), offset);
            }

            var timeSeries = new WellProjectCostProfile();
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

        public void GenerateCostProfileFromDrillingSchedules(WellProject wellProject, List<WellProjectWell> wellProjectWells, List<Well> wells)
        {
            var costProfile = new WellProjectCostProfile();
            var costProfiles = new List<WellProjectCostProfile>();
            foreach (var wellProjectWell in wellProjectWells)
            {
                if (wellProjectWell.DrillingSchedule == null) { continue; }

                var well = wells.Find(w => w.Id == wellProjectWell.WellId);
                if (well == null) { continue; }

                var wellCostProfile = new WellProjectCostProfile();
                wellCostProfile.StartYear = wellProjectWell.DrillingSchedule.StartYear;
                var values = new List<double>();
                foreach (var value in wellProjectWell.DrillingSchedule.Values)
                {
                    values.Add(value * well.WellCost);
                }
                wellCostProfile.Values = values.ToArray();
                costProfiles.Add(wellCostProfile);
            }

            var tempCostProfile = new WellProjectCostProfile();
            foreach (var profile in costProfiles)
            {
                tempCostProfile = MergeCostProfiles(tempCostProfile, profile);
            }

            costProfile = tempCostProfile;
            if (wellProject.CostProfile != null)
            {
                wellProject.CostProfile.StartYear = costProfile.StartYear;
                wellProject.CostProfile.Values = costProfile.Values;
            }
            else
            {
                wellProject.CostProfile = costProfile;
            }
        }

        public WellProjectWell GetWellProjectWell(Guid wellId, Guid caseId)
        {
            var wellProjectWell = _context.WellProjectWell!
                        .Include(wpw => wpw.DrillingSchedule)
                        .FirstOrDefault(w => w.WellId == wellId && w.WellProjectId == caseId);
            if (wellProjectWell == null)
            {
                throw new ArgumentException(string.Format("WellProjectWell {0} not found.", wellId));
            }
            return wellProjectWell;
        }

        public WellProjectWellDto GetWellProjectWellDto(Guid wellId, Guid caseId)
        {
            var wellProjectWell = GetWellProjectWell(wellId, caseId);
            var wellProjectWellDto = WellProjectWellDtoAdapter.Convert(wellProjectWell);

            return wellProjectWellDto;
        }

        public IEnumerable<WellProjectWell> GetAll()
        {
            if (_context.WellProjectWell != null)
            {
                return _context.WellProjectWell;
            }
            else
            {
                _logger.LogInformation("No WellProjectWells existing");
                return new List<WellProjectWell>();
            }
        }

        public IEnumerable<WellProjectWellDto> GetAllDtos()
        {
            var wellProjectWells = GetAll();
            if (wellProjectWells.Any())
            {
                var wellProjectWellDtos = new List<WellProjectWellDto>();
                foreach (WellProjectWell wellProjectWell in wellProjectWells)
                {
                    var wellProjectWellDto = WellProjectWellDtoAdapter.Convert(wellProjectWell);
                    wellProjectWellDtos.Add(wellProjectWellDto);
                }

                return wellProjectWellDtos;
            }
            else
            {
                return new List<WellProjectWellDto>();
            }
        }
    }
}
