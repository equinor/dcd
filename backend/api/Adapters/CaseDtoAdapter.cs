using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public static class CaseDtoAdapter
    {
        public static CaseDto Convert(Case case_)
        {
            var caseDto = new CaseDto();
            caseDto.ProjectId = case_.ProjectId;
            caseDto.Name = case_.Name;
            caseDto.Description = case_.Description;
            caseDto.ReferenceCase = case_.ReferenceCase;
            caseDto.DG4Date = case_.DG4Date;
            caseDto.CreateTime = case_.CreateTime;
            caseDto.ModifyTime = case_.ModifyTime;
            caseDto.DrainageStrategyLink = case_.DrainageStrategyLink;
            caseDto.WellProjectLink = case_.WellProjectLink;
            caseDto.SurfLink = case_.SurfLink;
            caseDto.SubstructureLink = case_.SubstructureLink;
            caseDto.TopsideLink = case_.TopsideLink;
            caseDto.TransportLink = case_.TransportLink;
            caseDto.ExplorationLink = case_.ExplorationLink;

            return caseDto;
        }
    }
}
