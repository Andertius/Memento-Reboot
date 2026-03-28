using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.API.Handlers;

namespace Memento.API.Endpoints.Categories.DeleteCategoryImage;

public sealed class DeleteCategoryImageEndpoint(IImageHandler imageHandler) : Endpoint<DeleteCategoryImageRequest>
{
    private readonly IImageHandler _imageHandler = imageHandler ?? throw new ArgumentNullException(nameof(imageHandler), "Image handler must not be null");

    public override void Configure()
    {
        Delete(ApiPrefixes.CategoriesApiPrefix + "/{CategoryId}/image");
        Roles("Learner");
    }

    public override async Task HandleAsync(DeleteCategoryImageRequest request, CancellationToken token)
    {
        await _imageHandler.DeleteCategoryImageAsync(request.CategoryId, token);
        await Send.OkAsync(cancellation: token);
    }
}
