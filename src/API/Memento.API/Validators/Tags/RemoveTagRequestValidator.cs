using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Tags.RemoveTag;

namespace Memento.API.Validators.Tags;

public sealed class RemoveTagRequestValidator : Validator<RemoveTagRequest>
{
    public RemoveTagRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be a positive integer");
    }
}
