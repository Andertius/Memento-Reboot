using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Tags.UpdateTag;

namespace Memento.API.Validators.Tags;

public sealed class UpdateTagRequestValidator : Validator<UpdateTagRequest>
{
    public UpdateTagRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Tag name is required")
            .MaximumLength(256)
            .WithMessage("Tag name cannot be longer than 256 characters");
    }
}
