using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Context;

public class DcdDbContext : DbContext
{
    public DcdDbContext(DbContextOptions<DcdDbContext> options) : base(options)
    {

    }
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ExplorationOperationalWellCosts> ExplorationOperationalWellCosts { get; set; } = null!;
    public DbSet<DevelopmentOperationalWellCosts> DevelopmentOperationalWellCosts { get; set; } = null!;

    public DbSet<Case> Cases { get; set; } = null!;
    public DbSet<Well> Wells { get; set; } = null!;
    public DbSet<WellProjectWell> WellProjectWell { get; set; } = null!;
    public DbSet<ExplorationWell> ExplorationWell { get; set; } = null!;

    public DbSet<Surf> Surfs { get; set; } = null!;
    public DbSet<SurfCostProfile> SurfCostProfile { get; set; } = null!;
    public DbSet<SurfCessationCostProfile> SurfCessationCostProfiles { get; set; } = null!;

    public DbSet<Substructure> Substructures { get; set; } = null!;
    public DbSet<SubstructureCostProfile> SubstructureCostProfiles { get; set; } = null!;
    public DbSet<SubstructureCostProfileOverride> SubstructureCostProfileOverride { get; set; } = null!;

    public DbSet<SubstructureCessationCostProfile> SubstructureCessationCostProfiles { get; set; } = null!;

    public DbSet<Topside> Topsides { get; set; } = null!;

    public DbSet<TopsideCostProfile> TopsideCostProfiles { get; set; } = null!;
    public DbSet<TopsideCostProfileOverride> TopsideCostProfileOverride { get; set; } = null!;
    public DbSet<TopsideCessationCostProfile> TopsideCessationCostProfiles { get; set; } = null!;

    public DbSet<Transport> Transports { get; set; } = null!;
    public DbSet<TransportCostProfile> TransportCostProfile { get; set; } = null!;
    public DbSet<TransportCessationCostProfile> TransportCessationCostProfiles { get; set; } = null!;

    public DbSet<DrainageStrategy> DrainageStrategies { get; set; } = null!;
    public DbSet<ProductionProfileGas> ProductionProfileGas { get; set; } = null!;
    public DbSet<ProductionProfileOil> ProductionProfileOil { get; set; } = null!;
    public DbSet<ProductionProfileWater> ProductionProfileWater { get; set; } = null!;
    public DbSet<ProductionProfileWaterInjection> ProductionProfileWaterInjection { get; set; } = null!;

    public DbSet<FuelFlaringAndLosses> FuelFlaringAndLosses { get; set; } = null!;
    public DbSet<FuelFlaringAndLossesOverride> FuelFlaringAndLossesOverride { get; set; } = null!;

    public DbSet<NetSalesGas> NetSalesGas { get; set; } = null!;
    public DbSet<NetSalesGasOverride> NetSalesGasOverride { get; set; } = null!;

    public DbSet<Co2Emissions> Co2Emissions { get; set; } = null!;
    public DbSet<Co2EmissionsOverride> Co2EmissionsOverride { get; set; } = null!;

    public DbSet<ProductionProfileNGL> ProductionProfileNGL { get; set; } = null!;

    public DbSet<ImportedElectricity> ImportedElectricity { get; set; } = null!;
    public DbSet<ImportedElectricityOverride> ImportedElectricityOverride { get; set; } = null!;


    public DbSet<WellProject> WellProjects { get; set; } = null!;

    public DbSet<DrillingSchedule> DrillingSchedule { get; set; } = null!;

    public DbSet<Exploration> Explorations { get; set; } = null!;
    public DbSet<GAndGAdminCost> GAndGAdminCost { get; set; } = null!;
    public DbSet<SeismicAcquisitionAndProcessing> SeismicAcquisitionAndProcessing { get; set; } = null!;
    public DbSet<CountryOfficeCost> CountryOfficeCost { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WellProjectWell>()
            .HasKey(wc => new { wc.WellProjectId, wc.WellId });

        modelBuilder.Entity<WellProjectWell>()
            .HasOne(w => w.Well)
            .WithMany(w => w.WellProjectWells)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ExplorationWell>()
            .HasKey(ew => new { ew.ExplorationId, ew.WellId });

        modelBuilder.Entity<ExplorationWell>()
            .HasOne(w => w.Well)
            .WithMany(w => w.ExplorationWells)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Project>()
            .Property(p => p.Classification)
            .HasDefaultValue(ProjectClassification.Internal);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }
}
