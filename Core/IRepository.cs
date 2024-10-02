using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace App.Core
{
    public interface IRepository<T> where T : class
    {
        public Task<T> Create(T entity);
        public Task<List<T>> CreateRange(List<T> entity);
        public Task<T> Update(T entity);
        public Task Delete(int id);
        public Task<T> FindOneById(int id);
        public Task<List<T>> FindAll();
        public DbSet<T> GetDbSet();
        public Task SaveChangesAsync();
/*
        IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> includeProperty);
        public IIncludableQueryable<T, TNextProperty> ThenInclude<TPreviousProperty, TNextProperty>(
            IIncludableQueryable<T, TPreviousProperty> source,
            Expression<Func<TPreviousProperty, TNextProperty>> navigationPropertyPath);*/
    }
}
