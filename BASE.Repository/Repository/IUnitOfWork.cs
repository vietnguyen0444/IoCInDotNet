using BASE.Model;

namespace BASE.Repository
{
    public interface IUnitOfWork
    {
        IDataContext Context { get; set; }
        IRepository<T, Int32> Repository<T>() where T : class, IEntity<Int32>;
        IRepository<U, V> Repository<U, V>() where U : class, IEntity<V>;
        int Language_Id { get; set; }
        bool UseCompiled_Query { get; set; }

        void SetLock(bool isLock);
    }
}
