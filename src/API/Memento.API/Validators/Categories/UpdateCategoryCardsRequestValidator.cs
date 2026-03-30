using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Categories.UpdateCategoryCards;

namespace Memento.API.Validators.Categories;

public sealed class UpdateCategoryCardsRequestValidator : Validator<UpdateCategoryCardsRequest>
{
    public UpdateCategoryCardsRequestValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category id must be a positive integer");

        RuleFor(x => x.CardIds)
            .ForEach(x => x.GreaterThan(0).WithMessage("All card ids must be positive integers"));
    }
}
