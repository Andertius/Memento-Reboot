using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.GetCardById;

public sealed class GetCardByIdndpoint(ICardService cardService) : Endpoint<GetCardByIdRequest>
{
    private readonly ICardService _cardService = cardService ?? throw new ArgumentNullException("Card Service must not be null", nameof(cardService));

    public override void Configure()
    {
        Get("/api/cards/{Id}");
        Roles("Learner");
    }

    public override async Task HandleAsync(GetCardByIdRequest request, CancellationToken token)
    {
        var card = await _cardService.GetById(request.Id);
        await Send.OkAsync(card);
    }
}
