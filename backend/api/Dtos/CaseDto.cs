using api.Models;

using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Identity.Client;

namespace api.Dtos
{

    public class CaseDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool ReferenceCase { get; set; }
        public ArtificialLift ArtificialLift { get; set; }
        public int ProducerCount { get; set; }
        public int GasInjectorCount { get; set; }
        public int WaterInjectorCount { get; set; }
        public double RigMobDemob { get; set; }
        public DateTimeOffset DG1Date { get; set; }
        public DateTimeOffset DG2Date { get; set; }
        public DateTimeOffset DG3Date { get; set; }
        public DateTimeOffset DG4Date { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        public DateTimeOffset ModifyTime { get; set; }
        public Guid DrainageStrategyLink { get; set; }
        public Guid WellProjectLink { get; set; }
        public Guid SurfLink { get; set; }
        public Guid SubstructureLink { get; set; }
        public Guid TopsideLink { get; set; }
        public Guid TransportLink { get; set; }
        public Guid ExplorationLink { get; set; }
        public double Capex { get; set; }
    }
}
