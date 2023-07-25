using BASE.Model;
using BASE.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Service;

public class OrderService : Service<Order, Int32>, IOrderService
{
    public OrderService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
