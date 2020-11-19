using BDSA2020.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BDSA2020.Models.Tests
{
    public abstract class SqlLiteContext
    {
        public SqliteConnection Connection { get; }

        public Context Context { get; }


        public SqlLiteContext()
        {
            // Arrange
            Connection = new SqliteConnection("Filename=:memory:");
            Connection.Open();
            var builder = new DbContextOptionsBuilder<Context>().UseSqlite(Connection);
            Context = new Context(builder.Options);
            Context.Database.EnsureCreated();
            // context.GenerateTestData();
        }
    }
}