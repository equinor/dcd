using api.AppInfrastructure;
using api.AppInfrastructure.Authorization;
using api.Models;
using api.Models.Infrastructure;
using api.Models.Infrastructure.BackgroundJobs;
using api.Models.Infrastructure.ProjectRecalculation;
using api.Models.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace api.Context;

public class DcdDbContext(DbContextOptions<DcdDbContext> options, CurrentUser? currentUser, IServiceScopeFactory? serviceScopeFactory) : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
    public DbSet<RevisionDetails> RevisionDetails => Set<RevisionDetails>();
    public DbSet<ExplorationOperationalWellCosts> ExplorationOperationalWellCosts => Set<ExplorationOperationalWellCosts>();
    public DbSet<DevelopmentOperationalWellCosts> DevelopmentOperationalWellCosts => Set<DevelopmentOperationalWellCosts>();
    public DbSet<Case> Cases => Set<Case>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<Well> Wells => Set<Well>();
    public DbSet<WellProjectWell> WellProjectWell => Set<WellProjectWell>();
    public DbSet<ExplorationWell> ExplorationWell => Set<ExplorationWell>();
    public DbSet<Surf> Surfs => Set<Surf>();
    public DbSet<Substructure> Substructures => Set<Substructure>();
    public DbSet<Topside> Topsides => Set<Topside>();
    public DbSet<Transport> Transports => Set<Transport>();
    public DbSet<OnshorePowerSupply> OnshorePowerSupplies => Set<OnshorePowerSupply>();
    public DbSet<DrainageStrategy> DrainageStrategies => Set<DrainageStrategy>();
    public DbSet<AdditionalProductionProfileOil> AdditionalProductionProfileOil => Set<AdditionalProductionProfileOil>();
    public DbSet<ProductionProfileGas> ProductionProfileGas => Set<ProductionProfileGas>();
    public DbSet<AdditionalProductionProfileGas> AdditionalProductionProfileGas => Set<AdditionalProductionProfileGas>();
    public DbSet<ProductionProfileWater> ProductionProfileWater => Set<ProductionProfileWater>();
    public DbSet<ProductionProfileWaterInjection> ProductionProfileWaterInjection => Set<ProductionProfileWaterInjection>();
    public DbSet<Co2Intensity> Co2Intensity => Set<Co2Intensity>();
    public DbSet<ProductionProfileNgl> ProductionProfileNgl => Set<ProductionProfileNgl>();
    public DbSet<DeferredOilProduction> DeferredOilProduction => Set<DeferredOilProduction>();
    public DbSet<DeferredGasProduction> DeferredGasProduction => Set<DeferredGasProduction>();
    public DbSet<WellProject> WellProjects => Set<WellProject>();
    public DbSet<DrillingSchedule> DrillingSchedule => Set<DrillingSchedule>();
    public DbSet<Exploration> Explorations => Set<Exploration>();
    public DbSet<TimeSeriesProfile> TimeSeriesProfiles => Set<TimeSeriesProfile>();
    public DbSet<ChangeLog> ChangeLogs => Set<ChangeLog>();
    public DbSet<RequestLog> RequestLogs => Set<RequestLog>();
    public DbSet<ExceptionLog> ExceptionLogs => Set<ExceptionLog>();
    public DbSet<LazyLoadingOccurrence> LazyLoadingOccurrences => Set<LazyLoadingOccurrence>();
    public DbSet<PendingRecalculation> PendingRecalculations => Set<PendingRecalculation>();
    public DbSet<CompletedRecalculation> CompletedRecalculations => Set<CompletedRecalculation>();
    public DbSet<BackgroundJobMachineInstanceLog> BackgroundJobMachineInstanceLogs => Set<BackgroundJobMachineInstanceLog>();
    public DbSet<FrontendException> FrontendExceptions => Set<FrontendException>();
    public DbSet<BackgroundJobLog> BackgroundJobLogs => Set<BackgroundJobLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectMemberConfiguration());
        modelBuilder.ApplyConfiguration(new CaseConfiguration());
        modelBuilder.ApplyConfiguration(new WellProjectWellConfiguration());
        modelBuilder.ApplyConfiguration(new ExplorationWellConfiguration());
        modelBuilder.ApplyConfiguration(new TimeSeriesProfileConfiguration());
        modelBuilder.ApplyConfiguration(new ChangeLogConfiguration());
        modelBuilder.ApplyConfiguration(new RequestLogConfiguration());
        modelBuilder.ApplyConfiguration(new ExceptionLogConfiguration());
        modelBuilder.ApplyConfiguration(new FrontendExceptionConfiguration());
        modelBuilder.ApplyConfiguration(new BackgroundJobLogConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (DcdEnvironments.EnableVerboseEntityFrameworkLogging)
        {
            optionsBuilder.LogTo(WriteLazyLoadingToDatabase);
        }

        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

        optionsBuilder.EnableSensitiveDataLogging();

        base.OnConfiguring(optionsBuilder);
    }

    private static bool _isLogging;
    private void WriteLazyLoadingToDatabase(string message)
    {
        if (serviceScopeFactory == null)
        {
            return;
        }

        const string patternStart = "The navigation";
        const string patternEnd = "is being lazy-loaded.";

        if (!message.Contains(patternStart) || !message.Contains(patternEnd))
        {
            return;
        }

        if (_isLogging)
        {
            return;
        }

        _isLogging = true;

        message = message[message.IndexOf(patternStart)..];
        message = message[..(message.IndexOf(patternEnd) + patternEnd.Length)];

        using var scope = serviceScopeFactory.CreateScope();

        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DcdDbContext>>();

        using var dbContext = dbContextFactory.CreateDbContext();

        dbContext.LazyLoadingOccurrences.Add(new LazyLoadingOccurrence
        {
            Message = message,
            FullStackTrace = new System.Diagnostics.StackTrace().ToString(),
            TimestampUtc = DateTime.UtcNow
        });

        dbContext.SaveChanges();

        _isLogging = false;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;

        ChangeLogs.AddRange(ChangeLogService.GenerateChangeLogs(this, currentUser, utcNow));
        SetCreatedAndUpdatedDates(utcNow);

        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        var utcNow = DateTime.UtcNow;

        ChangeLogs.AddRange(ChangeLogService.GenerateChangeLogs(this, currentUser, utcNow));
        SetCreatedAndUpdatedDates(utcNow);

        return base.SaveChanges();
    }

    private void SetCreatedAndUpdatedDates(DateTime utcNow)
    {
        var createdEntries = ChangeTracker.Entries()
            .Where(x => x is { Entity: IDateTrackedEntity, State: EntityState.Added });

        foreach (var entry in createdEntries)
        {
            var entity = (IDateTrackedEntity)entry.Entity;
            entity.CreatedUtc = utcNow;
            entity.UpdatedUtc = utcNow;
            entity.CreatedBy = currentUser?.Username ?? "SYSTEM";
            entity.UpdatedBy = currentUser?.Username ?? "SYSTEM";
        }

        var updatedEntries = ChangeTracker.Entries()
            .Where(x => x is { Entity: IDateTrackedEntity, State: EntityState.Modified });

        foreach (var entry in updatedEntries)
        {
            var entity = (IDateTrackedEntity)entry.Entity;
            entity.UpdatedUtc = utcNow;
            entity.UpdatedBy = currentUser?.Username ?? "SYSTEM";
        }
    }
}
