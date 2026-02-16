using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Tags.GetTagById;

public sealed class GetTagByIdEndpoint(ITagService tagService) : Endpoint<GetTagByIdRequest>
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService), "Tag Service must not be null");

    public override void Configure()
    {
        Get(ApiPrefixes.TagsPrefix + "/{Id}");
        Roles("Learner");
    }

    public override async Task HandleAsync(GetTagByIdRequest request, CancellationToken token)
    {
        var tags = await _tagService.GetTagById(request.Id);
        await Send.OkAsync(tags, cancellation: token);
    }
}
