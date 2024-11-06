using api.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Context;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasIndex(p => p.FusionProjectId).HasDatabaseName("IX_Project_FusionProjectId");

        // Configure self-referencing relationship for Project for revisions
        builder.HasOne(p => p.OriginalProject)
            .WithMany(p => p.Revisions)
            .HasForeignKey(p => p.OriginalProjectId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

        builder.Property(p => p.Classification)
            .HasDefaultValue(ProjectClassification.Internal);

        builder.Property(p => p.DiscountRate)
            .HasDefaultValue(8.0);

        builder.Property(p => p.OilPriceUSD)
            .HasDefaultValue(75.0);

        builder.Property(p => p.GasPriceNOK)
            .HasDefaultValue(3.0);

        builder.Property(p => p.ExchangeRateUSDToNOK)
            .HasDefaultValue(10);
    }
}

public class RevisionDetailsConfiguration : IEntityTypeConfiguration<RevisionDetails>
{
    public void Configure(EntityTypeBuilder<RevisionDetails> builder)
    {
        builder.HasIndex(rd => rd.OriginalProjectId).HasDatabaseName("IX_RevisionDetails_OriginalProjectId");
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
            .HasForeignKey(c => c.DrainageStrategyLink)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.WellProject)
            .WithMany()
            .HasForeignKey(c => c.WellProjectLink)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Exploration)
            .WithMany()
            .HasForeignKey(c => c.ExplorationLink)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Transport)
            .WithMany()
            .HasForeignKey(c => c.TransportLink)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Topside)
            .WithMany()
            .HasForeignKey(c => c.TopsideLink)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Substructure)
            .WithMany()
            .HasForeignKey(c => c.SubstructureLink)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Surf)
            .WithMany()
            .HasForeignKey(c => c.SurfLink)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class WellProjectWellConfiguration : IEntityTypeConfiguration<WellProjectWell>
{
    public void Configure(EntityTypeBuilder<WellProjectWell> builder)
    {
        builder.HasKey(wc => new { wc.WellProjectId, wc.WellId });

        builder.HasOne(w => w.Well)
            .WithMany(w => w.WellProjectWells)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class ExplorationWellConfiguration : IEntityTypeConfiguration<ExplorationWell>
{
    public void Configure(EntityTypeBuilder<ExplorationWell> builder)
    {
        builder.HasKey(ew => new { ew.ExplorationId, ew.WellId });

        builder.HasOne(w => w.Well)
            .WithMany(w => w.ExplorationWells)
            .OnDelete(DeleteBehavior.NoAction);
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
