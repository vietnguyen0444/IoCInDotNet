using Microsoft.EntityFrameworkCore;

namespace BASE.Model;

public partial class MyDataContext : DbContext
{
    public MyDataContext()
    {
    }

    public MyDataContext(DbContextOptions<MyDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<Category> Categories{ get; set; }
    public virtual DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
