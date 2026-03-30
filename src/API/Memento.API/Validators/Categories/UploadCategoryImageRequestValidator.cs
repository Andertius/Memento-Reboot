using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Categories.UploadCategoryImage;

namespace Memento.API.Validators.Categories;

public sealed class UploadCategoryImageRequestValidator : Validator<UploadCategoryImageRequest>
{
    public UploadCategoryImageRequestValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId must be a positive integer");
    }
}
