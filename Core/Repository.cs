using Microsoft.EntityFrameworkCore;
namespace App.Core
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public Repository(AppDbContext context) { _context = context; }

        public async Task<List<T>> CreateRange(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            await _context.Set<T>().AddRangeAsync(entities);
            
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task<T> Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var res = await _context.Set<T>().AddAsync(entity);
            if (res == null)
            {
                throw new Exception("Not found");
            }
            await _context.SaveChangesAsync();  
            return entity;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity != null)
                {
                    _context.Set<T>().Remove(entity);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Cannot find entity");
                }
            }
            catch 
            {
                throw;
            }
        }

        public async Task<List<T>> FindAll()
        {
            var res = await _context.Set<T>().ToListAsync();
            if (res == null)
            {
                throw new Exception("Cannot find entities");
            }
            return res;
        }

        public async Task<T> FindOneById(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                throw new Exception($"Entity {id} does not exist");
            }
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            if (entity == null) throw new ArgumentException("Invalid entity type", nameof(entity));

            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public  DbSet<T> GetDbSet()
        {
            return _context.Set<T>();
        }
       
    }
}
