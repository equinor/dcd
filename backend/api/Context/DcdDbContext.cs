using api.AppInfrastructure.Authorization;
using api.Features.Cases.Recalculation;
using api.Models;
using api.Models.Infrastructure;
using api.Models.Infrastructure.BackgroundJobs;
using api.Models.Infrastructure.ProjectRecalculation;
using api.Models.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace api.Context;

public class DcdDbContext(DbContextOptions<DcdDbContext> options, CurrentUser? currentUser) : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
    public DbSet<RevisionDetails> RevisionDetails => Set<RevisionDetails>();
    public DbSet<ExplorationOperationalWellCosts> ExplorationOperationalWellCosts => Set<ExplorationOperationalWellCosts>();
    public DbSet<DevelopmentOperationalWellCosts> DevelopmentOperationalWellCosts => Set<DevelopmentOperationalWellCosts>();
    public DbSet<Case> Cases => Set<Case>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<Well> Wells => Set<Well>();
    public DbSet<DevelopmentWell> DevelopmentWells => Set<DevelopmentWell>();
    public DbSet<ExplorationWell> ExplorationWell => Set<ExplorationWell>();
    public DbSet<Surf> Surfs => Set<Surf>();
    public DbSet<Substructure> Substructures => Set<Substructure>();
    public DbSet<Topside> Topsides => Set<Topside>();
    public DbSet<Transport> Transports => Set<Transport>();
    public DbSet<OnshorePowerSupply> OnshorePowerSupplies => Set<OnshorePowerSupply>();
    public DbSet<DrainageStrategy> DrainageStrategies => Set<DrainageStrategy>();
    public DbSet<WellProject> WellProjects => Set<WellProject>();
    public DbSet<Exploration> Explorations => Set<Exploration>();
    public DbSet<TimeSeriesProfile> TimeSeriesProfiles => Set<TimeSeriesProfile>();
    public DbSet<ChangeLog> ChangeLogs => Set<ChangeLog>();
    public DbSet<RequestLog> RequestLogs => Set<RequestLog>();
    public DbSet<ExceptionLog> ExceptionLogs => Set<ExceptionLog>();
    public DbSet<PendingRecalculation> PendingRecalculations => Set<PendingRecalculation>();
    public DbSet<CompletedRecalculation> CompletedRecalculations => Set<CompletedRecalculation>();
    public DbSet<BackgroundJobMachineInstanceLog> BackgroundJobMachineInstanceLogs => Set<BackgroundJobMachineInstanceLog>();
    public DbSet<BackgroundJobLog> BackgroundJobLogs => Set<BackgroundJobLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectMemberConfiguration());
        modelBuilder.ApplyConfiguration(new CaseConfiguration());
        modelBuilder.ApplyConfiguration(new DevelopmentWellConfiguration());
        modelBuilder.ApplyConfiguration(new ExplorationWellConfiguration());
        modelBuilder.ApplyConfiguration(new TimeSeriesProfileConfiguration());
        modelBuilder.ApplyConfiguration(new ChangeLogConfiguration());
        modelBuilder.ApplyConfiguration(new RequestLogConfiguration());
        modelBuilder.ApplyConfiguration(new ExceptionLogConfiguration());
        modelBuilder.ApplyConfiguration(new BackgroundJobLogConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

        optionsBuilder.EnableSensitiveDataLogging();

        base.OnConfiguring(optionsBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;

        ChangeLogs.AddRange(ChangeLogService.GenerateChangeLogs(this, currentUser, utcNow));
        SetCreatedAndUpdatedDates(utcNow);
        EnsureValidStartYearsForProfiles();

        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        var utcNow = DateTime.UtcNow;

        ChangeLogs.AddRange(ChangeLogService.GenerateChangeLogs(this, currentUser, utcNow));
        SetCreatedAndUpdatedDates(utcNow);
        EnsureValidStartYearsForProfiles();

        return base.SaveChanges();
    }

    private void EnsureValidStartYearsForProfiles()
    {
        var profileWithInvalidStartYear = ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.State is EntityState.Added or EntityState.Modified)
            .FirstOrDefault(x => x.Entity.StartYear is < -50 or > 50)
            ?.Entity;

        if (profileWithInvalidStartYear != null)
        {
            throw new TimeSeriesProfileException($"Cannot save TimeSeriesProfile of type {profileWithInvalidStartYear.ProfileType}. {profileWithInvalidStartYear.StartYear} is not a valid StartYear.");
        }

        var profileWithInvalidValues = ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.State is EntityState.Added or EntityState.Modified)
            .FirstOrDefault(x => x.Entity.Values.Length > 100)
            ?.Entity;

        if (profileWithInvalidValues != null)
        {
            throw new TimeSeriesProfileException($"Cannot save TimeSeriesProfile of type {profileWithInvalidValues.ProfileType}. There are {profileWithInvalidValues.Values.Length} values in the values list.");
        }
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
