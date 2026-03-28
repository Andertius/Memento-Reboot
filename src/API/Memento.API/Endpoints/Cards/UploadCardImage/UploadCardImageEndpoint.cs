using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.API.Handlers;

namespace Memento.API.Endpoints.Cards.UploadCardImage;

public sealed class UploadCardImageEndpoint(IImageHandler imageHandler) : Endpoint<UploadCardImageRequest>
{
    private readonly IImageHandler _imageHandler = imageHandler ?? throw new ArgumentNullException(nameof(imageHandler), "Image handler must not be null");

    public override void Configure()
    {
        Post(ApiPrefixes.CardsApiPrefix + "/{CardId}/image");
        Roles("Learner");
        AllowFileUploads();
    }

    public override async Task HandleAsync(UploadCardImageRequest request, CancellationToken token)
    {
        if (Files.Count == 0)
        {
            AddError("An image file must be provided.");
            await Send.ErrorsAsync(cancellation: token);

            return;
        }

        var file = Files[0];

        if (!file.ContentType.StartsWith("image"))
        {
            AddError("Only image formats are supported (i.e. .png, .jpg, .bmp).");
            await Send.ErrorsAsync(cancellation: token);

            return;
        }

        string? fileName = await _imageHandler.UploadCardImageAsync(file, request.CardId, token);
        await Send.OkAsync(new { fileName }, cancellation: token);
    }
}
