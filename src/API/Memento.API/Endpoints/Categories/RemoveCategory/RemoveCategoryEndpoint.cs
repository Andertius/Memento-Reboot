using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Cards.RemoveCategory;

public sealed class RemoveCategoryEndpoint(ICategoryService categoryService) : Endpoint<RemoveCategoryRequest>
{
    private readonly ICategoryService _categoryService = categoryService ?? throw new ArgumentNullException("Card Service must not be null", nameof(categoryService));

    public override void Configure()
    {
        Delete("/api/categories/{Id}");
        Roles("Learner");
    }

    // TODO remove hanging cards
    public override async Task HandleAsync(RemoveCategoryRequest request, CancellationToken token)
    {
        await _categoryService.RemoveCategory(request.Id);
        await Send.OkAsync();
    }
}
