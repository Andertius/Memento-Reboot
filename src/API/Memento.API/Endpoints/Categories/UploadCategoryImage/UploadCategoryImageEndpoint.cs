using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.API.Handlers;

namespace Memento.API.Endpoints.Categories.UploadCategoryImage;

public sealed class UploadCategoryImageEndpoint(IImageHandler imageHandler) : Endpoint<UploadCategoryImageRequest>
{
    private readonly IImageHandler _imageHandler = imageHandler ?? throw new ArgumentNullException(nameof(imageHandler), "Image handler must not be null");

    public override void Configure()
    {
        Post(ApiPrefixes.CategoriesApiPrefix + "/{CategoryId}/image");
        Roles("Learner");
        AllowFileUploads();
    }

    public override async Task HandleAsync(UploadCategoryImageRequest request, CancellationToken token)
    {
        if (Files.Count == 0)
        {
            AddError("A png file must be provided.");
            await Send.ErrorsAsync(cancellation: token);

            return;
        }

        var file = Files[0];

        if (file.ContentType != ContentTypes.PngContentType)
        {
            AddError("Only png formats are supported.");
            await Send.ErrorsAsync(cancellation: token);

            return;
        }

        string? fileName = await _imageHandler.UploadCategoryImageAsync(file, request.CategoryId, token);
        await Send.OkAsync(new { fileName }, cancellation: token);
    }
}
