using IssueTracker.Application.DTOs;
using IssueTracker.Core.Enums;
using IssueTracker.Core.Interfaces;

namespace IssueTracker.Application.Interfaces;

/// <summary>
/// Service interface for issue operations
/// </summary>
public interface IIssueService
{
    /// <summary>
    /// Get all issues, optionally filtered by status
    /// </summary>
    Task<IEnumerable<IssueDto>> GetAllAsync(
        IssueStatus? status = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a paginated list of issues, optionally filtered by status
    /// </summary>
    Task<PagedResult<IssueDto>> GetPagedAsync(
        IssueStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single issue by ID
    /// </summary>
    Task<IssueDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new issue
    /// </summary>
    Task<IssueDto> CreateAsync(CreateIssueDto createDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an existing issue
    /// </summary>
    Task<IssueDto?> UpdateAsync(int id, UpdateIssueDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Mark an issue as resolved
    /// </summary>
    Task<IssueDto?> ResolveAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete an issue
    /// </summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
