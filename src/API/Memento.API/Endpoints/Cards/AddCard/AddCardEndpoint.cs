using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.AddCard;

public sealed class AddCardEndpoint(ICardService cardService) : Endpoint<AddCardRequest>
{
    private readonly ICardService _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService), "Card Service must not be null");

    public override void Configure()
    {
        Post("/api/cards");
        Roles("Learner");
    }

    public override async Task HandleAsync(AddCardRequest request, CancellationToken token)
    {
        int id = await _cardService.AddCard(request.ToModel());
        await Send.CreatedAtAsync($"/api/cards/{id}", cancellation: token);
    }
}
