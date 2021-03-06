using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Repository.Implementation
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(long? id);
        T Insert(T entity);
        void Update(T entity);
        void Delete(T entity);

        public DbSet<T> getEntities();
    }
}
