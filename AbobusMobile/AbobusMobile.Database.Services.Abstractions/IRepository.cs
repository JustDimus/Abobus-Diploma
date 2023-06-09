﻿using AbobusMobile.Database.Services.Abstractions.Models;
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

        Task<List<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> SelectAsync();

        Task<int> UpdateAsync(TEntity entity);

        Task<int> InsertAsync(TEntity entity);

        Task<int> DeleteAsync(TEntity entity);
    }
}
