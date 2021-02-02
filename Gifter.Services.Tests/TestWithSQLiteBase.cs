using Gifter.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace Gifter.Services.Tests
{
    public abstract class TestWithSQLiteBase: IDisposable
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;

        protected readonly GifterDbContext DbContext;

        protected TestWithSQLiteBase()
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<GifterDbContext>().UseSqlite(_connection).Options;

            DbContext = new GifterDbContext(options);
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
