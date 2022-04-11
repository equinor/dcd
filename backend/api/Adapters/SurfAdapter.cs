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

            if (surfDto.CostProfile != null)
            {
                surf.CostProfile = Convert(surfDto.CostProfile, surf);
            }

            if (surfDto.SurfCessationCostProfileDto != null)
            {
                surf.SurfCessationCostProfile = Convert(surfDto.SurfCessationCostProfileDto, surf);
            }

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
            existing.SurfCessationCostProfile = Convert(surfDto.SurfCessationCostProfileDto, existing);
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

        private static SurfCessationCostProfile? Convert(SurfCessationCostProfileDto? cessationCostProfileDto, Surf surf)
        {

            if (cessationCostProfileDto == null)
            {
                return null;
            }

            SurfCessationCostProfile surfCessationCostProfile = new SurfCessationCostProfile
            {
                Id = cessationCostProfileDto.Id,
                Currency = cessationCostProfileDto.Currency,
                EPAVersion = cessationCostProfileDto.EPAVersion,
                StartYear = cessationCostProfileDto.StartYear,
                Values = cessationCostProfileDto.Values,
                Surf = surf
            };

            return surfCessationCostProfile;
        }


    }
}
