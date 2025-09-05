using System.Linq.Expressions;
using System.Reflection;
using Google.Apis.Storage.v1.Data;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Common.Pagination
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PaginationParams paginationParams)
        {

            // if (!string.IsNullOrWhiteSpace(paginationParams.Search))
            // {
            //     var stringProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            //     .Where(p => p.PropertyType == typeof(string))
            //     .ToArray();

            //     if (stringProperties.Length > 0)
            //     {
            //         ParameterExpression param = Expression.Parameter(typeof(T), "x");
            //         Expression? combined = null;

            //         foreach (var prop in stringProperties)
            //         {
            //             var propExpr = Expression.Property(param, prop);
            //             var searchExpr = Expression.Constant(paginationParams.Search);
            //             var containsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;
            //             var containsCall = Expression.Call(propExpr, containsMethod, searchExpr);

            //             combined = combined == null ? containsCall : Expression.OrElse(combined, containsCall);
            //         }
            //         var lambda = Expression.Lambda<Func<T, bool>>(combined!, param);
            //         query = query.Where(lambda);
            //     }
            // }


            var totalItems = await query.CountAsync();
            var items = await query.Skip((paginationParams.Page - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize).ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                Page = paginationParams.Page,
                PageSize = paginationParams.PageSize
            };
        }
    }
}