using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class SubstructureDtoAdapter
    {
        public static SubstructureDto Convert(Substructure substructure)
        {
            var substructureDto = new SubstructureDto
            {
                Id = substructure.Id,
                ProjectId = substructure.ProjectId,
                Name = substructure.Name,
                DryWeight = substructure.DryWeight,
                Maturity = substructure.Maturity,
                CostProfile = Convert(substructure.CostProfile),
                SubstructureCessasionCostProfileDto = Convert(substructure.SubstructureCessasionCostProfile)
            };
            return substructureDto;
        }

        private static SubstructureCostProfileDto? Convert(SubstructureCostProfile? costProfile)
        {
            if (costProfile == null)
            {
                return null;
            }
            var substructureCostProfile = new SubstructureCostProfileDto
            {
                Id = costProfile.Id,
                EPAVersion = costProfile.EPAVersion,
                Currency = costProfile.Currency,
                StartYear = costProfile.StartYear,
                Values = costProfile.Values
            };
            return substructureCostProfile;
        }

        private static SubstructureCessasionCostProfileDto? Convert(SubstructureCessasionCostProfile? substructureCessasionCostProfile)
        {
            if (substructureCessasionCostProfile == null)
            {
                return null;
            }
            SubstructureCessasionCostProfileDto substructureCessasionCostProfileDto = new SubstructureCessasionCostProfileDto
            {
                Id = substructureCessasionCostProfile.Id,
                EPAVersion = substructureCessasionCostProfile.EPAVersion,
                Currency = substructureCessasionCostProfile.Currency,
                StartYear = substructureCessasionCostProfile.StartYear,
                Values = substructureCessasionCostProfile.Values
            };
            return substructureCessasionCostProfileDto;
        }
    }
}
