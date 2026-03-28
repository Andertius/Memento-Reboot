using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.GetCategoryByName;

public sealed class GetCategoryByNameEndpoint(ICategoryService categoryService) : Endpoint<GetCategoryByNameRequest>
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService), "Category Service must not be null");

    public override void Configure()
    {
        Get(ApiPrefixes.CategoriesApiPrefix + "/name/{Name}");
        Roles("Learner");
    }

    public override async Task HandleAsync(GetCategoryByNameRequest request, CancellationToken token)
    {
        var category = await _categoryService.GetByName(request.Name, token);

        if (category is null)
        {
            await Send.NotFoundAsync(cancellation: token);

            return;
        }

        await Send.OkAsync(category, cancellation: token);
    }
}
