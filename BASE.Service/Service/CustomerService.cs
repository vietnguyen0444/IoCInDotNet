using BASE.Model;
using BASE.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Service;

public class CustomerService : Service<Customer, Int32>, ICustomerService
{
    public CustomerService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
