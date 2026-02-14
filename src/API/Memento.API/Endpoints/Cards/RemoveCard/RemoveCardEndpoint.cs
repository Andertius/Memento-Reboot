using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.RemoveCard;

public sealed class RemoveCardEndpoint(ICardService cardService) : Endpoint<RemoveCardRequest>
{
    private readonly ICardService _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService), "Card Service must not be null");

    public override void Configure()
    {
        Delete("/api/cards/{Id}");
        Roles("Learner");
    }

    public override async Task HandleAsync(RemoveCardRequest request, CancellationToken token)
    {
        await _cardService.RemoveCard(request.Id);
        await Send.OkAsync(cancellation: token);
    }
}
