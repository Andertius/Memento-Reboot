using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Cards.UpdateCardTags;

namespace Memento.API.Validators.Cards;

public sealed class UpdateCardTagsRequestValidator : Validator<UpdateCardTagsRequest>
{
    public UpdateCardTagsRequestValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .WithMessage("Card id must be a positive integer");

        RuleFor(x => x.TagIds)
            .ForEach(x => x.GreaterThan(0).WithMessage("All tag ids must be positive integers"));
    }
}
