using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Tags.GetTagById;

namespace Memento.API.Validators.Tags;

public sealed class GetTagByIdRequestValidator : Validator<GetTagByIdRequest>
{
    public GetTagByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be a positive integer");
    }
}
