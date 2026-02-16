using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Tags.AddTag;

public sealed class AddTagEndpoint(ITagService tagService) : Endpoint<AddTagEntityRequest>
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService), "Tag Service must not be null");

    public override void Configure()
    {
        Post(ApiPrefixes.TagsPrefix);
        Roles("Learner");
    }

    public override async Task HandleAsync(AddTagEntityRequest entityRequest, CancellationToken token)
    {
        int id = await _tagService.AddTag(entityRequest.ToModel());

        if (id == 0)
        {
            AddError("Could not add tag as it already exists");
            await Send.ErrorsAsync(cancellation: token);

            return;
        }

        await Send.CreatedAtAsync($"{ApiPrefixes.TagsPrefix}/{id}", cancellation: token);
    }
}
