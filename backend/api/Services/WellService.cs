using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class WellService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;
        private readonly WellProjectService _wellProjectService;
        private readonly ExplorationService _explorationService;
        private readonly ILogger<CaseService> _logger;

        public WellService(DcdDbContext context, ProjectService projectService, WellProjectService wellProjectService, ExplorationService explorationService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _projectService = projectService;
            _logger = loggerFactory.CreateLogger<CaseService>();
            _wellProjectService = wellProjectService;
            _explorationService = explorationService;
        }

        public ProjectDto CreateWell(WellDto wellDto)
        {
            var _well = WellAdapter.Convert(wellDto);
            _context.Wells!.Add(_well);
            _context.SaveChanges();
            return _projectService.GetProjectDto(wellDto.ProjectId);
        }

        public ProjectDto UpdateWell(WellDto updatedWellDto)
        {
            var existing = GetWell(updatedWellDto.Id);
            var updateCostProfiles = existing.WellCost != updatedWellDto.WellCost;
            WellAdapter.ConvertExisting(existing, updatedWellDto);

            if (updateCostProfiles)
            {
                if (existing.WellProjectWells?.Count > 0)
                {
                    foreach (var wpw in existing.WellProjectWells)
                    {
                        var wellProject = _wellProjectService.GetWellProject(wpw.WellProjectId);
                        _wellProjectService.CalculateCostProfile(wellProject, wpw, existing);
                    }
                }
                else if (existing.ExplorationWells?.Count > 0)
                {
                    foreach (var ew in existing.ExplorationWells)
                    {
                        var exploration = _explorationService.GetExploration(ew.ExplorationId);
                        _explorationService.CalculateCostProfile(exploration, ew, existing);
                    }
                }
            }

            _context.Wells!.Update(existing);
            _context.SaveChanges();
            return _projectService.GetProjectDto(existing.ProjectId);
        }

        public Well GetWell(Guid wellId)
        {
            var well = _context.Wells!
                        .Include(e => e.WellProjectWells)
                        .Include(e => e.ExplorationWells)
                        .FirstOrDefault(w => w.Id == wellId);
            if (well == null)
            {
                throw new ArgumentException(string.Format("Well {0} not found.", wellId));
            }
            return well;
        }

        public WellDto GetWellDto(Guid wellId)
        {
            var well = GetWell(wellId);
            var wellDto = WellDtoAdapter.Convert(well);

            return wellDto;
        }

        public IEnumerable<Well> GetAll()
        {
            if (_context.Wells != null)
            {
                return _context.Wells;
            }
            else
            {
                _logger.LogInformation("No Wells existing");
                return new List<Well>();
            }
        }

        public IEnumerable<WellDto> GetDtosForProject(Guid projectId)
        {
            var wells = GetWells(projectId);
            var wellsDtos = new List<WellDto>();
            foreach (Well well in wells)
            {
                wellsDtos.Add(WellDtoAdapter.Convert(well));
            }
            return wellsDtos;
        }

        public IEnumerable<Well> GetWells(Guid projectId)
        {
            if (_context.Wells != null)
            {
                return _context.Wells
                    .Where(d => d.ProjectId.Equals(projectId));
            }
            else
            {
                return new List<Well>();
            }
        }

        public IEnumerable<WellDto> GetAllDtos()
        {
            if (GetAll().Any())
            {
                var wells = GetAll();
                var wellDtos = new List<WellDto>();
                foreach (Well well in wells)
                {
                    var wellDto = WellDtoAdapter.Convert(well);
                    wellDtos.Add(wellDto);
                }

                return wellDtos;
            }
            else
            {
                return new List<WellDto>();
            }
        }
    }
}
