using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.UploadCardImage;

public sealed class UploadCardImageEndpoint(IImageService imageService) : Endpoint<UploadCardImageRequest>
{
    private readonly IImageService _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService), "Image service must not be null");

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

        string? fileName = await _imageService.UploadCardImageAsync(file.OpenReadStream(), file.FileName, request.CardId, token);
        await Send.OkAsync(new { fileName }, cancellation: token);
    }
}
