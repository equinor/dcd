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

            if (surfDto.CostProfile != null) {
                surf.CostProfile = Convert(surfDto.CostProfile, surf);
            }

            if (surfDto.cessationCostProfile != null) {
                surf.cessationCostProfile = Convert(surfDto.cessationCostProfile, surf);
            }

            return surf;
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

         private static SurfCessationCostProfile? Convert(SurfCessationCostProfileDto? cessationCostProfileDto, Surf surf) {

             if (cessationCostProfileDto == null) {
                 return null;
             }   

             var surfCessationCostProfile = new SurfCessationCostProfile 
             {
                 Id = cessationCostProfileDto.Id,
                 Currency = cessationCostProfileDto.Currency,
                 EPAVersion = cessationCostProfileDto.EPAversion,
                 StartYear = cessationCostProfileDto.StartYear,
                 Values = cessationCostProfileDto.values,
                 Surf = surf
             }

             return surfCessationCostProfile;
        }

       
    }
}
