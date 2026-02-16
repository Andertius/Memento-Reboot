using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Tags.GetTagByName;

namespace Memento.API.Validators.Tags;

public sealed class GetTagByNameRequestValidator : Validator<GetTagByNameRequest>
{
    public GetTagByNameRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Tag name cannot be empty");
    }
}
