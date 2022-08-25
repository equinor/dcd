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
        public DbSet<OperationalWellCosts>? OperationalWellCosts { get; set; }

        public DbSet<Case>? Cases { get; set; }
        public DbSet<Well>? Wells { get; set; }
        public DbSet<WellProjectWell>? WellProjectWell { get; set; }
        public DbSet<ExplorationWell>? ExplorationWell { get; set; }

        public DbSet<Surf>? Surfs { get; set; }
        public DbSet<SurfCostProfile>? SurfCostProfile { get; set; }
        public DbSet<SurfCessationCostProfile>? SurfCessationCostProfiles { get; set; }

        public DbSet<Substructure>? Substructures { get; set; }
        public DbSet<SubstructureCostProfile>? SubstructureCostProfiles { get; set; }
        public DbSet<SubstructureCessationCostProfile>? SubstructureCessationCostProfiles { get; set; }

        public DbSet<Topside>? Topsides { get; set; }

        public DbSet<TopsideCostProfile>? TopsideCostProfiles { get; set; }
        public DbSet<TopsideCessationCostProfile>? TopsideCessationCostProfiles { get; set; }

        public DbSet<Transport>? Transports { get; set; }
        public DbSet<TransportCostProfile>? TransportCostProfile { get; set; }
        public DbSet<TransportCessationCostProfile>? TransportCessationCostProfiles { get; set; }

        public DbSet<DrainageStrategy>? DrainageStrategies { get; set; }
        public DbSet<ProductionProfileGas>? ProductionProfileGas { get; set; }
        public DbSet<ProductionProfileOil>? ProductionProfileOil { get; set; }
        public DbSet<ProductionProfileWater>? ProductionProfileWater { get; set; }
        public DbSet<ProductionProfileWaterInjection>? ProductionProfileWaterInjection { get; set; }
        public DbSet<FuelFlaringAndLosses>? FuelFlaringAndLosses { get; set; }
        public DbSet<NetSalesGas>? NetSalesGas { get; set; }
        public DbSet<Co2Emissions>? Co2Emissions { get; set; }
        public DbSet<ProductionProfileNGL>? ProductionProfileNGL { get; set; }

        public DbSet<WellProject>? WellProjects { get; set; }
        public DbSet<WellProjectCostProfile>? WellProjectCostProfile { get; set; }

        public DbSet<DrillingSchedule>? DrillingSchedule { get; set; }

        public DbSet<Exploration>? Explorations { get; set; }
        public DbSet<ExplorationCostProfile>? ExplorationCostProfile { get; set; }
        public DbSet<GAndGAdminCost>? GAndGAdminCost { get; set; }
        public DbSet<SeismicAcquisitionAndProcessing>? SeismicAcquisitionAndProcessing { get; set; }
        public DbSet<CountryOfficeCost>? CountryOfficeCost { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Project>().HasOne(p => p.ExplorationWellCosts).WithOne(e => e.Project);
            // modelBuilder.Entity<Project>().HasOne(p => p.AppraisalWellCosts).WithOne(e => e.Project);
            // modelBuilder.Entity<Project>().HasOne(p => p.DrillingWellCosts).WithOne(e => e.Project);
            // modelBuilder.Entity<OperationalWellCosts>().HasOne(p => p.Project).WithOne(p => p.ExplorationWellCosts)
            // .HasForeignKey("Project.ExplorationWellCostsId");
            // modelBuilder.Entity<OperationalWellCosts>().HasOne(p => p.Project).WithOne(p => p.AppraisalWellCosts)
            // .HasForeignKey("Project.AppraisalWellCostsId");
            // modelBuilder.Entity<OperationalWellCosts>().HasOne(p => p.Project).WithOne(p => p.DrillingWellCosts);

            modelBuilder.Entity<Project>().HasOne(p => p.ExplorationWellCosts).WithOne().HasForeignKey("Project", "ExplorationWellCostsId");
            modelBuilder.Entity<Project>().HasOne(p => p.AppraisalWellCosts).WithOne().HasForeignKey("Project", "AppraisalWellCostsId");
            modelBuilder.Entity<Project>().HasOne(p => p.DrillingWellCosts).WithOne().HasForeignKey("Project", "DrillingWellCostsId");

            modelBuilder.Entity<WellProjectWell>().HasKey(wc => new { wc.WellProjectId, wc.WellId });
            modelBuilder.Entity<WellProjectWell>().HasOne(w => w.Well).WithMany(w => w.WellProjectWells).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExplorationWell>().HasKey(ew => new { ew.ExplorationId, ew.WellId });
            modelBuilder.Entity<ExplorationWell>().HasOne(w => w.Well).WithMany(w => w.ExplorationWells).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
