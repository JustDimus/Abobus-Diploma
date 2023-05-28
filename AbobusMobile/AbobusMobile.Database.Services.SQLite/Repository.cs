using AbobusMobile.Database.Services.Abstractions;
using AbobusMobile.Database.Services.Abstractions.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Database.Services.SQLite
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseModel, new()
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private SQLiteAsyncConnection Database => _dbContext.Database;
        private AsyncTableQuery<TEntity> Entities => Database.Table<TEntity>();

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Database.Table<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public Task<TEntity> FirstOrDefaultAsync()
        {
            return Entities.FirstOrDefaultAsync();
        }

        public async Task<bool> AnyAsync()
        {
            var entity = await FirstOrDefaultAsync();
            return entity != null;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await FirstOrDefaultAsync(predicate);
            return entity != null;
        }

        public Task<List<TEntity>> Select(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(predicate).ToListAsync();
        }

        public Task<List<TEntity>> Select()
        {
            return Entities.ToListAsync();
        }

        public Task<int> UpdateAsync(TEntity entity, bool insertIfExist = true)
        {
            if (insertIfExist)
            {
                return Database.InsertOrReplaceAsync(entity);
            }

            return Database.UpdateAsync(entity);
        }

        public Task<int> InsertAsync(TEntity entity)
        {
            return Database.InsertAsync(entity);
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            return Database.DeleteAsync(entity);
        }
    }
}
