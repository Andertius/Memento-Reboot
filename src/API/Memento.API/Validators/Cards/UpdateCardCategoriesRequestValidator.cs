using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Cards.UpdateCardCategories;

namespace Memento.API.Validators.Cards;

public sealed class UpdateCardCategoriesRequestValidator : Validator<UpdateCardCategoriesRequest>
{
    public UpdateCardCategoriesRequestValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .WithMessage("Card id must be a positive integer");

        RuleFor(x => x.CategoryIds)
            .ForEach(x => x.GreaterThan(0).WithMessage("All category ids must be positive integers"));
    }
}
