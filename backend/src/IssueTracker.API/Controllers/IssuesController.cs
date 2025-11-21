using IssueTracker.Application.DTOs;
using IssueTracker.Application.Interfaces;
using IssueTracker.Core.Enums;
using IssueTracker.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.API.Controllers;

/// <summary>
/// Controller for managing issues
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class IssuesController : ControllerBase
{
    private readonly IIssueService _issueService;
    private readonly ILogger<IssuesController> _logger;

    public IssuesController(IIssueService issueService, ILogger<IssuesController> logger)
    {
        _issueService = issueService;
        _logger = logger;
    }

    /// <summary>
    /// Get a paginated list of issues, optionally filtered by status
    /// </summary>
    /// <param name="status">Optional status filter (Open, InProgress, Resolved)</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 20, max: 100)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of issues</returns>
    /// <response code="200">Returns the paginated list of issues</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<IssueDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<IssueDto>>> GetAll(
        [FromQuery] IssueStatus? status = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        // Enforce limits
        if (pageSize > 100) pageSize = 100;
        if (pageSize < 1) pageSize = 20;
        if (pageNumber < 1) pageNumber = 1;

        _logger.LogInformation(
            "Getting issues page {PageNumber} with status filter: {Status}",
            pageNumber, status?.ToString() ?? "None");

        var result = await _issueService.GetPagedAsync(status, pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get a specific issue by ID
    /// </summary>
    /// <param name="id">Issue ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The requested issue</returns>
    /// <response code="200">Returns the issue</response>
    /// <response code="404">Issue not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IssueDto>> GetById(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting issue with ID: {IssueId}", id);

        var issue = await _issueService.GetByIdAsync(id, cancellationToken);
        return Ok(issue);
    }

    /// <summary>
    /// Create a new issue
    /// </summary>
    /// <param name="createDto">Issue creation details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created issue</returns>
    /// <response code="201">Issue created successfully</response>
    /// <response code="400">Invalid request data</response>
    [HttpPost]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IssueDto>> Create(
        [FromBody] CreateIssueDto createDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new issue with title: {Title}", createDto.Title);

        var issue = await _issueService.CreateAsync(createDto, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = issue.Id },
            issue);
    }

    /// <summary>
    /// Update an existing issue
    /// </summary>
    /// <param name="id">Issue ID</param>
    /// <param name="updateDto">Issue update details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated issue</returns>
    /// <response code="200">Issue updated successfully</response>
    /// <response code="404">Issue not found</response>
    /// <response code="400">Invalid request data</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IssueDto>> Update(
        int id,
        [FromBody] UpdateIssueDto updateDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating issue with ID: {IssueId}", id);

        var issue = await _issueService.UpdateAsync(id, updateDto, cancellationToken);
        return Ok(issue);
    }

    /// <summary>
    /// Mark an issue as resolved
    /// </summary>
    /// <param name="id">Issue ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The resolved issue</returns>
    /// <response code="200">Issue resolved successfully</response>
    /// <response code="404">Issue not found</response>
    [HttpPatch("{id}/resolve")]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IssueDto>> Resolve(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Resolving issue with ID: {IssueId}", id);

        var issue = await _issueService.ResolveAsync(id, cancellationToken);
        return Ok(issue);
    }

    /// <summary>
    /// Delete an issue
    /// </summary>
    /// <param name="id">Issue ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Issue deleted successfully</response>
    /// <response code="404">Issue not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting issue with ID: {IssueId}", id);

        await _issueService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
