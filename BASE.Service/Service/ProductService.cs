using BASE.Model;
using BASE.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Service;

public class ProductService : Service<Product, Int32>, IProductService
{
    public ProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
