using FluentValidation;
using IssueTracker.Application.DTOs;

namespace IssueTracker.Application.Validators;

/// <summary>
/// Validator for UpdateIssueDto
/// </summary>
public class UpdateIssueDtoValidator : AbstractValidator<UpdateIssueDto>
{
    public UpdateIssueDtoValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters")
            .Must(title => string.IsNullOrEmpty(title) || !string.IsNullOrWhiteSpace(title))
                .WithMessage("Title cannot be only whitespace")
            .When(x => x.Title != null);

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters")
            .Must(desc => string.IsNullOrEmpty(desc) || !string.IsNullOrWhiteSpace(desc))
                .WithMessage("Description cannot be only whitespace")
            .When(x => x.Description != null);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status value")
            .When(x => x.Status.HasValue);
    }
}
