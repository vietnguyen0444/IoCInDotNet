using BASE.Model;
using BASE.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Service;

public class OrderDetailService : Service<OrderDetail, Int32>, IOrderDetailService
{
    public OrderDetailService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
