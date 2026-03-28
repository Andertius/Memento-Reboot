using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.RemoveCategory;

public sealed class RemoveCategoryEndpoint(ICategoryService categoryService, IImageService imageService) : Endpoint<RemoveCategoryRequest>
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService), "Card Service must not be null");
    private readonly IImageService _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService), "Image service must not be null");

    public override void Configure()
    {
        Delete(ApiPrefixes.CategoriesApiPrefix + "/{Id}");
        Roles("Learner");
    }

    // TODO remove hanging cards
    public override async Task HandleAsync(RemoveCategoryRequest request, CancellationToken token)
    {
        await _imageService.DeleteCategoryImageAsync(request.Id, token);
        await _categoryService.RemoveCategory(request.Id, token);
        await Send.OkAsync(cancellation: token);
    }
}
