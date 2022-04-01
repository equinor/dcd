using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public static class SurfDtoAdapter
    {

        public static SurfDto Convert(Surf surf)
        {
            var surfDto = new SurfDto
            {
                Id = surf.Id,
                ProjectId = surf.ProjectId,
                Name = surf.Name,
                ArtificialLift = surf.ArtificialLift,
                Maturity = surf.Maturity,
                InfieldPipelineSystemLength = surf.InfieldPipelineSystemLength,
                ProductionFlowline = surf.ProductionFlowline,
                RiserCount = surf.RiserCount,
                CostProfile = Convert(surf.CostProfile)
            };

            return surfDto;
        }

        private static SurfCostProfileDto? Convert(SurfCostProfile? costprofile)
        {

            if (costprofile == null)
            {
                return null;
            }

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
    }
}
