using IssueTracker.Application.DTOs;
using IssueTracker.Core.Enums;

namespace IssueTracker.Application.Interfaces;

/// <summary>
/// Service interface for issue operations
/// </summary>
public interface IIssueService
{
    /// <summary>
    /// Get all issues, optionally filtered by status
    /// </summary>
    Task<IEnumerable<IssueDto>> GetAllAsync(IssueStatus? status = null);

    /// <summary>
    /// Get a single issue by ID
    /// </summary>
    Task<IssueDto?> GetByIdAsync(int id);

    /// <summary>
    /// Create a new issue
    /// </summary>
    Task<IssueDto> CreateAsync(CreateIssueDto createDto);

    /// <summary>
    /// Update an existing issue
    /// </summary>
    Task<IssueDto?> UpdateAsync(int id, UpdateIssueDto updateDto);

    /// <summary>
    /// Mark an issue as resolved
    /// </summary>
    Task<IssueDto?> ResolveAsync(int id);

    /// <summary>
    /// Delete an issue
    /// </summary>
    Task<bool> DeleteAsync(int id);
}
