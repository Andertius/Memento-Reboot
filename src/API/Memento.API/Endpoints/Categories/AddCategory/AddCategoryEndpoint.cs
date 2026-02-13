using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.AddCategory;

public sealed class AddCategoryEndpoint(ICategoryService categoryService) : Endpoint<AddCategoryRequest>
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException("Category Service must not be null", nameof(categoryService));

    public override void Configure()
    {
        Post("/api/categories");
        Roles("Learner");
    }

    public override async Task HandleAsync(AddCategoryRequest request, CancellationToken token)
    {
        int id = await _categoryService.AddCategory(request.ToModel());
        await Send.CreatedAtAsync($"/api/categories/{id}");
    }
}
