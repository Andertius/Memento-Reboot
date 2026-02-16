using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Cards.RemoveTagFromCard;

namespace Memento.API.Validators.Cards;

public sealed class RemoveTagFromCardRequestValidator : Validator<RemoveTagFromCardRequest>
{
    public RemoveTagFromCardRequestValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .WithMessage("Card id must be a positive integer");

        RuleFor(x => x.TagId)
            .GreaterThan(0)
            .WithMessage("Tag id must be a positive integer");
    }
}
