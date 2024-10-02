using System.Linq.Expressions;

namespace App.Core
{
    public interface IPaginationService<T>
    {
        Task<IPageResult<T>> Paginate(IQueryable<T> query,PaginationParams pageParams);
    }
}
