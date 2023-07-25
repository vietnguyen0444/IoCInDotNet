using BASE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Repository;

public interface IStoredRepository<U, V> : IRepository<U, V> where U : class, IStoredEntity<V>
{
    U UpdateStatus(U u);
}
