using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Categories.DeleteCategoryImage;

namespace Memento.API.Validators.Categories;

public sealed class DeleteCategoryImageRequestValidator : Validator<DeleteCategoryImageRequest>
{
    public DeleteCategoryImageRequestValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId must be a positive integer");
    }
}
