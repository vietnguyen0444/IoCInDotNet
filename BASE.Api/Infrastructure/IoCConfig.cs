using BASE.Model;
using BASE.Repository;
using BASE.Service;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace BASE.Api.Infrastructure;


public class LanguageId
{
    public LanguageId()
    {
    }
    public int Language_Id
    {
        get
        {
            //if (HttpContext.Current.Session == null)
            //{
            //    return (Int32)LanguageEnum.vi;
            //}
            //else
            //{
            //    var result = HttpContext.Current.Session["Lang_Id"] ?? (Int32)LanguageEnum.vi;
            //    return Convert.ToInt32(result);
            //}
            return (Int32)LanguageEnum.vi;
        }
    }
}

public class IoCConfig
{
    private static Container _container;

    /// <summary>
    /// Demo update langid cho api controller lấy từ request header
    /// Dùng cho đa ngôn ngữ trên App
    /// AuthenticateByToken -> IoCConfig.SetLangId(2);
    /// </summary>
    /// <param name="langid"></param>
    public static void SetLangId(int language_id)
    {
        _container.GetInstance<IUnitOfWork>().Language_Id = language_id;
    }

    public static void Register()
    {
        // 1. Create a new SimpleInjector container
        var container = new Container();

        container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

        var optionsBuilder = new DbContextOptionsBuilder<MyDataContext>();
        optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(AppSettings.Configuration["ConnectionString"]);

        // 2. Configure the container (register)
        container.Register<MyDataContext>(
                () => new MyDataContext(optionsBuilder.Options), Lifestyle.Singleton);

        container.Register<LanguageId>(
                () => new LanguageId(), Lifestyle.Scoped);

        container.Register<IUnitOfWork>(
                () => new UnitOfWork(container.GetInstance<MyDataContext>(), container.GetInstance<LanguageId>().Language_Id, false, false), Lifestyle.Scoped);

        var assembly = typeof(IServiceBase).Assembly;


        var registrations =
               from type in assembly.GetExportedTypes()
               where type.GetInterfaces().Contains(typeof(IServiceBase))
                    && type.IsClass && !type.IsAbstract
               select new { 
                    Service = type.GetInterfaces().Where(i => i.Name.Equals("I" + type.Name)).First(),
                    Implementation = type,
               };

        foreach (var reg in registrations)
        {
            if (!reg.Implementation.Name.StartsWith("Service"))
            {
                container.Register(reg.Service
                    , () => Activator.CreateInstance(reg.Implementation, container.GetInstance<IUnitOfWork>())
                    , Lifestyle.Singleton);
            }
        }

        // 3. Optionally verify the container's configuration.
        container.Verify();

        // 4. Store the container for use by Page classes.
        _container = container;
    }

    public static TService GetInstance<TService>() where TService : class
    {
        return _container.GetInstance<TService>();
    }

    public static IService<U, V> ServiceNonregistered<U, V>() where U : class, IEntity<V>
    {
        return new Service<U, V>(GetInstance<IUnitOfWork>());
    }

    //public static object ServiceNonregisteredObject(string modelName)
    //{
    //    var t = GetTypeModelName(modelName);
    //    //var service = new Service<U, V>(GetInstance<XT.Repository.IUow>());
    //    return Activator.CreateInstance(
    //                        typeof(Service<,>).MakeGenericType(t, typeof(int)), GetInstance<IUnitOfWork>());
    //}

    public static TService Service<TService>() where TService : class
    {
        return GetInstance<TService>();
    }

    public static object ServiceObject(string modelName)
    {
        var service = GetServiceModelName(modelName);
        if (service != null)
        {
            return _container.GetInstance(service);
        }

        return null;
    }

    public static object ServiceObject<T>() where T : class, IEntity<Int32>
    {
        var type = typeof(T);

        return ServiceObject(type.Name);
    }

    public static IService<T, Int32> ServiceBase<T>() where T : class, IEntity<Int32>
    {
        var type = typeof(T);
        return ServiceObject(type.Name) as IService<T, Int32>;
    }

    public static dynamic ServiceDynamic(string modelName)
    {
        var service = GetServiceModelName(modelName);
        if (service != null)
        {
            return _container.GetInstance(service);
        }

        return null;
    }

    #region Invoke
    //public static Type GetTypeModelName(string modelName)
    //{
    //    var modelAssemply = typeof(Account).Assembly;
    //    var assemblyQualifiedName = modelAssemply.GetName().Name + "." + modelName;
    //    return modelAssemply.GetExportedTypes().Where(t => t.Name == modelName).FirstOrDefault();
    //}

    public static Type GetServiceModelName(string modelName)
    {
        var assembly = typeof(IServiceBase).Assembly;
        var serviceName = modelName + "Service";
        var service = assembly.GetExportedTypes().Where(a => a.IsClass && !a.IsAbstract && a.Name.Equals(serviceName)).FirstOrDefault();
        return service;
    }

    public static Type GetIServiceBaseModelName(string modelName)
    {
        var assembly = typeof(IServiceBase).Assembly;
        var serviceName = "I" + modelName + "Service";
        var service = assembly.GetExportedTypes().Where(a => a.Name.Equals(serviceName)).FirstOrDefault();
        return service;
    }

    public static object InvokeService(object service, string method_name, object[] parameters)
    {
        var method = service.GetType().GetMethod(method_name);
        var item = method.Invoke(service, parameters);

        return item;
    }

    public static object Invoke(string entity, string method_name, object[] parameters)
    {
        var service = ServiceObject(entity);
        return InvokeService(service, method_name, parameters);
    }

    public static object FindById(string entity, int id)
    {
        return Invoke(entity, "FindById", new object[] { id });
    }

    public static object FindByName(string entity, string name)
    {
        return Invoke(entity, "FindByName", new object[] { name, 0 });
    }

    public static object Update(string entity, object item)
    {
        return Invoke(entity, "Update", new object[] { item });
    }

    //public static object NewObject(string entity)
    //{
    //    var objType = typeof(Account).Assembly.GetExportedTypes().Where(t => t.Name == entity).FirstOrDefault();
    //    if (objType != null)
    //    {
    //        dynamic obj = Activator.CreateInstance(objType);

    //        return obj;
    //    }

    //    return null;
    //}
    #endregion Invoke
}


