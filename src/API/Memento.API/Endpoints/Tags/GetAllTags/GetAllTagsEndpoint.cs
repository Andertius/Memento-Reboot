using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Tags.GetAllTags;

public sealed class GetAllTagsEndpoint(ITagService tagService) : EndpointWithoutRequest
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException("Tag Service must not be null", nameof(tagService));

    public override void Configure()
    {
        Get("/api/tags");
        Roles("Learner");
    }

    public override async Task HandleAsync(CancellationToken token)
    {
        var tags = await _tagService.GetAllTags();
        await Send.OkAsync(tags);
    }
}
