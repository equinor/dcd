using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public static class STEAProjectDtoAdapter
    {
        public static STEAProjectDto Convert(Project project, WellProjectService wellProjectService, SubstructureService substructureService,
                                            SurfService surfService, TopsideService topsideService, TransportService transportService,
                                            ExplorationService explorationService, DrainageStrategyService drainageStrategyService)
        {
            var sTEAprojectDto = new STEAProjectDto();
            sTEAprojectDto.Name = project.Name;
            sTEAprojectDto.STEACases = new List<STEACaseDto>();
            foreach (Case c in project.Cases)
            {
                sTEAprojectDto.STEACases.Add(STEACaseDtoAdapter.Convert(c, wellProjectService, substructureService, surfService, topsideService, transportService, explorationService, drainageStrategyService));
                Console.WriteLine("Case added " + c.Name);
            }
            return sTEAprojectDto;
        }
    }
}
