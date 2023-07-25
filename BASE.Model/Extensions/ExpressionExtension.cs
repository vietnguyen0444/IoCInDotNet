using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BASE.Model;

public static class ExpressionExtension
{
    public static Expression<Func<T, bool>> True<T>() { return f => true; }
    public static Expression<Func<T, bool>> False<T>() { return f => false; }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>(Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
    }

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>(Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
    }

    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
    {
        var negated = Expression.Not(expression.Body);
        return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
    }

    /// <summary>
    /// pageIndex = 1 [start from page #1]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static IEnumerable<T> Paging<T>(this IEnumerable<T> list, int pageIndex, int pageSize)
    {
        if (pageSize > 0)
            return list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        return list;
    }

    public static IEnumerable<T> Valid<T>(this IEnumerable<T> list) where T : class, IEntityBase
    {
        return list.Where(u => u.IsValid());
    }

    public static IEnumerable<T> GetValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    public static bool IsInt32(this string s)
    {
        int n;
        return int.TryParse(s, out n);
    }

    public static bool IsNumber(this string s)
    {
        if (!string.IsNullOrEmpty(s))
        {
            int n;
            var first = s.First().ToString();
            if (int.TryParse(first, out n))
            {
                if (IsPhoneNumber(s))
                    return true;

                return int.TryParse(s, out n);
            }
        }
        return false;
    }
    public static bool IsValidEmail(this string email)
    {
        if (!string.IsNullOrEmpty(email))
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }
        return false;
    }
    public static bool IsPhoneNumber(string number)
    {
        return Regex.Match(number, @"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})").Success;
    }

    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
    {
        if (condition)
        {
            return source.Where(predicate);
        }
        return source;
    }

    public static T ToEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    /// <summary>
    /// Finds the indices of all objects matching the given predicate.
    /// </summary>
    /// <typeparam name="T">The type of objects in the list.</typeparam>
    /// <param name="list">The list.</param>
    /// <param name="predicate">The predicate.</param>
    /// <returns>Indices of all objects matching the given predicate.</returns>
    public static IEnumerable<int> FindIndices<T>(this IList<T> list, Func<T, bool> predicate)
    {
        return list.Where(predicate).Select(list.IndexOf);
    }
}
