using IssueTracker.Application.DTOs;
using IssueTracker.Application.Interfaces;
using IssueTracker.Core.Enums;
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
    /// Get all issues, optionally filtered by status
    /// </summary>
    /// <param name="status">Optional status filter (Open, InProgress, Resolved)</param>
    /// <returns>List of issues</returns>
    /// <response code="200">Returns the list of issues</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IssueDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IssueDto>>> GetAll([FromQuery] IssueStatus? status = null)
    {
        _logger.LogInformation("Getting all issues with status filter: {Status}", status?.ToString() ?? "None");

        var issues = await _issueService.GetAllAsync(status);
        return Ok(issues);
    }

    /// <summary>
    /// Get a specific issue by ID
    /// </summary>
    /// <param name="id">Issue ID</param>
    /// <returns>The requested issue</returns>
    /// <response code="200">Returns the issue</response>
    /// <response code="404">Issue not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IssueDto>> GetById(int id)
    {
        _logger.LogInformation("Getting issue with ID: {IssueId}", id);

        var issue = await _issueService.GetByIdAsync(id);

        if (issue == null)
        {
            _logger.LogWarning("Issue with ID {IssueId} not found", id);
            return NotFound(new { message = $"Issue with ID {id} not found" });
        }

        return Ok(issue);
    }

    /// <summary>
    /// Create a new issue
    /// </summary>
    /// <param name="createDto">Issue creation details</param>
    /// <returns>The created issue</returns>
    /// <response code="201">Issue created successfully</response>
    /// <response code="400">Invalid request data</response>
    [HttpPost]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IssueDto>> Create([FromBody] CreateIssueDto createDto)
    {
        _logger.LogInformation("Creating new issue with title: {Title}", createDto.Title);

        var issue = await _issueService.CreateAsync(createDto);

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
    /// <returns>The updated issue</returns>
    /// <response code="200">Issue updated successfully</response>
    /// <response code="404">Issue not found</response>
    /// <response code="400">Invalid request data</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IssueDto>> Update(int id, [FromBody] UpdateIssueDto updateDto)
    {
        _logger.LogInformation("Updating issue with ID: {IssueId}", id);

        var issue = await _issueService.UpdateAsync(id, updateDto);

        if (issue == null)
        {
            _logger.LogWarning("Issue with ID {IssueId} not found for update", id);
            return NotFound(new { message = $"Issue with ID {id} not found" });
        }

        return Ok(issue);
    }

    /// <summary>
    /// Mark an issue as resolved
    /// </summary>
    /// <param name="id">Issue ID</param>
    /// <returns>The resolved issue</returns>
    /// <response code="200">Issue resolved successfully</response>
    /// <response code="404">Issue not found</response>
    [HttpPatch("{id}/resolve")]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IssueDto>> Resolve(int id)
    {
        _logger.LogInformation("Resolving issue with ID: {IssueId}", id);

        var issue = await _issueService.ResolveAsync(id);

        if (issue == null)
        {
            _logger.LogWarning("Issue with ID {IssueId} not found for resolve", id);
            return NotFound(new { message = $"Issue with ID {id} not found" });
        }

        return Ok(issue);
    }

    /// <summary>
    /// Delete an issue
    /// </summary>
    /// <param name="id">Issue ID</param>
    /// <returns>No content</returns>
    /// <response code="204">Issue deleted successfully</response>
    /// <response code="404">Issue not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting issue with ID: {IssueId}", id);

        var result = await _issueService.DeleteAsync(id);

        if (!result)
        {
            _logger.LogWarning("Issue with ID {IssueId} not found for deletion", id);
            return NotFound(new { message = $"Issue with ID {id} not found" });
        }

        return NoContent();
    }
}
