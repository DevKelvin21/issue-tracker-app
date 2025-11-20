using IssueTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssueTracker.Infrastructure.Configurations;

/// <summary>
/// Entity Framework configuration for the Issue entity
/// </summary>
public class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        // Primary key
        builder.HasKey(i => i.Id);

        // Title configuration
        builder.Property(i => i.Title)
            .IsRequired()
            .HasMaxLength(200);

        // Description configuration
        builder.Property(i => i.Description)
            .IsRequired()
            .HasMaxLength(2000);

        // Status configuration - stored as int in database
        builder.Property(i => i.Status)
            .IsRequired()
            .HasConversion<int>();

        // CreatedAt configuration - set default value in database
        builder.Property(i => i.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        // ResolvedAt configuration - nullable
        builder.Property(i => i.ResolvedAt)
            .IsRequired(false);

        // Indexes for performance
        builder.HasIndex(i => i.Status)
            .HasDatabaseName("IX_Issues_Status");

        builder.HasIndex(i => i.CreatedAt)
            .HasDatabaseName("IX_Issues_CreatedAt");

        // Table name
        builder.ToTable("Issues");
    }
}
