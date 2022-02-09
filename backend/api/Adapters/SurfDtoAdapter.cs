using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public static class SurfDtoAdapter
    {

        public static SurfDto Convert(Surf surf_)
        {
            var surfDto = new SurfDto();
            surfDto.Id = surf_.Id;
            surfDto.ProjectId = surf_.ProjectId;
            surfDto.Name = surf_.Name;
            surfDto.ArtificialLift = surf_.ArtificialLift;
            surfDto.Maturity = surf_.Maturity;
            surfDto.InfieldPipelineSystemLength = surf_.InfieldPipelineSystemLength;
            surfDto.ProductionFlowline = surf_.ProductionFlowline;
            surfDto.RiserCount = surf_.RiserCount;
            surfDto.CostProfile = Convert(surf_.CostProfile);
            surfDto.Id = surf_.Id;
            return surfDto;
        }

        private static SurfCostProfileDto Convert(SurfCostProfile? costprofile)
        {
            var surfCostProfile = new SurfCostProfileDto();
            if (costprofile != null)
            {
                surfCostProfile.Currency = costprofile.Currency;
                surfCostProfile.EPAVersion = costprofile.EPAVersion;
                surfCostProfile.YearValues = costprofile.YearValues;
            }
            return surfCostProfile;
        }
    }
}
