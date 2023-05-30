using AbobusMobile.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Abstractions.Configurations
{
    public interface IConfigurationDataManager
    {
        Task<ConfigurationModel> GetAsync(string name);

        Task<List<ConfigurationModel>> SelectAsync(Expression<Func<ConfigurationModel, bool>> selector);

        Task DeleteAsync(string name);

        Task UpdateAsync(string name, string value);
    }
}
