using AbobusMobile.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbobusMobile.DAL.Services.Abstractions.Configurations
{
    public static class ConfigurationExtensions
    {
        public static string GetString(this List<ConfigurationModel> configurations, string name)
            => configurations.First(config => config.Name == name).Value;

        public static int GetInt(this List<ConfigurationModel> configurations, string name)
            => int.Parse(GetString(configurations, name));

        public static DateTime GetDateTime(this List<ConfigurationModel> configurations, string name)
            => DateTime.Parse(GetString(configurations, name));
        public static Guid GetGuid(this List<ConfigurationModel> configurations, string name)
            => Guid.Parse(GetString(configurations, name));
    }
}
