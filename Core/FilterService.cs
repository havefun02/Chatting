
using System.Linq.Expressions;

namespace App.Core
{
    public class FilterService<T> : IFilterService<T> where T : class
    {
        public  IQueryable<T> Filter(IQueryable<T> query, FilterParams? filterParams)
        {
            if (query == null) throw new Exception("DbSet is undefined");
            if (filterParams == null ||filterParams.filterList==null) return query;
            foreach (var filter in filterParams.filterList)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, filter.PropertyName);
                if (filter is RangeFilterCriterion rangeFilter)
                {
                    Expression? body=null;

                    var startValue = rangeFilter.StartValue != null ? Expression.Constant(rangeFilter.StartValue) : null;
                    var endValue = rangeFilter.EndValue != null ? Expression.Constant(rangeFilter.EndValue) : null;
                    if (startValue != null)
                    {
                        var greaterThanOrEqual = Expression.GreaterThanOrEqual(property, startValue);
                        body = greaterThanOrEqual;
                    }

                    if (endValue != null)
                    {
                        var lessThanOrEqual = Expression.LessThanOrEqual(property, endValue);
                        body = body != null ? Expression.AndAlso(body, lessThanOrEqual) : lessThanOrEqual;
                    }

                    if (body != null)
                    {
                        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
                        query = query.Where(lambda);
                    }
                }
                else
                {
                    var value = Expression.Constant(filter.Value);
                    if (filter.Value == null) throw new ArgumentNullException($"{filter.PropertyName} value cannot be null");

                    var body = Expression.Equal(property, value);
                    var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
                    query = query.Where(lambda);
                }
            }

            return query;
        }
    }
}
