using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.AddCategory;

public sealed class AddCategoryEndpoint(ICategoryService categoryService) : Endpoint<AddCategoryEntityRequest>
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService), "Category Service must not be null");

    public override void Configure()
    {
        Post(ApiPrefixes.CategoriesPrefix);
        Roles("Learner");
    }

    public override async Task HandleAsync(AddCategoryEntityRequest entityRequest, CancellationToken token)
    {
        int id = await _categoryService.AddCategory(entityRequest.ToModel());

        if (id == 0)
        {
            AddError("Could not add category as it already exists");
            await Send.ErrorsAsync(cancellation: token);

            return;
        }

        await Send.CreatedAtAsync($"{ApiPrefixes.CategoriesPrefix}/{id}", cancellation: token);
    }
}
