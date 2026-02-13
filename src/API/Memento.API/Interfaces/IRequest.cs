namespace Memento.API.Interfaces;

public interface IRequest<T>
{
    T ToModel();
}
