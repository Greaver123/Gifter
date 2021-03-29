using Gifter.Common.Options;
using Gifter.DataAccess;
using Gifter.DataAccess.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Gifter.Services.Tests
{
    public abstract class TestWithSQLiteBase : IDisposable
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;
        protected readonly GifterDbContext DbContext;
        public IOptions<StoreOptions> StoreOptions { get; private set; }

        public string UserId { get; set; } = "fjldsnlvkcxnpj";
        public string ImageDestPath { get; set; } = @"C:\Users\pkolo\Repos\Gifter\Gifter.Services.Tests\ImagesDest\";
        public string ImageSrcPath { get; set; } = @"C:\Users\pkolo\Repos\Gifter\Gifter.Services.Tests\Images\";
        public string UserDirectory
        {
            get
            {
                return $"{ImageDestPath}\\{UserId}";
            }
        }

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

            var user = new User()
            {
                Auth0Id = UserId,
                Auth0Email = "test@gmail.com",
                Auth0Username = "JohnDoe",
            };
            DbContext.Users.Add(user);
            DbContext.SaveChanges();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
