using System.IO;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Domain.Database;

namespace TwitterClone.UnitTests.Helpers;

internal static class DbContextHelper
{
    public static TwitterCloneDbContext CreateTestDb()
    {
        var tempFile = Path.GetTempFileName();

        var options = new DbContextOptionsBuilder<TwitterCloneDbContext>()
            .UseSqlite($"Data Source={tempFile}")
            .Options;

        var dbContext = new TwitterCloneDbContext(options);
        dbContext.Database.Migrate();

        return dbContext;
    }
}