using api.Models;

namespace api.Features.Stea.Dtos;

public class SteaDbData
{
    public required Project Project { get; set; }
    public required List<DrainageStrategy> DrainageStrategies { get; set; }
    public required List<Exploration> Explorations { get; set; }
    public required List<OnshorePowerSupply> OnshorePowerSupplies { get; set; }
    public required List<Substructure> Substructures { get; set; }
    public required List<Surf> Surfs { get; set; }
    public required List<Topside> Topsides { get; set; }
    public required List<Transport> Transports { get; set; }
    public required List<Well> Wells { get; set; }
    public required List<WellProject> WellProjects { get; set; }
}
