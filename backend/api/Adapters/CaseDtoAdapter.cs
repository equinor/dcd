using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public static class CaseDtoAdapter
    {
        public static CaseDto Convert(Case case_)
        {
            var caseDto = new CaseDto
            {
                Id = case_.Id,
                ProjectId = case_.ProjectId,
                Name = case_.Name,
                Description = case_.Description,
                ReferenceCase = case_.ReferenceCase,
                DG1Date = case_.DG1Date,
                DG2Date = case_.DG2Date,
                DG3Date = case_.DG3Date,
                DG4Date = case_.DG4Date,
                CreateTime = case_.CreateTime,
                ModifyTime = case_.ModifyTime,
                DrainageStrategyLink = case_.DrainageStrategyLink,
                WellProjectLink = case_.WellProjectLink,
                SurfLink = case_.SurfLink,
                SubstructureLink = case_.SubstructureLink,
                TopsideLink = case_.TopsideLink,
                TransportLink = case_.TransportLink,
                ExplorationLink = case_.ExplorationLink
            };

            return caseDto;
        }
    }
}
