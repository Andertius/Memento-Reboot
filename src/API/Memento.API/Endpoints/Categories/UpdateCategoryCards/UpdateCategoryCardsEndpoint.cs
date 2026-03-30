using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.UpdateCategoryCards;

public sealed class UpdateCategoryCardsEndpoint(ICategoryService categoryService) : Endpoint<UpdateCategoryCardsRequest>
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService), "Category Service must not be null");

    public override void Configure()
    {
        Put(ApiPrefixes.CategoriesApiPrefix + "/{CategoryId}/cards");
        Roles("Learner");
    }

    // TODO remove hanging cards
    public override async Task HandleAsync(UpdateCategoryCardsRequest request, CancellationToken token)
    {
        await _categoryService.UpdateCategoryCards(request.CategoryId, request.CardIds, token);
        await Send.OkAsync(cancellation: token);
    }
}
