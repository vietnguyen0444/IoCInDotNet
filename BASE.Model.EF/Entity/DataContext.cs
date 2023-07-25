

using System.Linq.Expressions;
using System.Security.Principal;

namespace BASE.Model;

public partial class MyDataContext : IDataContext
{
    protected IEntityBase RefreshObject;
    public bool IsLock { get; set; }

    public U Add<U>(U u) where U : class, IEntityBase
    {
        u = this.Set<U>().Add(u).Entity;
        RefreshObject = u;
        return u;
    }

    public void Delete<U>(U u) where U : class
    {
        this.Set<U>().Remove(u);
    }

    public async Task<U> Find<U, V>(V id) where U : class, IEntity<V>
    {
        return this.Set<U>().Find(id);
    }

    public IQueryable<U> FindAll<U>() where U : class
    {
        return this.Set<U>().AsQueryable<U>();
    }

    public IQueryable<U> FindAllByCriteria<U>(Expression<Func<U, bool>> exp) where U : class
    {
        return this.Set<U>().Where(exp);
    }

    public void SaveChanges<U>() where U : class, IEntityBase
    {
        if (!IsLock)
        {
            base.SaveChanges();
            if (RefreshObject != null)
            {
                RefreshObject = this.Set<U>().Find(RefreshObject.ObjectId);
            }
        }
    }
}
