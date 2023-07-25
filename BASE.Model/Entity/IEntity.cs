namespace BASE.Model;

public interface IEntity<T> : IEntityBase
{
    new T ObjectId { get; }
}
