using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Categories.GetCardsByCategoryId;

namespace Memento.API.Validators.Categories;

public sealed class GetCardsByCategoryIdRequestValidator : Validator<GetCardsByCategoryIdRequest>
{
    public GetCardsByCategoryIdRequestValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category id must be a positive integer");
    }
}
