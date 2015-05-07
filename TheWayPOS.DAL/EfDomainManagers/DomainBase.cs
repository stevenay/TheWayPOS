 namespace TheWayPOS.DAL.EfDomainManagers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using GenericInterfaces;

    /// <summary>
    /// It can also be called as Generic Repository
    /// </summary>
    /// <typeparam name="T"> Entity Class </typeparam>
    /// <typeparam name="Tkey"> Entity Primary Key </typeparam>
    public class DomainBase<C, T, Tkey> : IDomainManager<T, Tkey> 
        where T : class /* Domain Entity Base */
        where C : DbContext
    {
        private C _context;
        protected DomainBase(C context)
        {
            _context = context;
        }

        public virtual void Add(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Added;
        }   

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void Save(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Attach(T entity, Func<T, object> id, bool entityModified = false)
        {
            var entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var set = _context.Set<T>();

                T attachEntity = set.Find(id(entity));

                if (attachEntity != null)
                {
                    var attachedEntry = _context.Entry(attachEntity);

                    if (entityModified)
                    {
                        attachedEntry.CurrentValues.SetValues(entity);
                        attachedEntry.State = EntityState.Modified;
                    }
                    else
                    {
                        attachedEntry.State = EntityState.Unchanged;
                    }
                }
                else
                {
                    entry.State = EntityState.Added;
                }
            }

            _context.Set<T>().Attach(entity);
        }

        public virtual T Get(Tkey id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> All()
        {
            return _context.Set<T>().ToList<T>();
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual int Count(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
                return _context.Set<T>().Count();
            else
                return _context.Set<T>().Count(predicate);
        }
    }
}
