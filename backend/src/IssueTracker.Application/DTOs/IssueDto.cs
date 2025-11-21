using IssueTracker.Core.Enums;

namespace IssueTracker.Application.DTOs;

/// <summary>
/// Data transfer object for an issue
/// </summary>
public class IssueDto
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
    public IssueStatus Status { get; set; }

    /// <summary>
    /// Date and time when the issue was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date and time when the issue was resolved, null if not resolved
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
}
