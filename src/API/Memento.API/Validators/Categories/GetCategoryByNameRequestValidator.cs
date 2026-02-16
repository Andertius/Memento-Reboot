using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Categories.GetCategoryByName;

namespace Memento.API.Validators.Categories;

public sealed class GetCategoryByNameRequestValidator : Validator<GetCategoryByNameRequest>
{
    public GetCategoryByNameRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Category name cannot be empty");
    }
}
