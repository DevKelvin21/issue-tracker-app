using System.Reflection;
using IssueTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Infrastructure.Data;

/// <summary>
/// Database context for the Issue Tracker application
/// </summary>
public class IssueTrackerDbContext : DbContext
{
    public IssueTrackerDbContext(DbContextOptions<IssueTrackerDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Issues table
    /// </summary>
    public DbSet<Issue> Issues => Set<Issue>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
