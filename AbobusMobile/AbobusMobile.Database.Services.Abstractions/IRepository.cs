using AbobusMobile.Database.Services.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Database.Services.Abstractions
{
    public interface IRepository<TEntity> where TEntity : BaseModel, new()
    {
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FirstOrDefaultAsync();

        Task<bool> AnyAsync();

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> Select(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> Select();

        Task<int> UpdateAsync(TEntity entity, bool insertIfExist = true);

        Task<int> InsertAsync(TEntity entity);

        Task<int> DeleteAsync(TEntity entity);
    }
}
