using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.API.Handlers;

namespace Memento.API.Endpoints.Cards.DeleteCardImage;

public sealed class DeleteCardImageEndpoint(IImageHandler imageHandler) : Endpoint<DeleteCardImageRequest>
{
    private readonly IImageHandler _imageHandler = imageHandler ?? throw new ArgumentNullException(nameof(imageHandler), "Image handler must not be null");

    public override void Configure()
    {
        Delete(ApiPrefixes.CardsApiPrefix + "/{CardId}/image");
        Roles("Learner");
    }

    public override async Task HandleAsync(DeleteCardImageRequest request, CancellationToken token)
    {
        await _imageHandler.DeleteCardImageAsync(request.CardId, token);
        await Send.OkAsync(cancellation: token);
    }
}
