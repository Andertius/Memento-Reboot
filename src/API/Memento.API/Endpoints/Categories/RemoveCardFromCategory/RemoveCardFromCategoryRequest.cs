namespace Memento.API.Endpoints.Categories.RemoveCardFromCategory;

public sealed class RemoveCardFromCategoryRequest
{
    public int CategoryId { get; init; }
    public int CardId { get; init; }
}
