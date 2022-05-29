using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Saltimer.Test.HandlerTests;

namespace efCdCollection.Tests.Data;
public class SqliteInMemory : BaseHandlerTest, IDisposable
{
    private readonly DbConnection _connection;
    public SqliteInMemory()
      : base(
          new DbContextOptionsBuilder<SaltimerDBContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options)
    {
        _connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
    }
    private static DbConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        return connection;
    }
    public void Dispose() => _connection.Dispose();
}