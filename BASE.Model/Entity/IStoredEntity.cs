using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Model;

public interface IStoredEntity<T> : IStoredEntityBase, IEntity<T>
{
}
