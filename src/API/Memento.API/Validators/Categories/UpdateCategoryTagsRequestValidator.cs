using FastEndpoints;
using FluentValidation;
using Memento.API.Endpoints.Categories.UpdateCategoryTags;

namespace Memento.API.Validators.Categories;

public sealed class UpdateCategoryTagsRequestValidator : Validator<UpdateCategoryTagsRequest>
{
    public UpdateCategoryTagsRequestValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category id must be a positive integer");

        RuleFor(x => x.TagIds)
            .ForEach(x => x.GreaterThan(0).WithMessage("All tag ids must be positive integers"));
    }
}
