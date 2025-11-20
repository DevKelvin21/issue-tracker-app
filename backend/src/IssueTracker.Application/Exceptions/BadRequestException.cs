namespace IssueTracker.Application.Exceptions;

/// <summary>
/// Exception thrown when a request is invalid or malformed
/// </summary>
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}
