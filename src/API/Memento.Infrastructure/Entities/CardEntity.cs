using System.Collections.Generic;

namespace Memento.Infrastructure.Entities;

public class CardEntity
{
    public int Id { get; set; }
    public string? Word { get; set; }
    public string? Translation { get; set; }
    public string? Definition { get; set; }
    public string? Hint { get; set; }
    public string? Image { get; set; }

    public virtual ICollection<CategoryEntity> Categories { get; set; } = [];
    public virtual ICollection<TagEntity> Tags { get; set; } = [];
}
