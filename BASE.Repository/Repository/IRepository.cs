using BASE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Repository;

public interface IRepository<U, V> : IRepositoryBase where U : class, IEntity<V>
{
    U Add(U u);
    void AddAll(U[] us);
    void UpdateAll(U[] us);
    U Update(U u);
    U Update(V id, Func<U, bool> pred);
    U Update(V id, Func<IDataContext, U, bool> pred);
    void Delete(V id);
    void Delete(U u);
    void DeleteAll(V[] ids);
    void DeleteAll(U[] us);
    U Active(U u);
    void ActiveAll(U[] us);
    void ActiveAllWithDuplicate<TKey>(Func<U, TKey> keySelector);

    U Find(V id);
    U FindByCriteria(Expression<Func<U, bool>> exp);

    IEnumerable<U> FindAll();
    IEnumerable<U> FindAllByCriteria(Expression<Func<U, bool>> exp);

    IQueryable<U> FindAllQuery();
    IQueryable<U> FindAllByCriteriaQuery(Expression<Func<U, bool>> exp);

    void SaveChanges();
}
