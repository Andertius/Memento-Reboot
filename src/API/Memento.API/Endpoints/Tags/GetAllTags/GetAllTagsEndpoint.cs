using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Tags.GetAllTags;

public sealed class GetAllTagsEndpoint(ITagService tagService) : Endpoint<GetAllTagsRequest>
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService), "Tag Service must not be null");

    public override void Configure()
    {
        Get(ApiPrefixes.TagsApiPrefix);
        Roles("Learner");
    }

    public override async Task HandleAsync(GetAllTagsRequest request, CancellationToken token)
    {
        var tags = await _tagService.GetAllTags(request.Filter, request.Take, request.Skip, token);
        await Send.OkAsync(tags, cancellation: token);
    }
}
