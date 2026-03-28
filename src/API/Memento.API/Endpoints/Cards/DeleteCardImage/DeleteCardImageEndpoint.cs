using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.DeleteCardImage;

public sealed class DeleteCardImageEndpoint(IImageService imageService) : Endpoint<DeleteCardImageRequest>
{
    private readonly IImageService _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService), "Image handler must not be null");

    public override void Configure()
    {
        Delete(ApiPrefixes.CardsApiPrefix + "/{CardId}/image");
        Roles("Learner");
    }

    public override async Task HandleAsync(DeleteCardImageRequest request, CancellationToken token)
    {
        await _imageService.DeleteCardImageAsync(request.CardId, token);
        await Send.OkAsync(cancellation: token);
    }
}
