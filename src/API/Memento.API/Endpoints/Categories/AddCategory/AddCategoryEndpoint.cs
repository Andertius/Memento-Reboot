using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.AddCategory;

public sealed class AddCategoryEndpoint(ICategoryService categoryService) : Endpoint<AddCategoryRequest>
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService), "Category Service must not be null");

    public override void Configure()
    {
        Post("/api/categories");
        Roles("Learner");
    }

    public override async Task HandleAsync(AddCategoryRequest request, CancellationToken token)
    {
        int id = await _categoryService.AddCategory(request.ToModel());

        if (id == 0)
        {
            AddError("Could not add category as it already exists");
            await Send.ErrorsAsync(cancellation: token);

            return;
        }

        await Send.CreatedAtAsync($"/api/categories/{id}", cancellation: token);
    }
}
