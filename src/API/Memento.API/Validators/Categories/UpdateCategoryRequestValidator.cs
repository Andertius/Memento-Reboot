using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Categories.UpdateCategory;

namespace Memento.API.Validators.Categories;

public class UpdateCategoryRequestValidator : Validator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Category name is required")
            .MaximumLength(256)
            .WithMessage("Category name cannot be longer than 256 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Category description is required")
            .MaximumLength(256)
            .WithMessage("Category description cannot be longer than 256 characters");
    }
}
