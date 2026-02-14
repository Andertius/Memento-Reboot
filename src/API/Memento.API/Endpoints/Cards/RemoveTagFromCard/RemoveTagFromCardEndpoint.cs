using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.RemoveTagFromCard;

public sealed class RemoveTagFromCardEndpoint(ITagService tagService) : Endpoint<RemoveTagFromCardRequest>
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService), "Tag Service must not be null");

    public override void Configure()
    {
        Delete("/api/cards/{CardId}/tags/{TagId}");
        Roles("Learner");
    }

    public override async Task HandleAsync(RemoveTagFromCardRequest request, CancellationToken token)
    {
        await _tagService.RemoveTagFromCard(request.TagId, request.CardId);
        await Send.OkAsync(cancellation: token);
    }
}
