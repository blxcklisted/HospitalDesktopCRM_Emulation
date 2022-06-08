using Project.DAL.Context;
using Project.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace Project.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext db)
        {
            this._db = db;
            _dbSet = _db.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet;
        public async Task<IEnumerable<T>> GetAllAsync() => await Task.Run(() => GetAll());
        public T Get(int id) => _dbSet.Find(id);

        //public void Update(int id, T entity)
        //{
        //    var existingEntity = _dbSet.Find(id);
        //    _dbSet.Attach(existingEntity);
        //    _db.Entry(existingEntity).State = EntityState.Modified;
        //}
        public void Update(int id, T entity) => _db.Entry(entity).State = EntityState.Modified;
        public void AddOrUpdate(T entity) => _dbSet.AddOrUpdate(entity);
        public void Create(T entity) => _dbSet.Add(entity);
        public void Delete(T entity) => _dbSet.Remove(entity);

        public void Save() => _db.SaveChanges();
        public async void SaveAsync() => await _db.SaveChangesAsync();
    }
}