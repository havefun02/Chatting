namespace App.Core
{
    public interface IUserBaseService<T>:IBaseService<T>
    {
        Task<IEnumerable<T>> GetAllByUserAsync(int userId);
        Task<T> GetOneByUserAsync(int id,int userId);

        Task<T> UpdateByUserAsync(int id, int userId, T entity);
        Task DeleteByUserAsync(int id, int userId);
    }
}
