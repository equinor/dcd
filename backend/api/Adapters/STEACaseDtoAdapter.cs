using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public static class STEACaseDtoAdapter
    {
        public static STEACaseDto Convert(Case c, WellProjectService wellProjectService, SubstructureService substructureService,
        SurfService surfService, TopsideService topsideService, TransportService transportService, ExplorationService explorationService,
        DrainageStrategyService drainageStrategyService)
        {
            var sTEACaseDto = new STEACaseDto();
            sTEACaseDto.Name = c.Name;
            sTEACaseDto.Capex = new CapexDto();

            if (c.WellProjectLink != Guid.Empty)
            {
                sTEACaseDto.Capex.Drilling = WellProjectDtoAdapter.Convert(wellProjectService.GetWellProject(c.WellProjectLink).CostProfile);
            }
            sTEACaseDto.Capex.OffshoreFacilities = new OffshoreFacilitiesDtoBuilder()
                                        .WithSubstructure(substructureService, c.SubstructureLink)
                                        .WithSurf(surfService, c.SurfLink)
                                        .WithTopside(topsideService, c.TopsideLink)
                                        .WithTransport(transportService, c.TransportLink)
                                        .Build();
            if (c.ExplorationLink != Guid.Empty)
            {
                sTEACaseDto.Exploration = ExplorationDtoAdapter.Convert(explorationService.GetExploration(c.ExplorationLink)).CostProfile;
            }
            if (c.DrainageStrategyLink != Guid.Empty)
            {
                sTEACaseDto.ProductionAndSalesVolumes = new ProductionAndSalesVolumesDto();
                sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil = DrainageStrategyDtoAdapter.Convert(drainageStrategyService.GetDrainageStrategy(c.DrainageStrategyLink)).ProductionProfileOil;
                sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = DrainageStrategyDtoAdapter.Convert(drainageStrategyService.GetDrainageStrategy(c.DrainageStrategyLink)).NetSalesGas;
                sTEACaseDto.ProductionAndSalesVolumes.Co2Emissions = DrainageStrategyDtoAdapter.Convert(drainageStrategyService.GetDrainageStrategy(c.DrainageStrategyLink)).Co2Emissions;
            }
            return sTEACaseDto;
        }
    }
}
