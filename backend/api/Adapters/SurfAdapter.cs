using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public class SurfAdapter
    {
        private readonly ISurfService _surfService = null!;

        public SurfAdapter(ISurfService surfService)
        {
            _surfService = surfService;
        }

        public Surf Convert(SurfDto surfDto)
        {
            var surf = new Surf();
            surf.ProjectId = surfDto.ProjectId;
            surf.Name = surfDto.Name;
            surf.ArtificialLift = surfDto.ArtificialLift;
            surf.Maturity = surfDto.Maturity;
            surf.InfieldPipelineSystemLength = surfDto.InfieldPipelineSystemLength;
            surf.ProductionFlowline = surfDto.ProductionFlowline;
            surf.RiserCount = surfDto.RiserCount;
            surf.CostProfile = Convert(surfDto.CostProfile, surf);
            surf.Id = surfDto.Id;
            return surf;
        }

        private SurfCostProfile Convert(SurfCostProfileDto costprofile, Surf surf)
        {
            var surfCostProfile = new SurfCostProfile();
            surfCostProfile.Currency = costprofile.Currency;
            surfCostProfile.EPAVersion = costprofile.EPAVersion;
            surfCostProfile.YearValues = costprofile.YearValues;
            surfCostProfile.Surf = surf;
            return surfCostProfile;
        }
    }
}
