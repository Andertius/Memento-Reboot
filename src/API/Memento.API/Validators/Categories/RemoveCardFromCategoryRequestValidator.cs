using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Categories.RemoveCardFromCategory;

namespace Memento.API.Validators.Categories;

public sealed class RemoveCardFromCategoryRequestValidator : Validator<RemoveCardFromCategoryRequest>
{
    public RemoveCardFromCategoryRequestValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category id must be a positive integer");

        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .WithMessage("Card id must be a positive integer");
    }
}
