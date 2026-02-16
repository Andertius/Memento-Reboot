using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Categories.AddCategory;

namespace Memento.API.Validators.Categories;

public sealed class AddCategoryRequestValidator : Validator<AddCategoryEntityRequest>
{
    public AddCategoryRequestValidator()
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
