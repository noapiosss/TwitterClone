using System.IO;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Domain.Database;

namespace TwitterClone.UnitTests.Helpers;

internal static class DbContextHelper
{
    public static TwitterCloneDbContext CreateTestDb()
    {
        var tempFile = Path.GetTempFileName();
        return CreateTestDb($"Data Source={tempFile}");
    }
    public static TwitterCloneDbContext CreateTestDb(string connectionString)
    {
        var options = new DbContextOptionsBuilder<TwitterCloneDbContext>()
            .UseSqlite(connectionString)
            .Options;

        var dbContext = new TwitterCloneDbContext(options);
        dbContext.Database.Migrate();

        return dbContext;
    }
}