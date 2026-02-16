using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.GetAllCategories;

public sealed class GetAllCategoriesEndpoint(ICategoryService categoryService) : EndpointWithoutRequest
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService), "Category Service must not be null");

    public override void Configure()
    {
        Get(ApiPrefixes.CategoriesPrefix);
        Roles("Learner");
    }

    public override async Task HandleAsync(CancellationToken token)
    {
        var cards = await _categoryService.GetAllCategories(token);
        await Send.OkAsync(cards, cancellation: token);
    }
}
