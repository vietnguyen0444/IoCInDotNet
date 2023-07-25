using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Model;

public partial class Product
{
    public bool IsGoodProduct()
    {
        return true;
    }
}

public partial class Order
{
    public bool IsFirstOrder()
    {
        return true;
    }
}

public partial class OrderDetail
{
    public bool HasMoreThanFiveProduct()
    {
        return true;
    }
}

public partial class Category
{
    public bool IsDeleted()
    {
        return false;
    }
}

public partial class Customer
{
    public bool IsVIPMember()
    {
        return true;
    }
}
