using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.RemoveTagFromCategory;

public sealed class RemoveTagFromCategoryEndpoint(ITagService tagService) : Endpoint<RemoveTagFromCategoryRequest>
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService), "Tag Service must not be null");

    public override void Configure()
    {
        Delete(ApiPrefixes.CategoriesPrefix + "/{CategoryId}/tags/{TagId}");
        Roles("Learner");
    }

    public override async Task HandleAsync(RemoveTagFromCategoryRequest request, CancellationToken token)
    {
        await _tagService.RemoveTagFromCategory(request.TagId, request.CategoryId);
        await Send.OkAsync(cancellation: token);
    }
}
