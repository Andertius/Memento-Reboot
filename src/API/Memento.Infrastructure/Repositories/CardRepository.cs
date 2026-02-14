using System;
using System.Linq;
using System.Threading.Tasks;
using Memento.Infrastructure.Database;
using Memento.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Memento.Infrastructure.Repositories;

public interface ICardRepository
{
    Task<CardEntity[]> GetAllCards();
    Task<int> AddCard(CardEntity entity);
    Task<CardEntity?> GetById(int id);
    Task RemoveCard(int id);
    Task<CardEntity[]> FetchByCategoryId(int categoryId);
}

public sealed class CardRepository(CardDbContext context) : ICardRepository
{
    private readonly CardDbContext _context = context ?? throw new ArgumentNullException(nameof(context), "Card DbContext must not be null");

    public Task<CardEntity[]> GetAllCards()
        => _context
            .Cards
            .AsNoTracking()
            .Include(x => x.Categories)
            .Include(x => x.Tags)
            .ToArrayAsync();

    public async Task<int> AddCard(CardEntity entity)
    {
        entity.Categories = await _context
            .Categories
            .Where(x => entity.Categories.Select(x => x.Id).Contains(x.Id))
            .ToArrayAsync();

        entity.Tags = await _context
            .Tags
            .Where(x => entity.Tags.Select(x => x.Id).Contains(x.Id))
            .ToArrayAsync();

        _context.Cards.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public Task<CardEntity?> GetById(int id)
        => _context
            .Cards
            .AsNoTracking()
            .Include(x => x.Categories)
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task RemoveCard(int id)
    {
        var card = await _context
            .Cards
            .FindAsync(id);

        if (card is null)
        {
            return;
        }

        _context.Cards.Remove(card);
        await _context.SaveChangesAsync();
    }

    public Task<CardEntity[]> FetchByCategoryId(int categoryId)
        => _context
            .Cards
            .AsNoTracking()
            .Include(x => x.Categories)
            .Include(x => x.Tags)
            .Where(x => x.Categories.Select(y => y.Id).Contains(categoryId))
            .ToArrayAsync();
}
