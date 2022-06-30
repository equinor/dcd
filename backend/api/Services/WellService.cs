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

        public WellService(DcdDbContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public ProjectDto CreateWell(WellDto wellDto)
        {
            var _well = WellAdapter.Convert(wellDto);
            _context.Well!.Add(_well);
            _context.SaveChanges();
            return _projectService.GetProjectDto(wellDto.ProjectId);
        }

        public ProjectDto UpdateWell(WellDto updatedWellDto)
        {
            var existing = GetWell(updatedWellDto.Id);
            WellAdapter.ConvertExisting(existing, updatedWellDto);
            _context.Well!.Update(existing);
            _context.SaveChanges();
            return _projectService.GetProjectDto(existing.ProjectId);
        }

        public Well GetWell(Guid wellId)
        {
            var well = _context.Well!
                        .Include(w => w.WellTypes)
                        .Include(e => e.ExplorationWellTypes)
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
            if (_context.Well != null)
            {
                return _context.Well;
            }
            else
            {
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
            if (_context.Well != null)
            {
                return _context.Well
                    .Include(w => w.WellTypes)
                    .Include(e => e.ExplorationWellTypes)
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
