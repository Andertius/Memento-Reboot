using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.AddCard;

public sealed class AddCardEndpoint(ICardService cardService) : Endpoint<AddCardEntityRequest>
{
    private readonly ICardService _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService), "Card Service must not be null");

    public override void Configure()
    {
        Post(ApiPrefixes.CardsPrefix);
        Roles("Learner");
    }

    public override async Task HandleAsync(AddCardEntityRequest entityRequest, CancellationToken token)
    {
        int id = await _cardService.AddCard(entityRequest.ToModel());
        await Send.CreatedAtAsync($"{ApiPrefixes.CardsPrefix}/{id}", cancellation: token);
    }
}
