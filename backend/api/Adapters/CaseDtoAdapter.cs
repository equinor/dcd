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
            caseDto.Id = case_.Id;
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

        public static void AddCapexToCases(ICollection<CaseDto> caseDtos, WellProjectService wellProjectService, SubstructureService substructureService,
        SurfService surfService, TopsideService topsideService, TransportService transportService, ExplorationService explorationService)
        {
            foreach (CaseDto c in caseDtos)
            {
                c.Capex = 0;
                if (c.WellProjectLink != Guid.Empty)
                {
                    var wellProject = wellProjectService.GetWellProject(c.WellProjectLink);
                    c.Capex += sumValues(wellProject.CostProfile);
                }
                if (c.SubstructureLink != Guid.Empty)
                {
                    var substructure = substructureService.GetSubstructure(c.SubstructureLink);
                    c.Capex += sumValues(substructure.CostProfile);
                }
                if (c.SurfLink != Guid.Empty)
                {
                    var surf = surfService.GetSurf(c.SurfLink);
                    c.Capex += sumValues(surf.CostProfile);
                }
                if (c.TopsideLink != Guid.Empty)
                {
                    var topside = topsideService.GetTopside(c.TopsideLink);
                    c.Capex += sumValues(topside.CostProfile);
                }
                if (c.TransportLink != Guid.Empty)
                {
                    var transport = transportService.GetTransport(c.TransportLink);
                    c.Capex += sumValues(transport.CostProfile);
                }
                if (c.ExplorationLink != Guid.Empty)
                {
                    var exploration = explorationService.GetExploration(c.ExplorationLink);
                    c.Capex += sumValues(exploration.CostProfile);
                }
            }
        }
        private static double sumValues(TimeSeriesCost<double> timeSeries)
        {
            double sum = 0;
            foreach (YearValue<double> yearValue in timeSeries.YearValues)
            {
                sum += yearValue.Value;
            }
            return sum;
        }
    }
}
