using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.RemoveTagFromCategory;

public sealed class RemoveTagFromCategoryEndpoint(ITagService tagService) : Endpoint<RemoveTagFromCategoryRequest>
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException("Tag Service must not be null", nameof(tagService));

    public override void Configure()
    {
        Delete("/api/categories/{CategoryId}/tags/{TagId}");
        Roles("Learner");
    }

    public override async Task HandleAsync(RemoveTagFromCategoryRequest request, CancellationToken token)
    {
        await _tagService.RemoveTagFromCategory(request.TagId, request.CategoryId);
        await Send.OkAsync();
    }
}
