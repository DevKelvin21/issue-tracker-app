namespace IssueTracker.Application.DTOs;

/// <summary>
/// Data transfer object for creating a new issue
/// </summary>
public class CreateIssueDto
{
    /// <summary>
    /// Brief title describing the issue
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the issue
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
