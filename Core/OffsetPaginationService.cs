using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace App.Core
{
    public class OffsetPaginationService<T>:IPaginationService<T> where T:class
    {
        public OffsetPaginationService()
        {
        }
        public async Task<IPageResult<T>> Paginate(IQueryable<T> query,PaginationParams pageParams)
        {
            var offsetParams = pageParams as OffsetParams;
            if (offsetParams == null) throw new ArgumentException("Invalid pagination parameters.");

            var count = await query.CountAsync();
            var items = await query.Skip(offsetParams.offset)
                                   .Take(offsetParams.limit)
                                   .ToListAsync();
            if (items == null)
            {
                throw new Exception("Cant find items");
            }
            return new OffsetPageResult<T>
            {
                totalItems= count,
                limit= offsetParams.limit,
                offset= offsetParams.offset,
                Items=items,
            };
        }
    }
}
