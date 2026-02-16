using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.AddCardsToCategory;

public sealed class AddCardsToCategoryEndpoint(ICategoryService categoryService) : Endpoint<AddCardsToCategoryRequest>
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService), "Category Service must not be null");

    public override void Configure()
    {
        Post(ApiPrefixes.CategoriesPrefix + "/{CategoryId}/cards");
        Roles("Learner");
    }

    public override async Task HandleAsync(AddCardsToCategoryRequest request, CancellationToken token)
    {
        await _categoryService.AddCardsToCategory(request.CategoryId, request.CardIds);
        await Send.OkAsync(cancellation: token);
    }
}
