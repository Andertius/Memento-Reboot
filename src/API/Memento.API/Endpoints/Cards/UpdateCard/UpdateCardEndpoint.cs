using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.UpdateCard;

public sealed class UpdateCardEndpoint(ICardService cardService) : Endpoint<UpdateCardRequest>
{
    private readonly ICardService _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService), "Card Service must not be null");

    public override void Configure()
    {
        Put(ApiPrefixes.CardsPrefix);
        Roles("Learner");
    }

    public override async Task HandleAsync(UpdateCardRequest request, CancellationToken token)
    {
        await _cardService.UpdateCard(request.ToModel(), token);
        await Send.OkAsync(cancellation: token);
    }
}
