using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public static class CaseAdapter
    {
        public static Case Convert(CaseDto caseDto)
        {
            return new Case
            {
                Id = caseDto.Id,
                ProjectId = caseDto.ProjectId,
                Name = caseDto.Name,
                Description = caseDto.Description,
                ReferenceCase = caseDto.ReferenceCase,
                DG0Date = caseDto.DG0Date,
                DG1Date = caseDto.DG1Date,
                DG2Date = caseDto.DG2Date,
                DG3Date = caseDto.DG3Date,
                DG4Date = caseDto.DG4Date,
                CreateTime = caseDto.CreateTime,
                ModifyTime = caseDto.ModifyTime,
                DrainageStrategyLink = caseDto.DrainageStrategyLink,
                ExplorationLink = caseDto.ExplorationLink,
                WellProjectLink = caseDto.WellProjectLink,
                SurfLink = caseDto.SurfLink,
                TopsideLink = caseDto.TopsideLink,
                SubstructureLink = caseDto.SubstructureLink,
                TransportLink = caseDto.TransportLink,
                ArtificialLift = caseDto.ArtificialLift,
                ProductionStrategyOverview = caseDto.ProductionStrategyOverview,
                ProducerCount = caseDto.ProducerCount,
                GasInjectorCount = caseDto.GasInjectorCount,
                WaterInjectorCount = caseDto.WaterInjectorCount,
                FacilitiesAvailability = caseDto.FacilitiesAvailability,
                Well = caseDto.Well
            };
        }
    }
}
