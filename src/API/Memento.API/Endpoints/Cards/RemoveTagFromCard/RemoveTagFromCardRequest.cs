namespace Memento.API.Endpoints.Cards.RemoveTagFromCard;

public sealed class RemoveTagFromCardRequest
{
    public int CardId { get; set; }
    public int TagId { get; set; }
}
