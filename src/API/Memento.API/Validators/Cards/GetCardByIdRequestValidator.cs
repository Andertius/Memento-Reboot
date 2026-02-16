using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Cards.GetCardById;

namespace Memento.API.Validators.Cards;

public sealed class GetCardByIdRequestValidator : Validator<GetCardByIdRequest>
{
    public GetCardByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be a positive integer");
    }
}
