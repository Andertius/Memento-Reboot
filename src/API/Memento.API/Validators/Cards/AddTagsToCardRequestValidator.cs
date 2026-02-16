using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Cards.AddTagsToCard;

namespace Memento.API.Validators.Cards;

public sealed class AddTagsToCardRequestValidator : Validator<AddTagsToCardRequest>
{
    public AddTagsToCardRequestValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .WithMessage("Card id must be a positive integer");

        RuleFor(x => x.TagIds)
            .NotEmpty()
            .WithMessage("At least one tag must be added to a card")
            .ForEach(x => x.GreaterThan(0).WithMessage("All tag ids must be positive integers"));
    }
}
