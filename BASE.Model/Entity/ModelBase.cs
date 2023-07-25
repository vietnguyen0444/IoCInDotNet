using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BASE.Model;

public abstract class ModelBase<T> : IEntity<T>
{
    public ModelBase()
    {
        Created_Date = DateTime.Now;
    }

    public virtual T ObjectId { get { throw new NotImplementedException(); } }

    object IEntityBase.ObjectId { get { return ObjectId; } }

    public virtual DateTime Created_Date { get; set; }

    public virtual bool IsValid()
    {
        return true;
    }

    public virtual bool IsAvailable()
    {
        return true;
    }

    public virtual bool IsValidCopyProperties(PropertyInfo prop)
    {
        return prop.Name != "Id" && prop.Name != "Obj_Id" && prop.Name != "Created_Date";
    }

    public virtual void Copy(IEntityBase u)
    {
        var this_namespace = typeof(IEntityBase).Namespace;
        foreach (var prop in this.GetType().GetProperties())
        {
            if (prop.PropertyType.Namespace != this_namespace
                && prop.CanWrite
                && !prop.PropertyType.IsArray//array
                &&
                    (!prop.PropertyType.IsGenericType ||
                    prop.PropertyType.IsGenericType
                    && prop.PropertyType.GetGenericTypeDefinition() != typeof(ICollection<>)//EF
                    && prop.PropertyType.GetGenericTypeDefinition() != typeof(IEnumerable<>))//EF
                && IsValidCopyProperties(prop)
                && u.IsValidCopyProperties(prop)
                && u.GetType().GetProperty(prop.Name) != null)
            {
                prop.SetValue(this, u.GetType().GetProperty(prop.Name).GetValue(u));
            }
        }
    }

    /// <summary>
    /// Copy các data properties từ entity u vào this (ngoại trừ non-data properties như Id, Status, Created_Date)
    /// </summary>
    /// <param name="u"></param>
    public void CopyModel(IEntityBase u)
    {
        Copy(u);
    }

    public static System.Reflection.PropertyInfo GetProperty_Translations(Type t)
    {
        return t.GetProperty(t.Name + "_Translations");
    }

    public void Localize(int langId)
    {
        IEnumerable<System.Object> castObj;
        var thisT = this.GetType();
        castObj = (IEnumerable<System.Object>)GetProperty_Translations(thisT).GetValue(this);
        foreach (var sub in castObj)
        {
            var subT = sub.GetType();
            int subLangId = Convert.ToInt32(subT.GetProperty("Lang_Id").GetValue(sub));
            if (subLangId == langId)
            {
                foreach (var subProp in subT.GetProperties())
                {
                    foreach (var mainProp in thisT.GetProperties())
                    {
                        if ("T" + mainProp.Name == subProp.Name)
                        {
                            mainProp.SetValue(this, subProp.GetValue(sub));
                        }
                    }
                }
            }
        }
    }
    public IEntityBase Clone()
    {
        return this.MemberwiseClone() as IEntity<T>;
    }

    public virtual IEntityBase ToModel()
    {
        var type = this.GetType();
        var modelName = type.Name;
        if (modelName.Contains("Model"))
        {
            modelName = modelName.Replace("Model", "");
            var originalType = this.GetType().BaseType.Assembly.GetExportedTypes().Where(t => t.Name == modelName).FirstOrDefault();
            if (originalType != null)
            {
                dynamic obj = Activator.CreateInstance(originalType);
                obj.Copy(this);
                return obj;
            }
        }
        return this;
    }

    public virtual IEntityBase ToModel(IEntityBase _model)
    {
        _model.CopyModel(this); //Can use AutoMapper instead
        return _model;
    }
}
