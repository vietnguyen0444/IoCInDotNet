using BASE.Model;
using BASE.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Service
{
    public class Service<U, V> : IService<U, V> where U : class, IEntity<V>
    {
        protected readonly IUnitOfWork _unitOfWork;

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected int Language_Id { get { return _unitOfWork.Language_Id; } }

        protected IRepository<U, V> GetRepository()
        {
            return _unitOfWork.Repository<U, V>();
        }

        protected IRepository<T, Int32> GetRepository<T>() where T : class, IEntity<Int32>
        {
            return _unitOfWork.Repository<T>();
        }
        protected IRepository<T, K> GetRepository<T, K>() where T : class, IEntity<K>
        {
            return _unitOfWork.Repository<T, K>();
        }

        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        // CRUD

        public virtual U Add(U u)
        {
            if (u.Created_Date == null || u.Created_Date.Date == default(DateTime).Date)
            {
                u.Created_Date = DateTime.Now;
            }

            return GetRepository().Add(u);
        }

        public virtual void AddAll(U[] us)
        {
            GetRepository().AddAll(us);
        }

        public virtual void UpdateAll(U[] us)
        {
            var curDateTime = DateTime.Now;
            foreach (var u in us)
            {
                u.GetType().GetProperty("Modified_Date").SetValue(u, curDateTime);
            }

            GetRepository().UpdateAll(us);
        }

        public virtual U Update(U u)
        {
            u.GetType().GetProperty("Modified_Date").SetValue(u, DateTime.Now);
            return GetRepository().Update(u);
        }

        public virtual void Delete(V id)
        {
            var found = FindById(id);
            if (found != null)
            {
                found?.GetType().GetProperty("Modified_Date").SetValue(DateTime.Now);
                GetRepository().Delete(found);
            }
            else
                throw new NullReferenceException();
        }

        public virtual void DeleteById(V id)
        {
            Delete(id);
        }

        public virtual void Delete(U u)
        {
            Delete(u.ObjectId);
        }

        public virtual U Active(V id)
        {
            var found = FindById(id);
            if (found != null)
            {
                found?.GetType().GetProperty("Modified_Date").SetValue(DateTime.Now);
                GetRepository().Active(found);
            }
            else
                throw new NullReferenceException();

            return found;
        }

        public U Active(U u)
        {
            return GetRepository().Active(u);
        }

        public virtual void ActiveAll(U[] us)
        {
            GetRepository().ActiveAll(us);
        }

        public virtual void ActiveAllWithDuplicate<TKey>(Func<U, TKey> keySelector)
        {
            GetRepository().ActiveAllWithDuplicate(keySelector);
        }

        public void DeleteAll(V[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                GetRepository().DeleteAll(ids);
            }
        }

        public void DeleteAll(U[] us)
        {
            if (us != null && us.Length > 0)
            {
                GetRepository().DeleteAll(us);
            }
        }

        public void DeleteByCriteria(Expression<Func<U, bool>> exp)
        {
            var us = FindAllByCriteria(exp).ToArray();
            DeleteAll(us);
        }

        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        //Find and Check

        private bool CanTranslate()
        {
            return false;
            //return Lang_Id != Convert.ToInt16(LanguageEnum.vi) && ModelBase<Int32>.GetProperty_Translations(typeof(U)) != null;
        }

        public U FindById(V id)
        {
            var target = GetRepository().Find(id);
            if (CanTranslate())
            {
                var localizer = (U)target.Clone();
                localizer.Localize(Language_Id);
                return localizer;
            }
            return target;
        }

        public U FindByCriteria(Expression<Func<U, bool>> exp)//with localize
        {
            //return GetRepository().FindByCriteria(exp);
            var target = GetRepository().FindByCriteria(exp);
            if (CanTranslate())
            {
                var t_target = target.Clone();
                t_target.Localize(Language_Id);
                return (U)t_target;
            }

            return target;
        }

        public virtual string NameForFinding
        {
            get { return string.Empty; }
        }

        public virtual string NameForParent
        {
            get { return string.Empty; }
        }

        public IEnumerable<U> FilterByName(IEnumerable<U> source, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return source;

            name = name.Trim();//.ToLower().Convert_Chuoi_Khong_Dau();
            var list = new List<U>();
            var HasName = false;

            foreach (var item in source)
            {
                var item_name_prop = item.GetType().GetProperty(NameForFinding);
                if (item_name_prop != null)
                {
                    HasName = true;
                    var item_name_value = item_name_prop.GetValue(item).ToString();
                    if (item_name_value.Contains(name))
                    {
                        list.Add(item);
                    }
                    //if (item_name_value.ToLower().Convert_Chuoi_Khong_Dau().Contains(name))
                    //{
                    //    list.Add(item);
                    //}
                }
                else
                {
                    break;
                }
            }

            return HasName ? list : source;
        }

        public IEnumerable<U> FilterByParent(IEnumerable<U> source, int parentId = 0)
        {
            if (parentId == 0)
                return source;

            var list = new List<U>();
            var HasName = false;

            foreach (var item in source)
            {
                var item_name_prop = item.GetType().GetProperty(NameForParent);
                if (item_name_prop != null)
                {
                    HasName = true;
                    var item_name_value = item_name_prop.GetValue(item);
                    if (item_name_value != null && item_name_value.ToString().Equals(parentId.ToString()))
                    {
                        list.Add(item);
                    }
                }
                else
                {
                    break;
                }
            }

            return HasName ? list : source;
        }

        public virtual IEnumerable<U> FindByName(string name, int parentId = 0)
        {
            return FilterByName(FilterByParent(FindAllValid(), parentId), name);
        }

        public virtual bool CheckExistIdentity(string identity)
        {
            return false;
        }

        /// <summary>
        /// Check trùng data (nếu có) khi Add/Edit
        /// Khi check trùng phải loại trừ chính mình (u.Id != current.Id) vì mình chắc chắn tồn tại
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public virtual string CheckExistIdentity(U current)
        {
            return null;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        //FindAll

        public IEnumerable<U> FindAll()
        {
            var result = GetRepository().FindAll();
            //if (CanTranslate())
            //{
            //    foreach (var item in result)
            //    {
            //        item.Localize(Lang_Id);
            //    }
            //}
            return result;
        }

        public IEnumerable<U> FindAllByCriteria(Expression<Func<U, bool>> exp)
        {
            var result = GetRepository().FindAllByCriteria(exp);
            //if (CanTranslate())
            //{
            //    foreach (var item in result)
            //    {
            //        item.Localize(Lang_Id);
            //    }
            //}
            return result;
        }

        public virtual IEnumerable<U> FindAllForFilter()
        {
            return FindAllValid();
        }

        public IQueryable<U> FindAllQuery()
        {
            return GetRepository().FindAllQuery();
        }

        public IQueryable<U> FindAllByCriteriaQuery(Expression<Func<U, bool>> exp)
        {
            var result = GetRepository().FindAllByCriteriaQuery(exp);
            //if (CanTranslate())
            //{
            //    foreach (var item in result)
            //    {
            //        item.Localize(Lang_Id);
            //    }
            //}
            return result;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        //FindAllBy

        public virtual IEnumerable<U> FindAllValid()
        {
            return FindAll().Valid();
        }

        public U FindValidByCriteria(Expression<Func<U, bool>> exp)
        {
            return FindAllValidByCriteria(exp).FirstOrDefault();
        }

        public IEnumerable<U> FindAllValidByCriteria(Expression<Func<U, bool>> exp)
        {
            return FindAllByCriteria(exp).Valid();
        }

        public void SaveChanges()
        {
            GetRepository().SaveChanges();
        }

        #region Compiled Query

        public bool UseCompiled_Query()
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            return _unitOfWork.UseCompiled_Query;
        }

        public static string GetCompiledQueryMethodName(string methodName)
        {
            return $"{methodName}_CompiledQuery";
        }

        public static T InvokeCompiledQuery<T>(object service, string methodName, object[] parameters)
        {
            var methodList = service.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name == methodName);
            foreach (var method in methodList)
            {
                try
                {
                    return (T)method.Invoke(service, parameters);
                }
                catch (Exception e)
                {
                    //
                }
            }

            return default(T);
        }

        #endregion
    }
}
