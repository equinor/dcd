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
        private readonly ILogger<CaseService> _logger;

        public WellProjectWellService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _projectService = projectService;
            _logger = loggerFactory.CreateLogger<CaseService>();
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
            _context.WellProjectWell!.Update(existing);
            _context.SaveChanges();
            var projectId = _context.WellProjects!.FirstOrDefault(c => c.Id == updatedWellProjectWellDto.WellProjectId)?.ProjectId;
            if (projectId != null)
            {
                return _projectService.GetProjectDto((Guid)projectId);
            }
            throw new NotFoundInDBException();
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
