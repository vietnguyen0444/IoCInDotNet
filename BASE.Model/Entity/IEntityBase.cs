using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Model;

public interface IEntityBase
{
    object ObjectId { get; }
    DateTime Created_Date { get; set; }
    bool IsValid();//tất cả đều cần xác định IsValid (== Visible)
    bool IsAvailable();//tất cả đều cần xác định IsAvailable (== Visible|UnPublic)
    bool IsValidCopyProperties(PropertyInfo prop);
    void Copy(IEntityBase u);
    void CopyModel(IEntityBase u);
    IEntityBase ToModel();
    IEntityBase ToModel(IEntityBase u);
    IEntityBase Clone();
    void Localize(int languageId);
}
