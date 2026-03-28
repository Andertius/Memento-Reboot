using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.AddTagsToCategory;

public sealed class AddTagsToCategoryEndpoint(ITagService tagService) : Endpoint<AddTagsToCategoryRequest>
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService), "Tag Service must not be null");

    public override void Configure()
    {
        Post(ApiPrefixes.CategoriesApiPrefix + "/{CategoryId}/tags");
        Roles("Learner");
    }

    public override async Task HandleAsync(AddTagsToCategoryRequest request, CancellationToken token)
    {
        await _tagService.AddTagsToCategory(request.CategoryId, request.TagIds, token);
        await Send.OkAsync(cancellation: token);
    }
}
