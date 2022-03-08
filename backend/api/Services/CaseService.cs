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

        public ProjectDto UpdateCase(Guid caseId, Case updatedCase){
            var case_ = GetCase(caseId);
            CopyData(case_, updatedCase);
            _context.Cases!.Update(case_);
            _context.SaveChanges();
            return _projectService.GetProjectDto(case_.ProjectId);
        }

        private static void CopyData(Case case_, Case updatedCase)
        {
            case_.Name = updatedCase.Name;
            case_.Description = updatedCase.Description;
            case_.ReferenceCase = updatedCase.ReferenceCase;
            case_.DG4Date = updatedCase.DG4Date;
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
