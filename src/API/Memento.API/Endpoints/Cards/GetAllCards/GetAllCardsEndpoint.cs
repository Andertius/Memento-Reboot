using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.GetAllCards;

public sealed class GetAllCardsEndpoint(ICardService cardService) : EndpointWithoutRequest
{
    private readonly ICardService _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService), "Card Service must not be null");

    public override void Configure()
    {
        Get("/api/cards");
        Roles("Learner");
    }

    public override async Task HandleAsync(CancellationToken token)
    {
        var cards = await _cardService.GetAllCards();
        await Send.OkAsync(cards, cancellation: token);
    }
}
