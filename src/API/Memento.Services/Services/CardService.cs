using System;
using System.Linq;
using System.Threading.Tasks;
using Memento.Infrastructure.Repositories;
using Memento.Services.Mappers;
using Memento.Services.Models;

namespace Memento.Services.Services;

public interface ICardService
{
    Task<Card[]> GetAllCards();
    Task<int> AddCard(Card card);
    Task RemoveCard(int id);
    Task<Card?> GetById(int id);
    Task<Card[]> FetchByCategory(int categoryId);
}

public sealed class CardService(ICardRepository cardRepository) : ICardService
{
    private readonly ICardRepository _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository), "Card Repository must not be null");
    private readonly CardMapper _cardMapper = new();

    public async Task<Card[]> GetAllCards()
    {
        var cards = await _cardRepository.GetAllCards();
        return cards.Select(_cardMapper.MapCardEntityToCard).ToArray();
    }

    public async Task<int> AddCard(Card card)
    {
        var cardEntity = _cardMapper.MapCardToCardEntity(card);
        return await _cardRepository.AddCard(cardEntity);
    }

    public async Task<Card?> GetById(int id)
    {
        var card = await _cardRepository.GetById(id);

        return card is null
            ? null
            : _cardMapper.MapCardEntityToCard(card);
    }

    public async Task<Card[]> FetchByCategory(int categoryId)
    {
        var cards = await _cardRepository.FetchByCategoryId(categoryId);
        return cards.Select(_cardMapper.MapCardEntityToCard).ToArray();
    }

    public Task RemoveCard(int id)
        => _cardRepository.RemoveCard(id);
}
