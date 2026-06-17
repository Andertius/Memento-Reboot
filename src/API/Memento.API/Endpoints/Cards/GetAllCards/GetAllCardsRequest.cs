namespace Memento.API.Endpoints.Cards.GetAllCards;

public sealed class GetAllCardsRequest
{
    public int? Take { get; set; }

    public int? Skip { get; set; }

    public string? Filter { get; set; }
}
