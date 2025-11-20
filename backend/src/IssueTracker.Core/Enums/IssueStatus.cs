namespace IssueTracker.Core.Enums;

/// <summary>
/// Represents the status of an issue in the tracking system
/// </summary>
public enum IssueStatus
{
    /// <summary>
    /// Issue has been created but work has not started
    /// </summary>
    Open = 1,

    /// <summary>
    /// Work is actively being done on this issue
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// Issue has been completed and resolved
    /// </summary>
    Resolved = 3
}
