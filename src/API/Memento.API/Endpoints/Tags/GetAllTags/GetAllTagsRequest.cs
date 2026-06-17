namespace Memento.API.Endpoints.Tags.GetAllTags;

public sealed class GetAllTagsRequest
{
    public int? Take { get; set; }

    public int? Skip { get; set; }

    public string? Filter { get; set; }
}
