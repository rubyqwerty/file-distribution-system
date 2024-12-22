using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class ApplicationContext : models.MetadataContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionsLine = Environment.GetEnvironmentVariable("DB_HOST") ??
            "Data Source=/home/egor/projects/file-distribution-system/Shared/DB/metadata.db";

        var connection = new SqliteConnection(connectionsLine);
        connection.StateChange += (sender, e) =>
        {
            if (e.CurrentState == System.Data.ConnectionState.Open)
            {
                var command = connection.CreateCommand();
                // Будьте прокляты :)
                command.CommandText = "PRAGMA foreign_keys = ON;";
                command.ExecuteNonQuery();
            }
        };

        optionsBuilder.UseSqlite(connection);
    }
}