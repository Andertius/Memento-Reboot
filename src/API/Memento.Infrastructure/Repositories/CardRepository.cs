using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Memento.Infrastructure.Database;
using Memento.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Memento.Infrastructure.Repositories;

public interface ICardRepository
{
    Task<CardEntity[]> GetAllCards(CancellationToken token = default);
    Task<int> AddCard(CardEntity entity, CancellationToken token = default);
    Task UpdateCard(CardEntity entity, CancellationToken token = default);
    Task<CardEntity?> GetById(int id, CancellationToken token = default);
    Task RemoveCard(int id, CancellationToken token = default);
    Task<CardEntity[]> FetchByCategoryId(int categoryId, CancellationToken token = default);
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
        _context.Cards.Update(entity);
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

    public Task<CardEntity[]> FetchByCategoryId(int categoryId, CancellationToken token = default)
        => _context
            .Cards
            .AsNoTracking()
            .Include(x => x.Categories)
            .Include(x => x.Tags)
            .Where(x => x.Categories.Select(y => y.Id).Contains(categoryId))
            .ToArrayAsync(token);
}
