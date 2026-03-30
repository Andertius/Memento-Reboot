using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.UpdateCardTags;

public sealed class UpdateCardTagsEndpoint(ICardService cardService) : Endpoint<UpdateCardTagsRequest>
{
    private readonly ICardService _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService), "Card Service must not be null");

    public override void Configure()
    {
        Put(ApiPrefixes.CardsApiPrefix + "/{CardId}/tags");
        Roles("Learner");
    }

    public override async Task HandleAsync(UpdateCardTagsRequest request, CancellationToken token)
    {
        await _cardService.UpdateCardTags(request.CardId, request.TagIds, token);
        await Send.OkAsync(cancellation: token);
    }
}
