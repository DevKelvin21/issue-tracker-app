using IssueTracker.Core.Entities;
using IssueTracker.Core.Enums;

namespace IssueTracker.Core.Interfaces;

/// <summary>
/// Repository interface for Issue entity operations
/// </summary>
public interface IIssueRepository
{
    /// <summary>
    /// Retrieves all issues with optional status filter
    /// </summary>
    Task<IEnumerable<Issue>> GetAllAsync(
        IssueStatus? status = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of issues with optional status filter
    /// </summary>
    Task<PagedResult<Issue>> GetPagedAsync(
        IssueStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single issue by ID
    /// </summary>
    Task<Issue?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new issue
    /// </summary>
    Task<Issue> AddAsync(Issue issue, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing issue
    /// </summary>
    Task UpdateAsync(Issue issue, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an issue
    /// </summary>
    Task DeleteAsync(Issue issue, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves all pending changes to the database
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
