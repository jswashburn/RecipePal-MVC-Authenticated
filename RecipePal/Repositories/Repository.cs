using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipePal.Data;
using RecipePal.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipePal.Repositories
{
    public class Repository<TContext, TModel> : IRepository<TModel> 
        where TModel : BaseEntity 
        where TContext : IdentityDbContext
    {
        TContext _context;
        DbSet<TModel> _entities;

        public Repository(TContext context)
        {
            _context = context;
            _entities = _context.Set<TModel>();
        }

        public IEnumerable<TModel> Get()
        {
            return _entities.AsEnumerable();
        }

        public TModel Get(int id)
        {
            return _entities.Find(id);
        }

        public TModel Insert(TModel item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Cannot insert null");

            _entities.Add(item);
            Save();

            return item;
        }

        public TModel Update(TModel item)
        {
            _context.Entry(item).State = EntityState.Modified;
            Save();
            return item;
        }

        public TModel Delete(int id)
        {
            TModel entity = _entities.Find(id);
            _entities.Remove(entity);
            Save();
            return entity;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
