using AbobusMobile.DAL.Services.Abstractions.Configurations;
using AbobusMobile.Database.Models;
using AbobusMobile.Database.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Configurations
{
    public class ConfigurationDataManager : IConfigurationDataManager
    {
        private IRepository<ConfigurationModel> _configurations;

        public ConfigurationDataManager(
            IRepository<ConfigurationModel> configurations)
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
        }

        public async Task DeleteAsync(string name)
        {
            var configuration = await GetByNameAsync(name);

            if (configuration != null)
            {
                await _configurations.DeleteAsync(configuration);
            }
        }

        public Task<ConfigurationModel> GetAsync(string name)
        {
            return _configurations.FirstOrDefaultAsync(i => i.Name == name);
        }

        public Task<List<ConfigurationModel>> SelectAsync(Expression<Func<ConfigurationModel, bool>> selector)
        {
            return _configurations.SelectAsync(selector);
        }

        public async Task UpdateAsync(string name, string value)
        {
            var configuration = await GetByNameAsync(name);

            if (configuration == null)
            {
                await InsertAsync(name, value);
            }
            else
            {
                configuration.Value = value;
                await _configurations.UpdateAsync(configuration);
            }
        }

        private async Task InsertAsync(string name, string value)
        {
            var configuration = new ConfigurationModel()
            {
                Name = name,
                Value = value
            };

            await _configurations.InsertAsync(configuration);
        }

        private async Task<ConfigurationModel> GetByNameAsync(string name)
        {
            return await _configurations.FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
