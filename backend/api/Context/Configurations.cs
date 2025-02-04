using api.Models;
using api.Models.Infrastructure;
using api.Models.Infrastructure.BackgroundJobs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Context;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasIndex(p => p.FusionProjectId);

        // Configure self-referencing relationship for Project for revisions
        builder.HasOne(p => p.OriginalProject)
            .WithMany(p => p.Revisions)
            .HasForeignKey(p => p.OriginalProjectId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes
    }
}

public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.HasIndex(ew => new { ew.UserId, ew.ProjectId }).IsUnique();
    }
}

public class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> builder)
    {
        builder.HasOne(c => c.DrainageStrategy)
            .WithMany()
            .HasForeignKey(c => c.DrainageStrategyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.WellProject)
            .WithMany()
            .HasForeignKey(c => c.WellProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Exploration)
            .WithMany()
            .HasForeignKey(c => c.ExplorationId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Transport)
            .WithMany()
            .HasForeignKey(c => c.TransportId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.OnshorePowerSupply)
            .WithMany()
            .HasForeignKey(c => c.OnshorePowerSupplyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Topside)
            .WithMany()
            .HasForeignKey(c => c.TopsideId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Substructure)
            .WithMany()
            .HasForeignKey(c => c.SubstructureId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Surf)
            .WithMany()
            .HasForeignKey(c => c.SurfId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class DevelopmentWellConfiguration : IEntityTypeConfiguration<DevelopmentWell>
{
    public void Configure(EntityTypeBuilder<DevelopmentWell> builder)
    {
        builder.Property(x => x.Id)
            .HasDefaultValueSql("newid()");

        builder.HasIndex(wc => new { wc.WellProjectId, wc.WellId }).IsUnique();

        builder.HasOne(w => w.Well)
            .WithMany(w => w.DevelopmentWells)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(w => w.Campaign)
            .WithMany(w => w.DevelopmentWells)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class ExplorationWellConfiguration : IEntityTypeConfiguration<ExplorationWell>
{
    public void Configure(EntityTypeBuilder<ExplorationWell> builder)
    {
        builder.Property(x => x.Id)
            .HasDefaultValueSql("newid()");

        builder.HasIndex(ew => new { ew.ExplorationId, ew.WellId }).IsUnique();

        builder.HasOne(w => w.Well)
            .WithMany(w => w.ExplorationWells)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(w => w.Campaign)
            .WithMany(w => w.ExplorationWells)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class TimeSeriesProfileConfiguration : IEntityTypeConfiguration<TimeSeriesProfile>
{
    public void Configure(EntityTypeBuilder<TimeSeriesProfile> builder)
    {
        builder.HasIndex(x => new { x.CaseId, x.ProfileType }).IsUnique();
    }
}

public class ChangeLogConfiguration : IEntityTypeConfiguration<ChangeLog>
{
    public void Configure(EntityTypeBuilder<ChangeLog> builder)
    {
        builder.HasIndex(x => x.EntityId);
        builder.HasIndex(x => x.EntityName);
        builder.HasIndex(x => x.PropertyName);
        builder.HasIndex(x => x.TimestampUtc);
    }
}

public class RequestLogConfiguration : IEntityTypeConfiguration<RequestLog>
{
    public void Configure(EntityTypeBuilder<RequestLog> builder)
    {
        builder.HasIndex(x => x.RequestStartUtc);
    }
}

public class ExceptionLogConfiguration : IEntityTypeConfiguration<ExceptionLog>
{
    public void Configure(EntityTypeBuilder<ExceptionLog> builder)
    {
        builder.HasIndex(x => x.UtcTimestamp);
    }
}

public class BackgroundJobLogConfiguration : IEntityTypeConfiguration<BackgroundJobLog>
{
    public void Configure(EntityTypeBuilder<BackgroundJobLog> builder)
    {
        builder.HasIndex(x => x.TimestampUtc);
    }
}
