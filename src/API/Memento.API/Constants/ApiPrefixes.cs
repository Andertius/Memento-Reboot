namespace Memento.API.Constants;

public static class ApiPrefixes
{
    public const string ApiPrefix = "/api";
        
    public const string ImagesPrefix = "/images";

    public const string CardsApiPrefix = $"{ApiPrefix}/cards";

    public const string CategoriesApiPrefix = $"{ApiPrefix}/categories";

    public const string TagsApiPrefix = $"{ApiPrefix}/tags";

    public const string CardsImagesPrefix = $"{ImagesPrefix}/cards";

    public const string CategoriesImagesPrefix = $"{ImagesPrefix}/categories";
}
