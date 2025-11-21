using AutoMapper;
using IssueTracker.Application.DTOs;
using IssueTracker.Application.Exceptions;
using IssueTracker.Application.Interfaces;
using IssueTracker.Core.Entities;
using IssueTracker.Core.Enums;
using IssueTracker.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace IssueTracker.Application.Services;

/// <summary>
/// Service for managing issue operations
/// </summary>
public class IssueService : IIssueService
{
    private readonly IIssueRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<IssueService> _logger;

    public IssueService(
        IIssueRepository repository,
        IMapper mapper,
        ILogger<IssueService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<IssueDto>> GetAllAsync(
        IssueStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all issues with status filter: {Status}", status?.ToString() ?? "None");

        var issues = await _repository.GetAllAsync(status, cancellationToken);

        return _mapper.Map<IEnumerable<IssueDto>>(issues);
    }

    public async Task<PagedResult<IssueDto>> GetPagedAsync(
        IssueStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Fetching issues page {PageNumber} with page size {PageSize} and status filter: {Status}",
            pageNumber, pageSize, status?.ToString() ?? "None");

        var pagedIssues = await _repository.GetPagedAsync(status, pageNumber, pageSize, cancellationToken);

        var dtos = _mapper.Map<IEnumerable<IssueDto>>(pagedIssues.Items);

        return new PagedResult<IssueDto>
        {
            Items = dtos,
            PageNumber = pagedIssues.PageNumber,
            PageSize = pagedIssues.PageSize,
            TotalCount = pagedIssues.TotalCount
        };
    }

    public async Task<IssueDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching issue with ID: {IssueId}", id);

        var issue = await _repository.GetByIdAsync(id, cancellationToken);

        if (issue == null)
        {
            _logger.LogWarning("Issue with ID {IssueId} not found", id);
            throw new NotFoundException("Issue", id);
        }

        return _mapper.Map<IssueDto>(issue);
    }

    public async Task<IssueDto> CreateAsync(CreateIssueDto createDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new issue with title: {Title}", createDto.Title);

        var issue = _mapper.Map<Issue>(createDto);

        await _repository.AddAsync(issue, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created issue with ID: {IssueId}", issue.Id);

        return _mapper.Map<IssueDto>(issue);
    }

    public async Task<IssueDto?> UpdateAsync(int id, UpdateIssueDto updateDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating issue with ID: {IssueId}", id);

        var issue = await _repository.GetByIdAsync(id, cancellationToken);

        if (issue == null)
        {
            _logger.LogWarning("Issue with ID {IssueId} not found for update", id);
            throw new NotFoundException("Issue", id);
        }

        // Map non-null properties from updateDto to issue
        if (!string.IsNullOrWhiteSpace(updateDto.Title))
        {
            issue.Title = updateDto.Title;
        }

        if (!string.IsNullOrWhiteSpace(updateDto.Description))
        {
            issue.Description = updateDto.Description;
        }

        // Apply status change business logic
        if (updateDto.Status.HasValue)
        {
            ApplyStatusChange(issue, updateDto.Status.Value);
        }

        await _repository.UpdateAsync(issue, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated issue with ID: {IssueId}", id);

        return _mapper.Map<IssueDto>(issue);
    }

    /// <summary>
    /// Applies status change business logic
    /// </summary>
    private void ApplyStatusChange(Issue issue, IssueStatus newStatus)
    {
        issue.Status = newStatus;

        // Set ResolvedAt if status is Resolved
        if (newStatus == IssueStatus.Resolved && issue.ResolvedAt == null)
        {
            issue.ResolvedAt = DateTime.UtcNow;
        }
        // Clear ResolvedAt if status is not Resolved
        else if (newStatus != IssueStatus.Resolved)
        {
            issue.ResolvedAt = null;
        }
    }

    public async Task<IssueDto?> ResolveAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Resolving issue with ID: {IssueId}", id);

        var issue = await _repository.GetByIdAsync(id, cancellationToken);

        if (issue == null)
        {
            _logger.LogWarning("Issue with ID {IssueId} not found for resolve", id);
            throw new NotFoundException("Issue", id);
        }

        issue.Status = IssueStatus.Resolved;
        issue.ResolvedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(issue, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Resolved issue with ID: {IssueId}", id);

        return _mapper.Map<IssueDto>(issue);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting issue with ID: {IssueId}", id);

        var issue = await _repository.GetByIdAsync(id, cancellationToken);

        if (issue == null)
        {
            _logger.LogWarning("Issue with ID {IssueId} not found for deletion", id);
            throw new NotFoundException("Issue", id);
        }

        await _repository.DeleteAsync(issue, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted issue with ID: {IssueId}", id);

        return true;
    }
}
