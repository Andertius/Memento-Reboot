using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Categories.UpdateCategory;

public sealed class UpdateCategoryEndpoint(ICategoryService categoryService) : Endpoint<UpdateCategoryRequest>
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService), "Category Service must not be null");

    public override void Configure()
    {
        Put(ApiPrefixes.CategoriesPrefix);
        Roles("Learner");
    }

    public override async Task HandleAsync(UpdateCategoryRequest request, CancellationToken token)
    {
        bool result = await _categoryService.UpdateCategory(request.ToModel(), token);

        if (result)
        {
            await Send.OkAsync(cancellation: token);

            return;
        }

        AddError($"Category '{request.Name}' already exists");
        await Send.ErrorsAsync(cancellation: token);
    }
}
