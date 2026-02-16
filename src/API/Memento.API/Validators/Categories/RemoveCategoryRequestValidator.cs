using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Categories.RemoveCategory;

namespace Memento.API.Validators.Categories;

public sealed class RemoveCategoryRequestValidator : Validator<RemoveCategoryRequest>
{
    public RemoveCategoryRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be a positive integer");
    }
}
