using AutoMapper;
using IssueTracker.Application.DTOs;
using IssueTracker.Application.Interfaces;
using IssueTracker.Core.Entities;
using IssueTracker.Core.Enums;
using IssueTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IssueTracker.Application.Services;

/// <summary>
/// Service for managing issue operations
/// </summary>
public class IssueService : IIssueService
{
    private readonly IssueTrackerDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<IssueService> _logger;

    public IssueService(
        IssueTrackerDbContext context,
        IMapper mapper,
        ILogger<IssueService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<IssueDto>> GetAllAsync(IssueStatus? status = null)
    {
        _logger.LogInformation("Fetching all issues with status filter: {Status}", status?.ToString() ?? "None");

        var query = _context.Issues.AsNoTracking();

        if (status.HasValue)
        {
            query = query.Where(i => i.Status == status.Value);
        }

        var issues = await query
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();

        return _mapper.Map<IEnumerable<IssueDto>>(issues);
    }

    public async Task<IssueDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching issue with ID: {IssueId}", id);

        var issue = await _context.Issues
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);

        if (issue == null)
        {
            _logger.LogWarning("Issue with ID {IssueId} not found", id);
            return null;
        }

        return _mapper.Map<IssueDto>(issue);
    }

    public async Task<IssueDto> CreateAsync(CreateIssueDto createDto)
    {
        _logger.LogInformation("Creating new issue with title: {Title}", createDto.Title);

        var issue = _mapper.Map<Issue>(createDto);

        _context.Issues.Add(issue);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created issue with ID: {IssueId}", issue.Id);

        return _mapper.Map<IssueDto>(issue);
    }

    public async Task<IssueDto?> UpdateAsync(int id, UpdateIssueDto updateDto)
    {
        _logger.LogInformation("Updating issue with ID: {IssueId}", id);

        var issue = await _context.Issues.FindAsync(id);

        if (issue == null)
        {
            _logger.LogWarning("Issue with ID {IssueId} not found for update", id);
            return null;
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

        if (updateDto.Status.HasValue)
        {
            issue.Status = updateDto.Status.Value;

            // Set ResolvedAt if status is Resolved
            if (updateDto.Status.Value == IssueStatus.Resolved && issue.ResolvedAt == null)
            {
                issue.ResolvedAt = DateTime.UtcNow;
            }
            // Clear ResolvedAt if status is not Resolved
            else if (updateDto.Status.Value != IssueStatus.Resolved)
            {
                issue.ResolvedAt = null;
            }
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated issue with ID: {IssueId}", id);

        return _mapper.Map<IssueDto>(issue);
    }

    public async Task<IssueDto?> ResolveAsync(int id)
    {
        _logger.LogInformation("Resolving issue with ID: {IssueId}", id);

        var issue = await _context.Issues.FindAsync(id);

        if (issue == null)
        {
            _logger.LogWarning("Issue with ID {IssueId} not found for resolve", id);
            return null;
        }

        issue.Status = IssueStatus.Resolved;
        issue.ResolvedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Resolved issue with ID: {IssueId}", id);

        return _mapper.Map<IssueDto>(issue);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting issue with ID: {IssueId}", id);

        var issue = await _context.Issues.FindAsync(id);

        if (issue == null)
        {
            _logger.LogWarning("Issue with ID {IssueId} not found for deletion", id);
            return false;
        }

        _context.Issues.Remove(issue);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted issue with ID: {IssueId}", id);

        return true;
    }
}
