using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PhyGen.Application.Abstraction.Query;
using PhyGen.Domain.Common;

namespace PhyGen.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> ApplyPagination<TEntity>(this IQueryable<TEntity> items, int page, int size)
    {
        return items.Skip((page - 1) * size).Take(size);
    }


    public static async Task<Page<TEntityDto>> PaginateWithOrderAsync<TEntity, TEntityDto>(
        this IQueryable<TEntity> items,
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, object>> keySelector,
        SortOrder? sortOrder,
        Func<TEntity, TEntityDto> entityMapper)
    {
        int count = await items.CountAsync();

        items = sortOrder == SortOrder.Descending
            ? items.OrderByDescending(keySelector)
            : items.OrderBy(keySelector);

        List<TEntity> list = await items
            .ApplyPagination(pageNumber, pageSize)
            .ToListAsync();

        List<TEntityDto> result = list
            .Select(entityMapper)
            .ToList();

        return new Page<TEntityDto>(result, count, pageNumber, pageSize);
    }
}