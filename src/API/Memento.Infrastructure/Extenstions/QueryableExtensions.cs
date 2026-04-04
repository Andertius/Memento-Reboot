using System.Collections.Generic;
using System.Linq;
using Memento.Infrastructure.Entities;

namespace Memento.Infrastructure.Extenstions;

public static class QueryableExtensions
{
    extension(IQueryable<CardEntity> query)
    {
        public IQueryable<CardEntity> ApplyCategoryFilter(int categoryId)
            => categoryId == 0
                ? query
                : query.Where(x => x.Categories.Select(x => x.Id).Contains(categoryId));

        public IQueryable<CardEntity> ApplyTagsFilter(IReadOnlyCollection<int>? tagIds)
             => tagIds is { Count: > 0 }
                ? query.Where(x => x.Tags.Select(x => x.Id).Intersect(tagIds).Any())
                : query;
    }
}
