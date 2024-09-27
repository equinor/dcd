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
