using api.Dtos;

namespace api.Adapters
{
    public static class STEACaseDtoBuilder
    {
        public static STEACaseDto Build(CaseDto c, ProjectDto p)
        {
            var sTEACaseDto = new STEACaseDto();
            sTEACaseDto.Name = c.Name;
            sTEACaseDto.Capex = new CapexDto();

            if (c.WellProjectLink != Guid.Empty)
            {
                sTEACaseDto.Capex.Drilling = p.WellProjects.FirstOrDefault(l => l.Id == c.WellProjectLink)!.CostProfile;
            }
            sTEACaseDto.Capex.OffshoreFacilities = new OffshoreFacilitiesDtoBuilder()
                                        .WithSubstructure(p, c.SubstructureLink)
                                        .WithSurf(p, c.SurfLink)
                                        .WithTopside(p, c.TopsideLink)
                                        .WithTransport(p, c.TransportLink)
                                        .Build();
            if (c.ExplorationLink != Guid.Empty)
            {
                sTEACaseDto.Exploration = p.Explorations.FirstOrDefault(e => e.Id == c.ExplorationLink)!.CostProfile;
            }
            if (c.DrainageStrategyLink != Guid.Empty)
            {
                sTEACaseDto.ProductionAndSalesVolumes = new ProductionAndSalesVolumesDto();
                DrainageStrategyDto drainageStrategyDto = p.DrainageStrategies.FirstOrDefault(d => d.Id == c.DrainageStrategyLink)!;
                sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil = drainageStrategyDto.ProductionProfileOil;
                sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = drainageStrategyDto.NetSalesGas;
                sTEACaseDto.ProductionAndSalesVolumes.Co2Emissions = drainageStrategyDto.Co2Emissions;
            }
            return sTEACaseDto;
        }
    }
}
