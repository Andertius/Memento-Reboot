using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.DeleteCategoryImage;

public sealed class DeleteCategoryImageEndpoint(IImageService imageService) : Endpoint<DeleteCategoryImageRequest>
{
    private readonly IImageService _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService), "Image service must not be null");

    public override void Configure()
    {
        Delete(ApiPrefixes.CategoriesApiPrefix + "/{CategoryId}/image");
        Roles("Learner");
    }

    public override async Task HandleAsync(DeleteCategoryImageRequest request, CancellationToken token)
    {
        await _imageService.DeleteCategoryImageAsync(request.CategoryId, token);
        await Send.OkAsync(cancellation: token);
    }
}
