using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.AddTagsToCategory;

public sealed class AddTagsToCategoryEndpoint(ITagService tagService) : Endpoint<AddTagsToCategoryRequest>
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException("Tag Service must not be null", nameof(tagService));

    public override void Configure()
    {
        Post("/api/categories/{CategoryId}/tags");
        Roles("Learner");
    }

    public override async Task HandleAsync(AddTagsToCategoryRequest request, CancellationToken token)
    {
        await _tagService.AddTagsToCategory(request.CategoryId, request.TagIds);
        await Send.OkAsync();
    }
}
