using System.Linq.Expressions;
using BusinessObjects.DbContexts;
using Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Api.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public FplmsManagementContext RepositoryContext { get; set; }

        public RepositoryBase(FplmsManagementContext repositoryContext) => RepositoryContext = repositoryContext;
        // public RepositoryBase(FplmsManagementContext repositoryContext)
        // {
        //     RepositoryContext = new FplmsManagementContext();
        // }

        // public RepositoryBase()
        // {
        //     RepositoryContext = new FplmsManagementContext();
        // }

        public IQueryable<T> FindAll()
        {
            return RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            RepositoryContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);
        }
    }
}