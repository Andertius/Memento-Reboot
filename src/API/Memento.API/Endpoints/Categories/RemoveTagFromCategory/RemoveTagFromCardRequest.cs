namespace Memento.API.Endpoints.Categories.RemoveTagFromCategory;

public sealed class RemoveTagFromCategoryRequest
{
    public int CategoryId { get; init; }
    public int TagId { get; init; }
}
