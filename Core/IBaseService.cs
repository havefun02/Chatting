namespace App.Core
{
    public interface IBaseService<T>
    {
         Task<IEnumerable<T>> GetAll();
         Task<T> GetById(int id);
         Task<T> Create(T entity);
         Task<T> Update(T entity);
         Task Delete(int id);
    }
}
