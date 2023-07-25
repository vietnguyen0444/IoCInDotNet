using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Model;

public partial class Product : StoredModelBase<Int32>
{
    public override int ObjectId { get { return this.Id; } }
}

public partial class Order : StoredModelBase<Int32>
{
    public override int ObjectId { get { return this.Id; } }
}

public partial class OrderDetail : StoredModelBase<Int32>
{
    public override int ObjectId { get { return this.Id; } }
}

public partial class Category : StoredModelBase<Int32>
{
    public override int ObjectId { get { return this.Id; } }
}

public partial class Customer : StoredModelBase<Int32>
{
    public override int ObjectId { get { return this.Id; } }
}
