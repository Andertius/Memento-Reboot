using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Categories.AddCardsToCategory;

namespace Memento.API.Validators.Categories;

public sealed class AddCardsToCategoryRequestValidator : Validator<AddCardsToCategoryRequest>
{
    public AddCardsToCategoryRequestValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category id must be a positive integer");

        RuleFor(x => x.CardIds)
            .NotEmpty()
            .WithMessage("At least one card must be added to a category")
            .ForEach(x => x.GreaterThan(0).WithMessage("All card ids must be positive integers"));
    }
}
