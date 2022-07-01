using System.Security.Principal;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class WellCaseService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;
        private readonly ILogger<CaseService> _logger;

        public WellCaseService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _projectService = projectService;
            _logger = loggerFactory.CreateLogger<CaseService>();
        }

        public ProjectDto CreateWellCase(WellCaseDto wellCaseDto)
        {
            var wellCase = WellCaseAdapter.Convert(wellCaseDto);
            // var well = _context.Wells!.FirstOrDefault(w => w.Id == wellCase.WellId);
            // var caseItem = _context.Cases!.FirstOrDefault(c => c.Id == wellCase.CaseId);
            // wellCase.Case = caseItem;
            // wellCase.Well = well;
            _context.WellCase!.Add(wellCase);
            _context.SaveChanges();
            return _projectService.GetProjectDto(new Guid("5a74a716-92e5-4ec8-8b5b-281381509ae3"));
        }

        public ProjectDto? UpdateWellCase(WellCaseDto updatedWellCaseDto)
        {
            return null;
            // var existing = GetWell(updatedWellDto.Id);
            // WellAdapter.ConvertExisting(existing, updatedWellDto);
            // _context.Wells!.Update(existing);
            // _context.SaveChanges();
            // return _projectService.GetProjectDto(existing.ProjectId);
        }

        public WellCase GetWellCase(Guid wellId, Guid caseId)
        {
            var wellCase = _context.WellCase!
                        // .Include(w => w.Well)
                        // .Include(w => w.Case)
                        .FirstOrDefault(w => w.WellId == wellId && w.CaseId == caseId);
            if (wellCase == null)
            {
                throw new ArgumentException(string.Format("WellCase {0} not found.", wellId));
            }
            return wellCase;
        }

        public WellCaseDto GetWellCaseDto(Guid wellId, Guid caseId)
        {
            var wellCase = GetWellCase(wellId, caseId);
            var wellCaseDto = WellCaseDtoAdapter.Convert(wellCase);

            return wellCaseDto;
        }

        public IEnumerable<WellCase> GetAll()
        {
            if (_context.WellCase != null)
            {
                return _context.WellCase;
            }
            else
            {
                _logger.LogInformation("No WellCases existing");
                return new List<WellCase>();
            }
        }

        // public IEnumerable<WellDto> GetDtosForProject(Guid projectId)
        // {
        //     var wells = GetWellCases(projectId);
        //     var wellsDtos = new List<WellDto>();
        //     foreach (Well well in wells)
        //     {
        //         wellsDtos.Add(WellDtoAdapter.Convert(well));
        //     }
        //     return wellsDtos;
        // }

        // public IEnumerable<Well> GetWellCases(Guid projectId)
        // {
        //     if (_context.Wells != null)
        //     {
        //         return _context.Wells
        //             .Where(d => d.ProjectId.Equals(projectId));
        //     }
        //     else
        //     {
        //         return new List<Well>();
        //     }
        // }

        public IEnumerable<WellCaseDto> GetAllDtos()
        {
            var wellCases = GetAll();
            if (wellCases.Any())
            {
                var wellCaseDtos = new List<WellCaseDto>();
                foreach (WellCase wellCase in wellCases)
                {
                    var wellCaseDto = WellCaseDtoAdapter.Convert(wellCase);
                    wellCaseDtos.Add(wellCaseDto);
                }

                return wellCaseDtos;
            }
            else
            {
                return new List<WellCaseDto>();
            }
        }
    }
}
