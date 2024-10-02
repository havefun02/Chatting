namespace App.Core
{
    public interface ISortingService<T>
    {
        IQueryable<T> Sort(IQueryable<T> query, SortingParams? orderParams);

    }
}
