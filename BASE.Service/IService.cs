using BASE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Service;

public interface IService<U, V> : IServiceBase where U : class, IEntity<V>
{
    U Add(U u);
    void AddAll(U[] us);
    void UpdateAll(U[] us);
    U Update(U u);
    void Delete(V id);
    void DeleteById(V id);//for anti-ambigous with Delete
    void Delete(U u);
    void DeleteByCriteria(Expression<Func<U, bool>> exp);
    U Active(V id);
    U Active(U u);
    void ActiveAll(U[] us);
    void ActiveAllWithDuplicate<TKey>(Func<U, TKey> keySelector);

    void DeleteAll(V[] ids);
    void DeleteAll(U[] us);

    U FindById(V id);
    U FindByCriteria(Expression<Func<U, bool>> exp);
    U FindValidByCriteria(Expression<Func<U, bool>> exp);

    bool CheckExistIdentity(string identity);//hàm override cho class con kế thừa
    string CheckExistIdentity(U current);//hàm override cho class con kế thừa: empty == true, not empty == error

    IEnumerable<U> FindAll();
    IEnumerable<U> FindAllValid();
    IEnumerable<U> FindAllForFilter();

    IQueryable<U> FindAllQuery();
    IQueryable<U> FindAllByCriteriaQuery(Expression<Func<U, bool>> exp);

    IEnumerable<U> FindByName(string name, int parentId = 0);

    IEnumerable<U> FindAllByCriteria(Expression<Func<U, bool>> exp);
    IEnumerable<U> FindAllValidByCriteria(Expression<Func<U, bool>> exp);

    void SaveChanges();
}
