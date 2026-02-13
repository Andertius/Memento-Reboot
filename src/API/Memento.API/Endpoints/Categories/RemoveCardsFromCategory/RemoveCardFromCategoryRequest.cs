namespace Memento.API.Endpoints.Categories.RemoveCardFromCategory;

public sealed class RemoveCardFromCategoryRequest
{
    public int CategoryId { get; set; }
    public int CardId { get; set; }
}
