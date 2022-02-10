using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public class SurfAdapter
    {

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
            return surf;
        }

        private SurfCostProfile Convert(SurfCostProfileDto costprofile, Surf surf)
        {
            var surfCostProfile = new SurfCostProfile();
            surfCostProfile.Currency = costprofile.Currency;
            surfCostProfile.EPAVersion = costprofile.EPAVersion;
            surfCostProfile.StartYear = costprofile.StartYear;
            surfCostProfile.Values = costprofile.Values;
            surfCostProfile.Surf = surf;
            return surfCostProfile;
        }
    }
}
