using System.Linq.Expressions;

namespace BASE.Model;

public interface IDataContext
{
    U Add<U>(U u) where U : class, IEntityBase;
    void Delete<U>(U u) where U : class;
    Task<U> Find<U, V>(V id) where U : class, IEntity<V>;
    IQueryable<U> FindAll<U>() where U : class;
    IQueryable<U> FindAllByCriteria<U>(Expression<Func<U, bool>> exp) where U : class;
    bool IsLock { get; set; }

    void SaveChanges<U>() where U : class, IEntityBase;
}
