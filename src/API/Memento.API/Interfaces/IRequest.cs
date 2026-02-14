namespace Memento.API.Interfaces;

public interface IRequest<out T>
{
    T ToModel();
}
