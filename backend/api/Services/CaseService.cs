using api.Context;
using api.Dtos;
using api.Models;

using Newtonsoft.Json;

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

        public ProjectDto CreateCase(Case case_)
        {
            var project = _projectService.GetProject(case_.ProjectId);
            case_.Project = project;
            _context.Cases!.Add(case_);
            _context.SaveChanges();
            return _projectService.GetProjectDto(project.Id);
        }

        public ProjectDto UpdateCase(Case updatedCase){
            _context.Cases!.Update(updatedCase);
            _context.SaveChanges();
            return _projectService.GetProjectDto(updatedCase.ProjectId);
        }

        public Case GetCase(Guid caseId)
        {
            var case_ = _context.Cases!.FirstOrDefault(o => o.Id == caseId);

            if ( case_ == null){
                throw new ArgumentException(string.Format("Case {0} not found.", caseId));
            }

            return case_;
        }
    }

}
