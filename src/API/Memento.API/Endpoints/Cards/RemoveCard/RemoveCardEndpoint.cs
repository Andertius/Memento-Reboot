using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.API.Handlers;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.RemoveCard;

public sealed class RemoveCardEndpoint(ICardService cardService, IImageHandler imageHandler) : Endpoint<RemoveCardRequest>
{
    private readonly ICardService _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService), "Card Service must not be null");
    private readonly IImageHandler _imageHandler = imageHandler ?? throw new ArgumentNullException(nameof(imageHandler), "Image handler must not be null");

    public override void Configure()
    {
        Delete(ApiPrefixes.CardsApiPrefix + "/{Id}");
        Roles("Learner");
    }

    public override async Task HandleAsync(RemoveCardRequest request, CancellationToken token)
    {
        await _imageHandler.DeleteCardImageAsync(request.Id, token);
        await _cardService.RemoveCard(request.Id, token);
        await Send.OkAsync(cancellation: token);
    }
}
