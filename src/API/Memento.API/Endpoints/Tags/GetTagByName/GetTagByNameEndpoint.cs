using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Tags.GetTagByName;

public sealed class GetTagByNameEndpoint(ITagService tagService) : Endpoint<GetTagByNameRequest>
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService), "Tag Service must not be null");

    public override void Configure()
    {
        Get("/api/tags/name/{Name}");
        Roles("Learner");
    }

    public override async Task HandleAsync(GetTagByNameRequest request, CancellationToken token)
    {
        var tags = await _tagService.GetTagByName(request.Name);
        await Send.OkAsync(tags, cancellation: token);
    }
}
