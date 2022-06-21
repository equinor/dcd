using api.Adapters;
using api.Context;
using api.Dtos;

namespace api.Services
{
    public class WellService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;
        private readonly ILogger<CaseService> _logger;

        public WellService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _projectService = projectService;
            _logger = loggerFactory.CreateLogger<CaseService>();
        }

        public ProjectDto CreateWell(WellDto wellDto)
        {
            var _well = WellAdapter.Convert(wellDto);
            var project = _projectService.GetProject(_well.ProjectId);
            _well.Project = project;
            _context.Wells!.Add(_well);
            _context.SaveChanges();
            return _projectService.GetProjectDto(project.Id);
        }

        public ProjectDto UpdateWell(WellDto updatedWellDto)
        {
            var updatedWell = WellAdapter.Convert(updatedWellDto);
            _context.Wells!.Update(updatedWell);
            _context.SaveChanges();
            return _projectService.GetProjectDto(updatedWell.ProjectId);
        }
    }
}
