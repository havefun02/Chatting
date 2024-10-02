using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace App.Core
{
    public class SortingService<T> : ISortingService<T> where T : class
    {
        private readonly AppDbContext _todoDbContext;
        public SortingService(AppDbContext todoDbContext) { _todoDbContext = todoDbContext; }
        public IQueryable<T> Sort(IQueryable<T> query, SortingParams? sortingParams)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            if (sortingParams == null|| sortingParams.sorts==null)
            {
                return query;
            }
            if (sortingParams.sorts.Count == 0)
            {
                if (sortingParams.order == "asc")
                    query = SortByPrimaryKey(query, false);
                else if (sortingParams.order == "desc")
                    query = SortByPrimaryKey(query, true);
            }
            else
            for (int i = 0; i < sortingParams?.sorts?.Count; i++)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, sortingParams.sorts[i]);
                var keySelector = Expression.Lambda<Func<T, object>>(
                    Expression.Convert(property, typeof(object)),
                    parameter
                );
                if (sortingParams.order == "asc")
                {
                    query = query.OrderBy(keySelector);
                }
                else
                {
                    query = query.OrderByDescending(keySelector);
                }
            }
            return query;
        }
        public IQueryable<T> SortByPrimaryKey(IQueryable<T> query,bool option)
        {
            var entityType = typeof(T);
            var primaryKey = _todoDbContext.Model?.FindEntityType(entityType)?.FindPrimaryKey()?.Properties.FirstOrDefault();

            if (primaryKey != null)
            {
                var parameter = Expression.Parameter(entityType, "e");
                var property = Expression.Property(parameter, primaryKey.Name);
                var defaultOrderBy = Expression.Lambda<Func<T, object>>(
                    Expression.Convert(property, typeof(object)), parameter);
                if (option==false)
                query = query.OrderBy(defaultOrderBy);
                else
                query = query.OrderByDescending(defaultOrderBy);
                return query;
            }
            else
            {
                throw new InvalidOperationException($"No primary key or ordering specified for {entityType.Name}");
            }
            
        }
    }
}
