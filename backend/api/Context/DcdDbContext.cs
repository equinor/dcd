using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Context;

public class DomainEvent
{
    public string EventType { get; set; }
    public object Data { get; set; }
}

public interface IDomainEventDispatcher
{
    void Dispatch(DomainEvent domainEvent);
}

public class DcdDbContext : DbContext
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public DcdDbContext(
        DbContextOptions<DcdDbContext> options,
        IDomainEventDispatcher domainEventDispatcher
        ) : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    public override int SaveChanges()
    {
        DetectChanges();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        DetectChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

private void DetectChanges()
{
    var changeTracker = ChangeTracker.Entries<Case>();

    bool shouldNotify = changeTracker.Any(e =>
        e.State == EntityState.Modified &&
        (e.Property("DG3Date").IsModified || e.Property("DG4Date").IsModified));

    if (shouldNotify)
    {
        var domainEvent = new DomainEvent { EventType = "CaseChanged", Data = null /* Add relevant data */ };
        _domainEventDispatcher.Dispatch(domainEvent);
    }
}

    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ExplorationOperationalWellCosts> ExplorationOperationalWellCosts { get; set; } = null!;
    public DbSet<DevelopmentOperationalWellCosts> DevelopmentOperationalWellCosts { get; set; } = null!;

    public DbSet<Case> Cases { get; set; } = null!;
    public DbSet<CessationWellsCostOverride> CessationWellsCostOverride { get; set; } = null!;
    public DbSet<CessationOffshoreFacilitiesCostOverride> CessationOffshoreFacilitiesCostOverride { get; set; } = null!;
    public DbSet<TotalFeasibilityAndConceptStudiesOverride> TotalFeasibilityAndConceptStudiesOverride { get; set; } = null!;
    public DbSet<TotalFEEDStudiesOverride> TotalFEEDStudiesOverride { get; set; } = null!;
    public DbSet<HistoricCostCostProfile> HistoricCostCostProfile { get; set; } = null!;
    public DbSet<WellInterventionCostProfileOverride> WellInterventionCostProfileOverride { get; set; } = null!;
    public DbSet<OffshoreFacilitiesOperationsCostProfileOverride> OffshoreFacilitiesOperationsCostProfileOverride { get; set; } = null!;
    public DbSet<OnshoreRelatedOPEXCostProfile> OnshoreRelatedOPEXCostProfile { get; set; } = null!;
    public DbSet<AdditionalOPEXCostProfile> AdditionalOPEXCostProfile { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;

    public DbSet<Well> Wells { get; set; } = null!;
    public DbSet<WellProjectWell> WellProjectWell { get; set; } = null!;
    public DbSet<ExplorationWell> ExplorationWell { get; set; } = null!;

    public DbSet<Surf> Surfs { get; set; } = null!;
    public DbSet<SurfCostProfile> SurfCostProfile { get; set; } = null!;
    public DbSet<SurfCostProfileOverride> SurfCostProfileOverride { get; set; } = null!;
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
    public DbSet<TransportCostProfileOverride> TransportCostProfileOverride { get; set; } = null!;
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

    public DbSet<DeferredOilProduction> DeferredOilProduction { get; set; } = null!;
    public DbSet<DeferredGasProduction> DeferredGasProduction { get; set; } = null!;

    public DbSet<WellProject> WellProjects { get; set; } = null!;
    public DbSet<OilProducerCostProfileOverride> OilProducerCostProfileOverride { get; set; } = null!;
    public DbSet<GasProducerCostProfileOverride> GasProducerCostProfileOverride { get; set; } = null!;
    public DbSet<WaterInjectorCostProfileOverride> WaterInjectorCostProfileOverride { get; set; } = null!;
    public DbSet<GasInjectorCostProfileOverride> GasInjectorCostProfileOverride { get; set; } = null!;

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
