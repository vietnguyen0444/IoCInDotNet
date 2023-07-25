using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Model;

public partial class Order
{
    public int Id { get; set; }
    public string No { get; set; }
    public DateTime Date { get; set; }
    public double Total { get; set; }
    public DateTime ShippingDate { get; set; }
    public int CustomerId { get; set; }
    public bool IsDeliverd { get; set; }
}
