namespace Memento.API.Interfaces;

public interface IEntityRequest<out T>
{
    T ToModel();
}
