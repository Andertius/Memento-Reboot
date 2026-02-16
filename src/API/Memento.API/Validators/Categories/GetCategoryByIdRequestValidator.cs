using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Categories.GetCategoryById;

namespace Memento.API.Validators.Categories;

public sealed class GetCategoryByIdRequestValidator : Validator<GetCategoryByIdRequest>
{
    public GetCategoryByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be a positive integer");
    }
}
