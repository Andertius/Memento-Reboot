using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Categories.RemoveTagFromCategory;

namespace Memento.API.Validators.Categories;

public class RemoveTagFromCategoryRequestValidator : Validator<RemoveTagFromCategoryRequest>
{
    public RemoveTagFromCategoryRequestValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category id must be a positive integer");

        RuleFor(x => x.TagId)
            .GreaterThan(0)
            .WithMessage("Tag id must be a positive integer");
    }
}
