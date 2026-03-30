using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.UpdateCategoryTags;

public sealed class UpdateCategoryTagsEndpoint(ICategoryService categoryService) : Endpoint<UpdateCategoryTagsRequest>
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService), "Category Service must not be null");

    public override void Configure()
    {
        Put(ApiPrefixes.CategoriesApiPrefix + "/{CategoryId}/tags");
        Roles("Learner");
    }

    public override async Task HandleAsync(UpdateCategoryTagsRequest request, CancellationToken token)
    {
        await _categoryService.UpdateCategoryTags(request.CategoryId, request.TagIds, token);
        await Send.OkAsync(cancellation: token);
    }
}
