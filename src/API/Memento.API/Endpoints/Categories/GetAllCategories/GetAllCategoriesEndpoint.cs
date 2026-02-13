using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.GetAllCategories;

public sealed class GetAllCategoriesEndpoint(ICategoryService categoryService) : EndpointWithoutRequest
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException("Category Service must not be null", nameof(categoryService));

    public override void Configure()
    {
        Get("/api/categories");
        Roles("Learner");
    }

    public override async Task HandleAsync(CancellationToken token)
    {
        var cards = await _categoryService.GetAllCategories();
        await Send.OkAsync(cards);
    }
}
