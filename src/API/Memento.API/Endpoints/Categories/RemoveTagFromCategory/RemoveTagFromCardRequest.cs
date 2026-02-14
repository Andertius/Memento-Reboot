namespace Memento.API.Endpoints.Categories.RemoveTagFromCategory;

public sealed class RemoveTagFromCategoryRequest
{
    public int CategoryId { get; set; }
    public int TagId { get; set; }
}
