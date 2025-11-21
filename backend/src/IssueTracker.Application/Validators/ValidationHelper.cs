using System.Text.RegularExpressions;

namespace IssueTracker.Application.Validators;

/// <summary>
/// Helper class for common validation rules to prevent XSS and injection attacks
/// </summary>
public static class ValidationHelper
{
    // Common HTML tags pattern
    private static readonly Regex HtmlTagPattern = new(@"<[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    // Dangerous content patterns that could lead to XSS
    private static readonly string[] DangerousPatterns = new[]
    {
        "<script",
        "</script",
        "javascript:",
        "onerror=",
        "onload=",
        "onclick=",
        "onmouseover=",
        "<iframe",
        "<object",
        "<embed",
        "<applet",
        "vbscript:",
        "data:text/html"
    };

    /// <summary>
    /// Checks if the input contains HTML tags
    /// </summary>
    public static bool ContainsHtml(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        return HtmlTagPattern.IsMatch(input);
    }

    /// <summary>
    /// Checks if the input contains potentially dangerous content (XSS vectors)
    /// </summary>
    public static bool ContainsDangerousContent(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        var lowerInput = input.ToLowerInvariant();
        return DangerousPatterns.Any(pattern => lowerInput.Contains(pattern));
    }

    /// <summary>
    /// Validates that input is safe from XSS attacks
    /// </summary>
    public static bool IsSafeFromXss(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return true; // Null/empty is considered safe

        return !ContainsHtml(input) && !ContainsDangerousContent(input);
    }
}
