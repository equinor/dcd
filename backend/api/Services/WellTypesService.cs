using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class WellTypesService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;

        public WellTypesService(DcdDbContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public ProjectDto CreateWellType(WellTypeDto wellTypeDto)
        {
            // WellTypeAdapter needs to be implemented
            var _wellType = WellTypeAdapter.Convert(wellTypeDto);
            _context.WellTypes!.Add(_wellType);
            _context.SaveChanges();
            // projectId might need to be put on welltype and welltypedto
            return _projectService.GetProjectDto(wellTypeDto.ProjectId);
        }

        public ProjectDto UpdateWellType(WellTypeDto updatedWellTypeDto)
        {
            var existing = GetWellType(updatedWellTypeDto.Id);
            // WellTypeAdapter needs to be implemented
            WellTypeAdapter.ConvertExisting(existing, updatedWellDto);
            _context.WellTypes!.Update(existing);
            _context.SaveChanges();
            // projectId might need to be put on welltype and welltypedto
            return _projectService.GetProjectDto(existing.ProjectId);
        }

        public WellType GetWellType(Guid wellTypeId)
        {
            var wellType = _context.WellTypes!
                        .FirstOrDefault(w => w.Id == wellTypeId);
            if (wellType == null)
            {
                throw new ArgumentException(string.Format("Well {0} not found.", wellTypeId));
            }
            return wellType;
        }

        public WellTypeDto GetWellDto(Guid wellTypeId)
        {
            var wellType = GetWellType(wellTypeId);
            // WellTypeDtoAdapter needs to be implemented
            var wellTypeDto = WellTypeDtoAdapter.Convert(wellType);

            return wellTypeDto;
        }

        public IEnumerable<WellType> GetAll()
        {
            if (_context.WellTypes != null)
            {
                return _context.WellTypes;
            }
            else
            {
                return new List<WellType>();
            }
        }

        public IEnumerable<WellTypeDto> GetDtosForProject(Guid projectId)
        {
            var wellTypes = GetWells(projectId);
            var wellTypesDtos = new List<WellTypeDto>();
            foreach (WellType wellType in wellTypes)
            {
                // WellTypeDtoAdapter needs to be implemented
                wellTypesDtos.Add(WellTypeDtoAdapter.Convert(wellType));
            }
            return wellTypesDtos;
        }

        public IEnumerable<WellType> GetWells(Guid projectId)
        {
            if (_context.Well != null)
            {
                return _context.WellTypes
                // projectId might need to be put on welltype and welltypedto
                    .Where(d => d.ProjectId.Equals(projectId));
            }
            else
            {
                return new List<WellType>();
            }
        }

        public IEnumerable<WellTypeDto> GetAllDtos()
        {
            if (GetAll().Any())
            {
                var wellTypes = GetAll();
                var wellTypesDtos = new List<WellTypeDto>();
                foreach (WellType wellType in wellTypes)
                {
                    // WellTypeDtoAdapter needs to be implemented
                    var wellDto = WellTypeDtoAdapter.Convert(wellType);
                    wellTypesDtos.Add(wellDto);
                }

                return wellTypesDtos;
            }
            else
            {
                return new List<WellTypeDto>();
            }
        }
    }
}
