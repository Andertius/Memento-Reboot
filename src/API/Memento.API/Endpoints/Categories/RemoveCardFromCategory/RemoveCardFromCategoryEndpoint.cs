using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.RemoveCardFromCategory;

public sealed class RemoveCardFromCategoryEndpoint(ICategoryService categoryService) : Endpoint<RemoveCardFromCategoryRequest>
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService), "Category Service must not be null");

    public override void Configure()
    {
        Delete(ApiPrefixes.CategoriesPrefix + "/{CategoryId}/cards/{CardId}");
        Roles("Learner");
    }

    public override async Task HandleAsync(RemoveCardFromCategoryRequest request, CancellationToken token)
    {
        await _categoryService.RemoveCardFromCategory(request.CategoryId, request.CardId);
        await Send.OkAsync(cancellation: token);
    }
}
