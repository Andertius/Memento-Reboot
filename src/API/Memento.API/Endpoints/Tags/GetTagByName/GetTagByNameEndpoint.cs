using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Tags.GetTagByName;

public sealed class GetTagByNameEndpoint(ITagService tagService) : Endpoint<GetTagByNameRequest>
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService), "Tag Service must not be null");

    public override void Configure()
    {
        Get(ApiPrefixes.TagsPrefix + "/name/{Name}");
        Roles("Learner");
    }

    public override async Task HandleAsync(GetTagByNameRequest request, CancellationToken token)
    {
        var tags = await _tagService.GetTagByName(request.Name, token);
        await Send.OkAsync(tags, cancellation: token);
    }
}
