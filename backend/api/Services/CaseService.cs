using api.Context;
using api.Models;

namespace api.Services
{
    public class CaseService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;

        public CaseService(DcdDbContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public Project CreateCase(Case case_)
        {
            var project = _projectService.GetProject(case_.ProjectId);
            case_.Project = project;
            _context.Cases!.Add(case_);
            _context.SaveChanges();
            return _projectService.GetProject(project.Id);
        }
    }

}
