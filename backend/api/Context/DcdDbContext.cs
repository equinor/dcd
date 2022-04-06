using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Context
{
    public class DcdDbContext : DbContext
    {
        public DcdDbContext(DbContextOptions<DcdDbContext> options) : base(options)
        {

        }
        public DbSet<Project>? Projects { get; set; }

        public DbSet<Case>? Cases { get; set; }

        public DbSet<Surf>? Surfs { get; set; }

        public DbSet<Substructure>? Substructures { get; set; }

        public DbSet<Topside>? Topsides { get; set; }

        public DbSet<Transport>? Transports { get; set; }

        public DbSet<DrainageStrategy>? DrainageStrategies { get; set; }
        public DbSet<ProductionProfileGas>? ProductionProfileGas { get; set; }
        public DbSet<ProductionProfileOil>? ProductionProfileOil { get; set; }
        public DbSet<ProductionProfileWater>? ProductionProfileWater { get; set; }
        public DbSet<ProductionProfileWaterInjection>? ProductionProfileWaterInjection { get; set; }
        public DbSet<FuelFlaringAndLosses>? FuelFlaringAndLosses { get; set; }
        public DbSet<NetSalesGas>? NetSalesGas { get; set; }
        public DbSet<Co2Emissions>? Co2Emissions { get; set; }

        public DbSet<WellProject>? WellProjects { get; set; }

        public DbSet<Exploration>? Explorations { get; set; }
    }
}
