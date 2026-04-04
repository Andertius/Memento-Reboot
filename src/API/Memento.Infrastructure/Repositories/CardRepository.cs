using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Memento.Infrastructure.Database;
using Memento.Infrastructure.Entities;
using Memento.Infrastructure.Extenstions;
using Microsoft.EntityFrameworkCore;

namespace Memento.Infrastructure.Repositories;

public interface ICardRepository
{
    Task<CardEntity[]> GetAllCards(CancellationToken token = default);

    Task<CardEntity[]> GetCards(int categoryId = 0, IReadOnlyCollection<int>? tagIds = null, CancellationToken token = default);

    Task<int> AddCard(CardEntity entity, CancellationToken token = default);

    Task UpdateCard(CardEntity entity, CancellationToken token = default);

    Task<(bool Exists, string? ImageName)> GetImageName(int cardId, CancellationToken token = default);

    Task UpsertImage(int cardId, string imageName, CancellationToken token = default);

    Task RemoveImage(int cardId, CancellationToken token = default);

    Task<CardEntity?> GetById(int id, CancellationToken token = default);

    Task RemoveCard(int id, CancellationToken token = default);

    Task UpdateCardTags(int cardId, IReadOnlyCollection<int> tagIds, CancellationToken token = default);

    Task UpdateCardCategories(int cardId, IReadOnlyCollection<int> categoryIds, CancellationToken token = default);
}

public sealed class CardRepository(CardDbContext context) : ICardRepository
{
    private readonly CardDbContext _context = context ?? throw new ArgumentNullException(nameof(context), "Card DbContext must not be null");

    public Task<CardEntity[]> GetAllCards(CancellationToken token = default)
        => _context
            .Cards
            .AsNoTracking()
            .Include(x => x.Categories)
            .Include(x => x.Tags)
            .ToArrayAsync(token);

    public Task<CardEntity[]> GetCards(int categoryId = 0, IReadOnlyCollection<int>? tagIds = null, CancellationToken token = default)
        => _context
            .Cards
            .AsNoTracking()
            .Include(x => x.Tags)
            .ApplyCategoryFilter(categoryId)
            .ApplyTagsFilter(tagIds)
            .ToArrayAsync(token);

    public async Task<int> AddCard(CardEntity entity, CancellationToken token = default)
    {
        entity.Categories = await _context
            .Categories
            .Where(x => entity.Categories.Select(x => x.Id).Contains(x.Id))
            .ToArrayAsync(token);

        entity.Tags = await _context
            .Tags
            .Where(x => entity.Tags.Select(x => x.Id).Contains(x.Id))
            .ToArrayAsync(token);

        _context.Cards.Add(entity);
        await _context.SaveChangesAsync(token);

        return entity.Id;
    }

    public async Task UpdateCard(CardEntity entity, CancellationToken token = default)
    {
        var entry = _context.Cards.Update(entity);
        entry.Property(x => x.Image).IsModified = false;
        await _context.SaveChangesAsync(token);
    }

    public async Task<(bool Exists, string? ImageName)> GetImageName(int cardId, CancellationToken token = default)
    {
        var card = await _context
            .Cards
            .AsNoTracking()
            .Select(x => new { x.Id, x.Image })
            .FirstOrDefaultAsync(x => x.Id == cardId, token);

        return (card is not null, card?.Image);
    }

    public async Task UpsertImage(int cardId, string imageName, CancellationToken token = default)
    {
        var entity = await _context.Cards.FindAsync([cardId], token);

        if (entity is null)
        {
            return;
        }

        entity.Image = imageName;
        await _context.SaveChangesAsync(token);
    }

    public async Task RemoveImage(int cardId, CancellationToken token = default)
    {
        var entity = await _context.Cards.FindAsync([cardId], token);

        if (entity is null)
        {
            return;
        }

        entity.Image = null;
        await _context.SaveChangesAsync(token);
    }

    public Task<CardEntity?> GetById(int id, CancellationToken token = default)
        => _context
            .Cards
            .AsNoTracking()
            .Include(x => x.Categories)
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == id, token);

    public async Task RemoveCard(int id, CancellationToken token = default)
    {
        var card = await _context
            .Cards
            .FindAsync([id], token);

        if (card is null)
        {
            return;
        }

        _context.Cards.Remove(card);
        await _context.SaveChangesAsync(token);
    }

    public async Task UpdateCardTags(int cardId, IReadOnlyCollection<int> tagIds, CancellationToken token = default)
    {
        var cardEntity = await _context
            .Cards
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == cardId, token);

        if (cardEntity is null)
        {
            return;
        }

        cardEntity.Tags = await _context
            .Tags
            .Where(x => tagIds.Contains(x.Id))
            .ToListAsync(token);

        await _context.SaveChangesAsync(token);
    }

    public async Task UpdateCardCategories(int cardId, IReadOnlyCollection<int> categoryIds, CancellationToken token = default)
    {
        var cardEntity = await _context
            .Cards
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == cardId, token);

        if (cardEntity is null)
        {
            return;
        }

        cardEntity.Categories = await _context
            .Categories
            .Where(x => categoryIds.Contains(x.Id))
            .ToListAsync(token);

        await _context.SaveChangesAsync(token);
    }
}
