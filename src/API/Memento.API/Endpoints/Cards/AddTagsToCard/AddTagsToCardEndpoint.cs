using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.AddTagsToCard;

public sealed class AddTagsToCardEndpoint(ITagService tagService) : Endpoint<AddTagsToCardRequest>
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService), "Tag Service must not be null");

    public override void Configure()
    {
        Post("/api/cards/{CardId}/tags");
        Roles("Learner");
    }

    public override async Task HandleAsync(AddTagsToCardRequest request, CancellationToken token)
    {
        await _tagService.AddTagsToCard(request.CardId, request.TagIds);
        await Send.OkAsync(cancellation: token);
    }
}
