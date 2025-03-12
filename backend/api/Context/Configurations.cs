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

        builder.HasOne(p => p.OriginalProject)
            .WithMany(p => p.Revisions)
            .HasForeignKey(p => p.OriginalProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.ReferenceCase)
            .WithMany()
            .HasForeignKey(p => p.ReferenceCaseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.HasIndex(ew => new { ew.AzureAdUserId, ew.ProjectId }).IsUnique();
    }
}

public class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> builder)
    {
        builder.HasOne(c => c.DrainageStrategy)
            .WithOne(c => c.Case)
            .HasForeignKey<DrainageStrategy>(c => c.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Transport)
            .WithOne(c => c.Case)
            .HasForeignKey<Transport>(c => c.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.OnshorePowerSupply)
            .WithOne(c => c.Case)
            .HasForeignKey<OnshorePowerSupply>(c => c.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Topside)
            .WithOne(c => c.Case)
            .HasForeignKey<Topside>(c => c.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Substructure)
            .WithOne(c => c.Case)
            .HasForeignKey<Substructure>(c => c.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Surf)
            .WithOne(c => c.Case)
            .HasForeignKey<Surf>(c => c.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.OriginalCase)
            .WithMany(e => e.RevisionCases)
            .HasForeignKey(e => e.OriginalCaseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class CampaignWellConfiguration : IEntityTypeConfiguration<CampaignWell>
{
    public void Configure(EntityTypeBuilder<CampaignWell> builder)
    {
        builder.HasIndex(x => new { x.WellId, x.CampaignId }).IsUnique();

        builder.HasOne(w => w.Well)
            .WithMany(w => w.CampaignWells)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(w => w.Campaign)
            .WithMany(w => w.CampaignWells)
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
        builder.HasIndex(x => x.TimestampUtc);
    }
}

public class BackgroundJobLogConfiguration : IEntityTypeConfiguration<BackgroundJobLog>
{
    public void Configure(EntityTypeBuilder<BackgroundJobLog> builder)
    {
        builder.HasIndex(x => x.TimestampUtc);
    }
}
