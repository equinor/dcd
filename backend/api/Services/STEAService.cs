using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

namespace api.Services
{
    public class STEAService
    {
        private readonly WellProjectService _wellProjectService;
        private readonly DrainageStrategyService _drainageStrategyService;
        private readonly SurfService _surfService;
        private readonly SubstructureService _substructureService;
        private readonly TopsideService _topsideService;
        private readonly TransportService _transportService;
        private readonly ExplorationService _explorationService;
        private readonly CaseService _caseService;
        private readonly ProjectService _projectService;


        public STEAService(DcdDbContext context)
        {
            _projectService = new ProjectService(context);
            _wellProjectService = new WellProjectService(context, _projectService);
            _drainageStrategyService = new DrainageStrategyService(context, _projectService);
            _surfService = new SurfService(context, _projectService);
            _substructureService = new SubstructureService(context, _projectService);
            _topsideService = new TopsideService(context, _projectService);
            _caseService = new CaseService(context, _projectService);
            _explorationService = new ExplorationService(context, _projectService);
            _transportService = new TransportService(context, _projectService);
        }

        public STEAProjectDto GetInputToSTEA(Guid ProjectId)
        {
            var project = _projectService.GetProject(ProjectId);

            return STEAProjectDtoAdapter.Convert(project, _wellProjectService, _substructureService, _surfService,
             _topsideService, _transportService, _explorationService, _drainageStrategyService);
        }
    }
}
