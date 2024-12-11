using api.AppInfrastructure;
using api.AppInfrastructure.Authorization;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Context;

public class DcdDbContext(DbContextOptions<DcdDbContext> options, CurrentUser? currentUser) : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
    public DbSet<RevisionDetails> RevisionDetails => Set<RevisionDetails>();
    public DbSet<ExplorationOperationalWellCosts> ExplorationOperationalWellCosts => Set<ExplorationOperationalWellCosts>();
    public DbSet<DevelopmentOperationalWellCosts> DevelopmentOperationalWellCosts => Set<DevelopmentOperationalWellCosts>();
    public DbSet<Case> Cases => Set<Case>();
    public DbSet<CessationWellsCostOverride> CessationWellsCostOverride => Set<CessationWellsCostOverride>();
    public DbSet<CessationOffshoreFacilitiesCostOverride> CessationOffshoreFacilitiesCostOverride => Set<CessationOffshoreFacilitiesCostOverride>();
    public DbSet<CessationOnshoreFacilitiesCostProfile> CessationOnshoreFacilitiesCostProfile => Set<CessationOnshoreFacilitiesCostProfile>();
    public DbSet<TotalFeasibilityAndConceptStudiesOverride> TotalFeasibilityAndConceptStudiesOverride => Set<TotalFeasibilityAndConceptStudiesOverride>();
    public DbSet<TotalFEEDStudiesOverride> TotalFEEDStudiesOverride => Set<TotalFEEDStudiesOverride>();
    public DbSet<TotalOtherStudiesCostProfile> TotalOtherStudiesCostProfile => Set<TotalOtherStudiesCostProfile>();
    public DbSet<HistoricCostCostProfile> HistoricCostCostProfile => Set<HistoricCostCostProfile>();
    public DbSet<WellInterventionCostProfileOverride> WellInterventionCostProfileOverride => Set<WellInterventionCostProfileOverride>();
    public DbSet<OffshoreFacilitiesOperationsCostProfileOverride> OffshoreFacilitiesOperationsCostProfileOverride => Set<OffshoreFacilitiesOperationsCostProfileOverride>();
    public DbSet<OnshoreRelatedOPEXCostProfile> OnshoreRelatedOPEXCostProfile => Set<OnshoreRelatedOPEXCostProfile>();
    public DbSet<AdditionalOPEXCostProfile> AdditionalOPEXCostProfile => Set<AdditionalOPEXCostProfile>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<Well> Wells => Set<Well>();
    public DbSet<WellProjectWell> WellProjectWell => Set<WellProjectWell>();
    public DbSet<ExplorationWell> ExplorationWell => Set<ExplorationWell>();
    public DbSet<Surf> Surfs => Set<Surf>();
    public DbSet<SurfCostProfile> SurfCostProfile => Set<SurfCostProfile>();
    public DbSet<SurfCostProfileOverride> SurfCostProfileOverride => Set<SurfCostProfileOverride>();
    public DbSet<SurfCessationCostProfile> SurfCessationCostProfiles => Set<SurfCessationCostProfile>();
    public DbSet<Substructure> Substructures => Set<Substructure>();
    public DbSet<SubstructureCostProfile> SubstructureCostProfiles => Set<SubstructureCostProfile>();
    public DbSet<SubstructureCostProfileOverride> SubstructureCostProfileOverride => Set<SubstructureCostProfileOverride>();
    public DbSet<SubstructureCessationCostProfile> SubstructureCessationCostProfiles => Set<SubstructureCessationCostProfile>();
    public DbSet<Topside> Topsides => Set<Topside>();
    public DbSet<TopsideCostProfile> TopsideCostProfiles => Set<TopsideCostProfile>();
    public DbSet<TopsideCostProfileOverride> TopsideCostProfileOverride => Set<TopsideCostProfileOverride>();
    public DbSet<TopsideCessationCostProfile> TopsideCessationCostProfiles => Set<TopsideCessationCostProfile>();
    public DbSet<Transport> Transports => Set<Transport>();
    public DbSet<TransportCostProfile> TransportCostProfile => Set<TransportCostProfile>();
    public DbSet<TransportCostProfileOverride> TransportCostProfileOverride => Set<TransportCostProfileOverride>();
    public DbSet<TransportCessationCostProfile> TransportCessationCostProfiles => Set<TransportCessationCostProfile>();
    public DbSet<OnshorePowerSupply> OnshorePowerSupplies => Set<OnshorePowerSupply>();
    public DbSet<OnshorePowerSupplyCostProfile> OnshorePowerSupplyCostProfile => Set<OnshorePowerSupplyCostProfile>();
    public DbSet<OnshorePowerSupplyCostProfileOverride> OnshorePowerSupplyCostProfileOverride => Set<OnshorePowerSupplyCostProfileOverride>();
    public DbSet<DrainageStrategy> DrainageStrategies => Set<DrainageStrategy>();
    public DbSet<ProductionProfileOil> ProductionProfileOil => Set<ProductionProfileOil>();
    public DbSet<AdditionalProductionProfileOil> AdditionalProductionProfileOil => Set<AdditionalProductionProfileOil>();
    public DbSet<ProductionProfileGas> ProductionProfileGas => Set<ProductionProfileGas>();
    public DbSet<AdditionalProductionProfileGas> AdditionalProductionProfileGas => Set<AdditionalProductionProfileGas>();
    public DbSet<ProductionProfileWater> ProductionProfileWater => Set<ProductionProfileWater>();
    public DbSet<ProductionProfileWaterInjection> ProductionProfileWaterInjection => Set<ProductionProfileWaterInjection>();
    public DbSet<FuelFlaringAndLosses> FuelFlaringAndLosses => Set<FuelFlaringAndLosses>();
    public DbSet<FuelFlaringAndLossesOverride> FuelFlaringAndLossesOverride => Set<FuelFlaringAndLossesOverride>();
    public DbSet<NetSalesGas> NetSalesGas => Set<NetSalesGas>();
    public DbSet<NetSalesGasOverride> NetSalesGasOverride => Set<NetSalesGasOverride>();
    public DbSet<Co2Emissions> Co2Emissions => Set<Co2Emissions>();
    public DbSet<Co2EmissionsOverride> Co2EmissionsOverride => Set<Co2EmissionsOverride>();
    public DbSet<ProductionProfileNGL> ProductionProfileNGL => Set<ProductionProfileNGL>();
    public DbSet<ImportedElectricity> ImportedElectricity => Set<ImportedElectricity>();
    public DbSet<ImportedElectricityOverride> ImportedElectricityOverride => Set<ImportedElectricityOverride>();
    public DbSet<DeferredOilProduction> DeferredOilProduction => Set<DeferredOilProduction>();
    public DbSet<DeferredGasProduction> DeferredGasProduction => Set<DeferredGasProduction>();
    public DbSet<WellProject> WellProjects => Set<WellProject>();
    public DbSet<OilProducerCostProfileOverride> OilProducerCostProfileOverride => Set<OilProducerCostProfileOverride>();
    public DbSet<GasProducerCostProfileOverride> GasProducerCostProfileOverride => Set<GasProducerCostProfileOverride>();
    public DbSet<WaterInjectorCostProfileOverride> WaterInjectorCostProfileOverride => Set<WaterInjectorCostProfileOverride>();
    public DbSet<GasInjectorCostProfileOverride> GasInjectorCostProfileOverride => Set<GasInjectorCostProfileOverride>();
    public DbSet<DrillingSchedule> DrillingSchedule => Set<DrillingSchedule>();
    public DbSet<Exploration> Explorations => Set<Exploration>();
    public DbSet<GAndGAdminCost> GAndGAdminCost => Set<GAndGAdminCost>();
    public DbSet<SeismicAcquisitionAndProcessing> SeismicAcquisitionAndProcessing => Set<SeismicAcquisitionAndProcessing>();
    public DbSet<CountryOfficeCost> CountryOfficeCost => Set<CountryOfficeCost>();
    public DbSet<GAndGAdminCostOverride> GAndGAdminCostOverride => Set<GAndGAdminCostOverride>();
    public DbSet<ExplorationWellCostProfile> ExplorationWellCostProfile => Set<ExplorationWellCostProfile>();
    public DbSet<AppraisalWellCostProfile> AppraisalWellCostProfile => Set<AppraisalWellCostProfile>();
    public DbSet<SidetrackCostProfile> SidetrackCostProfile => Set<SidetrackCostProfile>();
    public DbSet<CalculatedTotalIncomeCostProfile> CalculatedTotalIncomeCostProfile => Set<CalculatedTotalIncomeCostProfile>();
    public DbSet<CalculatedTotalCostCostProfile> CalculatedTotalCostCostProfile => Set<CalculatedTotalCostCostProfile>();
    public DbSet<ChangeLog> ChangeLogs => Set<ChangeLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectMemberConfiguration());
        modelBuilder.ApplyConfiguration(new RevisionDetailsConfiguration());
        modelBuilder.ApplyConfiguration(new CaseConfiguration());
        modelBuilder.ApplyConfiguration(new WellProjectWellConfiguration());
        modelBuilder.ApplyConfiguration(new ExplorationWellConfiguration());
        modelBuilder.ApplyConfiguration(new ChangeLogConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (DcdEnvironments.EnableVerboseEntityFrameworkLogging)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }

        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;

        ChangeLogs.AddRange(ChangeLogService.GenerateChangeLogs(this, currentUser, utcNow));

        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        var utcNow = DateTime.UtcNow;

        ChangeLogs.AddRange(ChangeLogService.GenerateChangeLogs(this, currentUser, utcNow));

        return base.SaveChanges();
    }
}
