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
                UmbilicalSystemLength = surf.UmbilicalSystemLength,
                ProductionFlowline = surf.ProductionFlowline,
                RiserCount = surf.RiserCount,
                TemplateCount = surf.TemplateCount,
                ProducerCount = surf.ProducerCount,
                GasInjectorCount = surf.GasInjectorCount,
                WaterInjectorCount = surf.WaterInjectorCount,
                Currency = surf.Currency,
                CostProfile = Convert(surf.CostProfile),
                LastChangedDate = surf.LastChangedDate,
                CostYear = surf.CostYear
            };

            if (surf.CostProfile != null)
            {
                surfDto.CostProfile = Convert(surf.CostProfile);
            }

            if (surf.CessationCostProfile != null)
            {
                surfDto.CessationCostProfile = Convert(surf.CessationCostProfile);
            }

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

        private static SurfCessationCostProfileDto? Convert(SurfCessationCostProfile? surfCessationCostProfile)
        {

            if (surfCessationCostProfile == null)
            {
                return null;
            }

            SurfCessationCostProfileDto surfCessationCostProfileDto = new SurfCessationCostProfileDto
            {
                Id = surfCessationCostProfile.Id,
                Currency = surfCessationCostProfile.Currency,
                EPAVersion = surfCessationCostProfile.EPAVersion,
                Values = surfCessationCostProfile.Values,
                StartYear = surfCessationCostProfile.StartYear
            };

            return surfCessationCostProfileDto;

        }
    }
}
