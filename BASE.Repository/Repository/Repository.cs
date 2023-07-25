using BASE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Repository
{
    //U là 1 class và là con của IEntity<V>
    //V là kiểu dữ liệu khóa
    public class Repository<U, V> : IRepository<U, V> where U : class, IEntity<V>
    {
        protected IDataContext Context { get; set; }

        public Repository(IDataContext context)
        {
            Context = context;
        }

        public virtual U Active(U u)
        {
            throw new NotImplementedException();
        }

        public virtual void ActiveAll(U[] us)
        {
            throw new NotImplementedException();
        }

        public virtual void ActiveAllWithDuplicate<TKey>(Func<U, TKey> keySelector)
        {
            throw new NotImplementedException();
        }

        public virtual U Add(U u)
        {
            throw new NotImplementedException();
        }

        public virtual void AddAll(U[] us)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(V id)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(U u)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteAll(V[] ids)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteAll(U[] us)
        {
            throw new NotImplementedException();
        }

        public virtual U Find(V id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<U> FindAll()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<U> FindAllByCriteria(Expression<Func<U, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<U> FindAllByCriteriaQuery(Expression<Func<U, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<U> FindAllQuery()
        {
            throw new NotImplementedException();
        }

        public virtual U FindByCriteria(Expression<Func<U, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public virtual void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public virtual U Update(U u)
        {
            throw new NotImplementedException();
        }

        public virtual U Update(V id, Func<U, bool> pred)
        {
            throw new NotImplementedException();
        }

        public virtual U Update(V id, Func<IDataContext, U, bool> pred)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateAll(U[] us)
        {
            throw new NotImplementedException();
        }
    }
}
