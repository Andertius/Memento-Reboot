using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.GetAllCategories;

public sealed class GetAllCategoriesEndpoint(ICategoryService categoryService) : Endpoint<GetAllCategoriesRequest>
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService), "Category Service must not be null");

    public override void Configure()
    {
        Get(ApiPrefixes.CategoriesApiPrefix);
        Roles("Learner");
    }

    public override async Task HandleAsync(GetAllCategoriesRequest request, CancellationToken token)
    {
        var categories = await _categoryService.GetAllCategories(request.Filter, request.Take, request.Skip, token);
        await Send.OkAsync(categories, cancellation: token);
    }
}
