using System.Collections.Generic;

namespace Memento.Infrastructure.Entities;

public class CategoryEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }

    public virtual ICollection<CardEntity> Cards { get; set; } = [];
    public virtual ICollection<TagEntity> Tags { get; set; } = [];
}
