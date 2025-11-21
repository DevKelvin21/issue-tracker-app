using IssueTracker.Core.Entities;
using IssueTracker.Core.Enums;
using IssueTracker.Core.Interfaces;
using IssueTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Issue entity
/// </summary>
public class IssueRepository : IIssueRepository
{
    private readonly IssueTrackerDbContext _context;

    public IssueRepository(IssueTrackerDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Issue>> GetAllAsync(
        IssueStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Issues.AsNoTracking();

        if (status.HasValue)
        {
            query = query.Where(i => i.Status == status.Value);
        }

        return await query
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedResult<Issue>> GetPagedAsync(
        IssueStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Issues.AsNoTracking();

        if (status.HasValue)
        {
            query = query.Where(i => i.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Issue>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    /// <inheritdoc />
    public async Task<Issue?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Issues
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Issue> AddAsync(Issue issue, CancellationToken cancellationToken = default)
    {
        await _context.Issues.AddAsync(issue, cancellationToken);
        return issue;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Issue issue, CancellationToken cancellationToken = default)
    {
        _context.Issues.Update(issue);
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Issue issue, CancellationToken cancellationToken = default)
    {
        _context.Issues.Remove(issue);
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
