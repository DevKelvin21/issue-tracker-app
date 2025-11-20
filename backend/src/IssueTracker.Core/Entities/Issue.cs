using IssueTracker.Core.Enums;

namespace IssueTracker.Core.Entities;

/// <summary>
/// Represents an issue in the tracking system
/// </summary>
public class Issue
{
    /// <summary>
    /// Unique identifier for the issue
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Brief title describing the issue
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the issue
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Current status of the issue
    /// </summary>
    public IssueStatus Status { get; set; } = IssueStatus.Open;

    /// <summary>
    /// Date and time when the issue was created (UTC)
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date and time when the issue was resolved (UTC), null if not resolved
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
}
