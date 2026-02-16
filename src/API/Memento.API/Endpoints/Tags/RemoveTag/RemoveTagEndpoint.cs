using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Tags.RemoveTag;

public sealed class RemoveTagEndpoint(ITagService tagService) : Endpoint<RemoveTagRequest>
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService), "Tag Service must not be null");

    public override void Configure()
    {
        Delete(ApiPrefixes.TagsPrefix + "/{Id}");
        Roles("Learner");
    }

    public override async Task HandleAsync(RemoveTagRequest request, CancellationToken token)
    {
        await _tagService.RemoveTag(request.Id);
        await Send.OkAsync(cancellation: token);
    }
}
