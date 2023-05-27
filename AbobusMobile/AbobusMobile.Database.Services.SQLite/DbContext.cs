using AbobusMobile.Database.Services.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

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
                    InitializeDatabase();
                }

                return _database;
            }
        }

        private void InitializeDatabase()
        {
            Database.CreateTablesAsync(_options.CreateFlags, _options.Tables);
        }
    }
}
