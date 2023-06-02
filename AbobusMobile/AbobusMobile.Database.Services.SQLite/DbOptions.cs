using AbobusMobile.Database.Services.Abstractions.Models;
using AbobusMobile.Utilities.Extensions;
using SQLite;
using System;
using System.Linq;
using System.Reflection;

namespace AbobusMobile.Database.Services.SQLite
{
    public class DbOptions
    {
        internal string DatabasePath { get; private set; }

        internal SQLiteOpenFlags OpenFlags { get; private set; } = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

        internal CreateFlags CreateFlags { get; private set; } = CreateFlags.None;

        internal bool StoreDateTimeAsTicks { get; private set; } = false;

        internal Type[] Tables { get; private set; }

        public DbOptions UseDateTimeAsTicks()
        {
            StoreDateTimeAsTicks = true;

            return this;
        }

        public DbOptions LoadTablesFromAssembly(Assembly assembly)
        {
            Tables = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(BaseModel))).ToArray().Add(Tables);

            return this;
        }

        public DbOptions UseSQLiteOpenFlags(SQLiteOpenFlags flags)
        {
            OpenFlags = flags;

            return this;
        }

        public DbOptions UseTableCreateFlags(CreateFlags flags)
        {
            CreateFlags = flags;

            return this;
        }

        public DbOptions UseDatabasePath(string path)
        {
            DatabasePath = path;

            return this;
        }
    }
}
