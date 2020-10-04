using DominosGeolocation.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DominosGeolocation.Data
{
    public abstract class BaseEntity
    {

    }
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity);
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
        T GetById(object id);
        void Insert(T entity);
        void Insert(IEnumerable<T> entities);
        void Update(T entity);
        void Update(IEnumerable<T> entities);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        IQueryable<T> IncludeMany(params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetSql(string sql);
    }

    public class GeneralRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly GeolocationContext _context;
        private DbSet<T> _entities;

        public GeneralRepository(GeolocationContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public virtual T GetById(object id)
        {
            return Entities.Find(id);
        }

        public void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _entities.Add(entity);
            _context.SaveChanges();
        }

        public virtual void Insert(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
                Entities.Add(entity);

            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _context.SaveChanges();
        }

        public virtual void Update(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            Entities.Remove(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
                Entities.Remove(entity);
            _context.SaveChanges();
        }

        public IQueryable<T> IncludeMany(params Expression<Func<T, object>>[] includes)
        {
            return _entities.IncludeMultiple(includes);
        }

        public IEnumerable<T> GetSql(string sql)
        {
            return Entities.FromSqlRaw(sql).AsNoTracking();
        }

        public virtual IQueryable<T> Table => Entities;

        public virtual IQueryable<T> TableNoTracking => Entities.AsNoTracking();

        protected virtual DbSet<T> Entities => _entities ?? (_entities = _context.Set<T>());

        public async Task<T> AddAsync(T entity)
        {
            var addedEntity = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
