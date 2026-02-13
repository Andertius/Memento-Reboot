using System.Collections.Generic;

namespace Memento.Infrastructure.Entities;

public class TagEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public virtual ICollection<CardEntity> Cards { get; set; } = [];
    public virtual ICollection<CategoryEntity> Categories { get; set; } = [];
}
