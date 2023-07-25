using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Model;

public class StoredModelBase<T> : ModelBase<T>, IStoredEntity<T>
{
    public StoredModelBase() : base()
    {
        Modified_Date = DateTime.Now;
    }

    public virtual int Status { get; set; }

    public virtual int? Created_By { get; set; }

    public virtual int? Modified_By { get; set; }

    [Required]
    public virtual DateTime Modified_Date { get; set; }

    public override bool IsValid()
    {
        return this.Status == (int)EntityStatus.Visible;
    }

    public override bool IsAvailable()
    {
        return this.Status == (int)EntityStatus.Visible || this.Status == (int)EntityStatus.UnPublic;
    }

    public override bool IsValidCopyProperties(PropertyInfo prop)
    {
        return base.IsValidCopyProperties(prop) && prop.Name != "Status" && prop.Name != "Created_By";
    }
}
