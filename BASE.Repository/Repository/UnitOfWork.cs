using BASE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Repository;

public class UnitOfWork : IUnitOfWork
{
    public IDataContext Context { get; set; }
    private Dictionary<string, object> Repositories;
    public int Language_Id { get; set; }
    public bool UseCompiled_Query { get; set; }

    public UnitOfWork(IDataContext context, int languageId, bool isLock = false, bool useCompiled_Query = false)
    {
        Context = context;
        Context.IsLock = isLock;
        Language_Id = languageId;
        UseCompiled_Query = useCompiled_Query;
        if (Repositories == null)
        {
            Repositories = new Dictionary<string, object>();
        }
    }

    private object GetInstance<V>(Type t)
    {
        if (t.GetInterfaces().Contains(typeof(IStoredEntityBase)))
        {
            return Activator.CreateInstance(typeof(StoredRepository<,>)
                        .MakeGenericType(t, typeof(V)), Context);
        }
        else
        {
            return Activator.CreateInstance(typeof(Repository<,>)
                    .MakeGenericType(t, typeof(V)), Context);
        }
    }

    public void SetLock(bool isLock)
    {
        Context.IsLock = isLock;
    }

    public IRepository<T, Int32> Repository<T>() where T : class, IEntity<Int32>
    {
        return Repository<T, Int32>();
    }

    public IRepository<U, V> Repository<U, V>() where U : class, IEntity<V>
    {
        var t = typeof(U);
        if (!Repositories.ContainsKey(t.Name))
        {
            Repositories.Add(t.Name, GetInstance<V>(t));
        }
        
        return (IRepository<U, V>)Repositories[t.Name];
    }
}
