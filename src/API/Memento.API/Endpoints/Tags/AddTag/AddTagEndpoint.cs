using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Tags.AddTag;

public sealed class AddTagEndpoint(ITagService tagService) : Endpoint<AddTagRequest>
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException("Tag Service must not be null", nameof(tagService));

    public override void Configure()
    {
        Post("/api/tags");
        Roles("Learner");
    }

    public override async Task HandleAsync(AddTagRequest request, CancellationToken token)
    {
        int id = await _tagService.AddTag(request.ToModel());
        await Send.CreatedAtAsync($"/api/categories/{id}");
    }
}
