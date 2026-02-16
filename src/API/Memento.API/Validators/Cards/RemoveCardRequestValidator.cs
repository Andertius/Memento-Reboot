using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Cards.RemoveCard;

namespace Memento.API.Validators.Cards;

public sealed class RemoveCardRequestValidator : Validator<RemoveCardRequest>
{
    public RemoveCardRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be a positive integer");
    }
}
