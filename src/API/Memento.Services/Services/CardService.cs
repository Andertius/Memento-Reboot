using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Memento.Infrastructure.Repositories;
using Memento.Services.Mappers;
using Memento.Services.Models;

namespace Memento.Services.Services;

public interface ICardService
{
    Task<Card[]> GetAllCards(CancellationToken token = default);
    
    Task<Card[]> GetCards(int categoryId = 0, IReadOnlyCollection<int>? tagIds = null, CancellationToken token = default);

    Task<int> AddCard(Card card, CancellationToken token = default);

    Task UpdateCard(Card card, CancellationToken token = default);

    Task RemoveCard(int id, CancellationToken token = default);

    Task<Card?> GetById(int id, CancellationToken token = default);

    Task UpdateCardTags(int cardId, IReadOnlyCollection<int> tagIds, CancellationToken token = default);

    Task UpdateCardCategories(int cardId, IReadOnlyCollection<int> categoryIds, CancellationToken token = default);
}

public sealed class CardService(ICardRepository cardRepository) : ICardService
{
    private readonly ICardRepository _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository), "Card Repository must not be null");
    private readonly CardMapper _cardMapper = new();

    public async Task<Card[]> GetAllCards(CancellationToken token = default)
    {
        var cards = await _cardRepository.GetAllCards(token);

        return cards.Select(_cardMapper.MapCardEntityToCard).ToArray();
    }

    public async Task<Card[]> GetCards(int categoryId = 0, IReadOnlyCollection<int>? tagIds = null, CancellationToken token = default)
    {
        var cards = await _cardRepository.GetCards(categoryId, tagIds, token);

        return cards.Select(_cardMapper.MapCardEntityToCard).ToArray();
    }

    public async Task<int> AddCard(Card card, CancellationToken token = default)
    {
        var cardEntity = _cardMapper.MapCardToCardEntity(card);

        return await _cardRepository.AddCard(cardEntity, token);
    }

    public async Task UpdateCard(Card card, CancellationToken token = default)
    {
        var cardEntity = _cardMapper.MapCardToCardEntity(card);
        await _cardRepository.UpdateCard(cardEntity, token);
    }

    public Task RemoveCard(int id, CancellationToken token = default)
        => _cardRepository.RemoveCard(id, token);

    public async Task<Card?> GetById(int id, CancellationToken token = default)
    {
        var card = await _cardRepository.GetById(id, token);

        return card is null
            ? null
            : _cardMapper.MapCardEntityToCard(card);
    }

    public Task UpdateCardTags(int cardId, IReadOnlyCollection<int> tagIds, CancellationToken token = default)
        => _cardRepository.UpdateCardTags(cardId, tagIds, token);

    public Task UpdateCardCategories(int cardId, IReadOnlyCollection<int> categoryIds, CancellationToken token = default)
        => _cardRepository.UpdateCardCategories(cardId, categoryIds, token);
}
