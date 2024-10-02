namespace App.Core
{
    public interface IFilterService<T>
    {
        IQueryable<T>  Filter(IQueryable<T> query, FilterParams? filterParams);
    }
}
