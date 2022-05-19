using api.Adapters;
using api.Context;
using api.Dtos;

namespace api.Services
{
    public class CaseService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;
        private readonly ILogger<CaseService> _logger;

        public CaseService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _projectService = projectService;
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

        public ProjectDto UpdateCase(CaseDto updatedCaseDto)
        {
            var updatedCase = CaseAdapter.Convert(updatedCaseDto);
            _context.Cases!.Update(updatedCase);
            _context.SaveChanges();
            return _projectService.GetProjectDto(updatedCase.ProjectId);
        }
    }
}
