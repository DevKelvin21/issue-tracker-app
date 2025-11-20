using FluentValidation;
using IssueTracker.Application.DTOs;

namespace IssueTracker.Application.Validators;

/// <summary>
/// Validator for CreateIssueDto
/// </summary>
public class CreateIssueDtoValidator : AbstractValidator<CreateIssueDto>
{
    public CreateIssueDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters")
            .Must(title => !string.IsNullOrWhiteSpace(title))
                .WithMessage("Title cannot be only whitespace");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters")
            .Must(desc => !string.IsNullOrWhiteSpace(desc))
                .WithMessage("Description cannot be only whitespace");
    }
}
