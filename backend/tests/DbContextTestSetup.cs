using System;
using System.Collections.Generic;
using Xunit;

using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

using api.Context;
using api.Models;

namespace tests
{
    // Class for building and disposing dbcontext
    public class DatabaseFixture : IDisposable
    {
        public DcdDbContext context { get; private set; }
        private readonly SqliteConnection _connection;

        public DatabaseFixture()
        {
            DbContextOptionsBuilder<DcdDbContext> builder = new DbContextOptionsBuilder<DcdDbContext>();
            string connectionString = new SqliteConnectionStringBuilder { DataSource = ":memory:", Cache = SqliteCacheMode.Shared }.ToString();
            _connection = new SqliteConnection(connectionString);
            _connection.Open();
            builder.EnableSensitiveDataLogging();
            builder.UseSqlite(_connection);
            context = new DcdDbContext(builder.Options);
            context.Database.EnsureCreated();
            InitContent.PopulateDb(context);
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
