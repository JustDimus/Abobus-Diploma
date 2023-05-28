using AbobusMobile.Database.Services.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Database.Services.SQLite
{
    public class DbContext : IDbContext
    {
        private readonly SQLiteAsyncConnection _database;
        private readonly DbOptions _options;

        private bool initialized = false;

        public DbContext(DbOptions dbOptions)
        {
            _options = dbOptions ?? throw new ArgumentNullException(nameof(dbOptions));

            _database = new SQLiteAsyncConnection(
                new SQLiteConnectionString(dbOptions.DatabasePath, dbOptions.OpenFlags, dbOptions.StoreDateTimeAsTicks));
        }

        internal SQLiteAsyncConnection Database
        {
            get
            {
                if (!initialized)
                {
                    InitializeDatabase().Wait();
                }

                return _database;
            }
        }

        private Task InitializeDatabase()
        {
            initialized = true;

            return Task.Run(async () => await Database.CreateTablesAsync(_options.CreateFlags, _options.Tables));
        }
    }
}
