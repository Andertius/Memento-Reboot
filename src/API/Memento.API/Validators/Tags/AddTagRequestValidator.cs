using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Tags.AddTag;

namespace Memento.API.Validators.Tags;

public sealed class AddTagRequestValidator : Validator<AddTagRequest>
{
    public AddTagRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Tag name is required")
            .MaximumLength(256)
            .WithMessage("Tag name cannot be longer than 256 characters");
    }
}
