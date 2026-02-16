using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Categories.AddTagsToCategory;

namespace Memento.API.Validators.Categories;

public sealed class AddTagsToCategoryRequestValidator : Validator<AddTagsToCategoryRequest>
{
    public AddTagsToCategoryRequestValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category id must be a positive integer");

        RuleFor(x => x.TagIds)
            .NotEmpty()
            .WithMessage("At least one tag must be added to a category")
            .ForEach(x => x.GreaterThan(0).WithMessage("All tag ids must be positive integers"));
    }
}
