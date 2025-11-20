using IssueTracker.Core.Enums;

namespace IssueTracker.Application.DTOs;

/// <summary>
/// Data transfer object for updating an existing issue
/// </summary>
public class UpdateIssueDto
{
    /// <summary>
    /// Brief title describing the issue
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Detailed description of the issue
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Current status of the issue
    /// </summary>
    public IssueStatus? Status { get; set; }
}
