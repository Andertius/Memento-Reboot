using System.Collections.Generic;
using System.Threading;

namespace Memento.Domain.Models;

public sealed class Card
{
    public int Id { get; set; }
	
    public string? Word { get; set; }
	
    public string? Translation { get; set; }
	
    public string? Definition { get; set; }
	
	public string? Hint { get; set; }
	
	public string? Image { get; set; }
	
    public IReadOnlyCollection<Category>  Categories{ get; set; } = [];
}
