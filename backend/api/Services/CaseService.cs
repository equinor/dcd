using api.Adapters;
using api.Context;
using api.Dtos;


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

        public ProjectDto CreateCase(CaseDto caseDto)
        {
            var case_ = CaseAdapter.Convert(caseDto);
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
