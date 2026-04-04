using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.GetCards;

public sealed class GetCardsEndpoint(ICardService cardService) : Endpoint<GetCardsRequest>
{
    private readonly ICardService _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService), "Card Service must not be null");

    public override void Configure()
    {
        Get(ApiPrefixes.CardsApiPrefix);
        Roles("Learner");
    }

    public override async Task HandleAsync(GetCardsRequest request, CancellationToken token)
    {
        var cards = await _cardService.GetCards(request.CategoryId ?? 0, request.TagIds, token);
        await Send.OkAsync(cards, cancellation: token);
    }
}
