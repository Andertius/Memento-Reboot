namespace Memento.API.Endpoints.Categories.GetAllCategories;

public sealed class GetAllCategoriesRequest
{
    public int? Take { get; set; }

    public int? Skip { get; set; }

    public string? Filter { get; set; }
}
