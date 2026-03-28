using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.UploadCategoryImage;

public sealed class UploadCategoryImageEndpoint(IImageService imageService) : Endpoint<UploadCategoryImageRequest>
{
    private readonly IImageService _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService), "Image service must not be null");

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

        string? fileName = await _imageService.UploadCategoryImageAsync(file.OpenReadStream(), file.FileName, request.CategoryId, token);
        await Send.OkAsync(new { fileName }, cancellation: token);
    }
}
