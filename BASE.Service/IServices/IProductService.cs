using Azure;
using BASE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Service;

public interface IProductService : IService<Product, Int32>
{
}
