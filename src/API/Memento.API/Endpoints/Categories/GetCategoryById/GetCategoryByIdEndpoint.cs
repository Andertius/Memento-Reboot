using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.GetCategoryById;

public sealed class GetCategoryByIdEndpoint(ICategoryService categoryService) : Endpoint<GetCategoryByIdRequest>
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService), "Category Service must not be null");

    public override void Configure()
    {
        Get(ApiPrefixes.CategoriesApiPrefix + "/{Id}");
        Roles("Learner");
    }

    public override async Task HandleAsync(GetCategoryByIdRequest request, CancellationToken token)
    {
        var category = await _categoryService.GetById(request.Id, token);

        if (category is null)
        {
            await Send.NotFoundAsync(cancellation: token);

            return;
        }

        await Send.OkAsync(category, cancellation: token);
    }
}
