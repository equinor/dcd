using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class SubstructureDtoAdapter
    {
        public static SubstructureDto Convert(Substructure substructure)
        {
            var substructureDto = new SubstructureDto();
            substructureDto.Id = substructure.Id;
            substructureDto.ProjectId = substructure.ProjectId;
            substructureDto.Name = substructure.Name;
            substructureDto.DryWeight = substructure.DryWeight;
            substructureDto.Maturity = substructure.Maturity;
            substructureDto.CostProfile = Convert(substructure.CostProfile);

            return substructureDto;
        }

        public static SubstructureCostProfileDto Convert(SubstructureCostProfile substructureCostProfile)
        {
            return new SubstructureCostProfileDto
            {
                EPAVersion = substructureCostProfile.EPAVersion,
                Currency = substructureCostProfile.Currency,
                StartYear = substructureCostProfile.StartYear,
                Values = substructureCostProfile.Values
            };
        }
    }
}
