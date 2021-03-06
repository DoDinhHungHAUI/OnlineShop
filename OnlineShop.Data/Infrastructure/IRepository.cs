using System;
using System.Linq;
using System.Linq.Expressions;

namespace OnlineShop.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        //marks an entity as new
        void Add(T emtity);

        //Marks an entity as modified
        void Update(T entity);

        //Marks an entuty to be removed
        void Delete(T entity);

        //Delete multi records
        void DeleteMulti(Expression<Func<T, bool>> where);

        //Get an entity by int id
        T GetSingleById(int id);

        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);

        IQueryable<T> GetAll(string[] includes = null);

        IQueryable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);

        IQueryable<T> GetMultiPaging(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, string[] includes = null);

        int Count(Expression<Func<T, bool>> where);

        bool CheckContains(Expression<Func<T, bool>> preducate);
    }
}