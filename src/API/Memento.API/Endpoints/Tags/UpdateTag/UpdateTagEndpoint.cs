using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.API.Constants;
using Memento.Services.Services;

namespace Memento.API.Endpoints.Tags.UpdateTag;

public sealed class UpdateTagEndpoint(ITagService tagService) : Endpoint<UpdateTagRequest>
{
    private readonly ITagService _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService), "Tag Service must not be null");

    public override void Configure()
    {
        Put(ApiPrefixes.TagsApiPrefix);
        Roles("Learner");
    }

    public override async Task HandleAsync(UpdateTagRequest request, CancellationToken token)
    {
        bool result = await _tagService.UpdateTag(request.ToModel(), token);

        if (result)
        {
            await Send.OkAsync(cancellation: token);

            return;
        }

        AddError($"Tag '{request.Name}' already exists");
        await Send.ErrorsAsync(cancellation: token);
    }
}
