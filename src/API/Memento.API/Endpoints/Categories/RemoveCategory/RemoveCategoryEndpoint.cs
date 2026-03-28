using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.API.Handlers;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.RemoveCategory;

public sealed class RemoveCategoryEndpoint(ICategoryService categoryService, IImageHandler imageHandler) : Endpoint<RemoveCategoryRequest>
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService), "Card Service must not be null");
    private readonly IImageHandler _imageHandler = imageHandler ?? throw new ArgumentNullException(nameof(imageHandler), "Image handler must not be null");

    public override void Configure()
    {
        Delete(ApiPrefixes.CategoriesApiPrefix + "/{Id}");
        Roles("Learner");
    }

    // TODO remove hanging cards
    public override async Task HandleAsync(RemoveCategoryRequest request, CancellationToken token)
    {
        await _imageHandler.DeleteCategoryImageAsync(request.Id, token);
        await _categoryService.RemoveCategory(request.Id, token);
        await Send.OkAsync(cancellation: token);
    }
}
