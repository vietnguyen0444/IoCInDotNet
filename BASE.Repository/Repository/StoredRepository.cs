using BASE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Repository;

public class StoredRepository<U, V> : Repository<U, V>, IStoredRepository<U, V> where U : class, IStoredEntity<V>
{
    public StoredRepository(IDataContext context) : base(context)
    {
    }

    private U SetEntityStatus(U u)
    {
        if (u.Status == (int)EntityStatus.UnDefined)
        {
            u.Status = (int)EntityStatus.Visible;
        }

        return u;
    }

    public override U Add(U u)
    {
        u = SetEntityStatus(u);
        return base.Add(u);
    }

    public override void AddAll(U[] us)
    {
        for (int i = 0; i < us.Length; i++)
        {
            us[i] = SetEntityStatus(us[i]);
        }
        base.AddAll(us);
    }

    public override void Delete(U u)
    {
        u.Status = (int)EntityStatus.Invisible;
        //base.Update(u);
        UpdateStatus(u);
    }

    public override U Active(U u)
    {
        u.Status = (int)EntityStatus.Visible;
        return UpdateStatus(u);
        //return base.Update(u);
    }

    public virtual U UpdateStatus(U u)
    {
        U saving = Find(u.ObjectId);
        if (saving != null)
            saving.Status = u.Status;
        SaveChanges();
        return saving;
    }

    public override void DeleteAll(U[] us)
    {
        foreach (var u in us)
        {
            U saving = Find(u.ObjectId);
            saving.Status = (int)EntityStatus.Invisible;
        }
        SaveChanges();
    }

    public override void DeleteAll(V[] ids)
    {
        foreach (var id in ids)
        {
            U saving = Find(id);
            saving.Status = (int)EntityStatus.Invisible;
        }
        SaveChanges();
    }

    public override void ActiveAll(U[] us)
    {
        foreach (var u in us)
        {
            U saving = Find(u.ObjectId);
            saving.Status = (int)EntityStatus.Visible;
        }
        SaveChanges();
    }

    public override void ActiveAllWithDuplicate<TKey>(Func<U, TKey> keySelector)
    {
        var us = FindAll();
        us = us.DistinctBy(keySelector);
        foreach (var u in us)
        {
            u.Status = (int)EntityStatus.Visible;
        }
        SaveChanges();
    }
}
