using Gifter.Common.Options;
using Gifter.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace Gifter.Services.Tests
{
    public abstract class TestWithSQLiteBase: IDisposable
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;
        protected readonly GifterDbContext DbContext;
        public IOptions<StoreOptions> StoreOptions { get; private set; }
        public string ImageDestPath { get; set; } = @"C:\Users\pkolo\Repos\Gifter\Gifter.Services.Tests\ImagesDest\";
        public string ImageSrcPath { get; set; } = @"C:\Users\pkolo\Repos\Gifter\Gifter.Services.Tests\Images\";


        protected TestWithSQLiteBase()
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<GifterDbContext>().UseSqlite(_connection).Options;

            DbContext = new GifterDbContext(options);
            DbContext.Database.EnsureCreated();

           StoreOptions = Options.Create(new StoreOptions()
            {
                BaseDirectory = @"C:\Users\pkolo\Repos\Gifter\Gifter.Services.Tests\ImagesDest",
                UserStoreMaxSize = 100,
                FileMaxSize = 5000000
            });
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
