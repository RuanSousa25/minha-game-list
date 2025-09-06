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