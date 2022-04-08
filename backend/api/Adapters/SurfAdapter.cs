using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public static class SurfAdapter
    {

        public static Surf Convert(SurfDto surfDto)
        {
            var surf = new Surf
            {
                Id = surfDto.Id,
                ProjectId = surfDto.ProjectId,
                Name = surfDto.Name,
                ArtificialLift = surfDto.ArtificialLift,
                Maturity = surfDto.Maturity,
                InfieldPipelineSystemLength = surfDto.InfieldPipelineSystemLength,
                ProductionFlowline = surfDto.ProductionFlowline,
                RiserCount = surfDto.RiserCount
            };
            surf.CostProfile = Convert(surfDto.CostProfile, surf);
            return surf;
        }

        public static void ConvertExisting(Surf existing, SurfDto surfDto)
        {
            existing.Id = surfDto.Id;
            existing.ProjectId = surfDto.ProjectId;
            existing.Name = surfDto.Name;
            existing.ArtificialLift = surfDto.ArtificialLift;
            existing.Maturity = surfDto.Maturity;
            existing.InfieldPipelineSystemLength = surfDto.InfieldPipelineSystemLength;
            existing.ProductionFlowline = surfDto.ProductionFlowline;
            existing.RiserCount = surfDto.RiserCount;
            existing.CostProfile = Convert(surfDto.CostProfile, existing);
        }

        private static SurfCostProfile? Convert(SurfCostProfileDto? costprofile, Surf surf)
        {
            if (costprofile == null)
            {
                return null;
            }
            var surfCostProfile = new SurfCostProfile
            {
                Id = costprofile.Id,
                Currency = costprofile.Currency,
                EPAVersion = costprofile.EPAVersion,
                StartYear = costprofile.StartYear,
                Values = costprofile.Values,
                Surf = surf
            };
            return surfCostProfile;
        }
    }
}
