namespace TheWayPOS.DAL.GenericInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    // it's just like the iRepository
    public interface IDomainManager<T, in TKey> where T : class
    {
        // IQuerable vs IEnumerable
        IEnumerable<T> All();
        T Get(TKey id);
        void Add(T entity);
        void Save(T entity);
        void Delete(T entity);
        void Attach(T entity, Func<T, object> id, bool entityModified = false);
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate = null);
    }
}
