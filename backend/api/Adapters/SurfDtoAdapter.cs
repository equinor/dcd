using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public static class SurfDtoAdapter
    {

        public static SurfDto Convert(Surf surf_)
        {
            var surfDto = new SurfDto
            {
                Id = surf_.Id,
                ProjectId = surf_.ProjectId,
                Name = surf_.Name,
                ArtificialLift = surf_.ArtificialLift,
                Maturity = surf_.Maturity,
                InfieldPipelineSystemLength = surf_.InfieldPipelineSystemLength,
                ProductionFlowline = surf_.ProductionFlowline,
                RiserCount = surf_.RiserCount,
                CostProfile = Convert(surf_.CostProfile)
            };
            surfDto.Id = surf_.Id;
            return surfDto;
        }

        private static SurfCostProfileDto? Convert(SurfCostProfile? costprofile)
        {

            if (costprofile != null)
            {
                var surfCostProfile = new SurfCostProfileDto
                {
                    Id = costprofile.Id,
                    Currency = costprofile.Currency,
                    EPAVersion = costprofile.EPAVersion,
                    Values = costprofile.Values,
                    StartYear = costprofile.StartYear
                };
                return surfCostProfile;
            }
            return null;

        }
    }
}
