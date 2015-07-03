using Bloqs.Data.Internals;
using Bloqs.Logging.Data.Migrations;

namespace Bloqs.Logging.Data
{
    public class LogDataConfig
    {
        public static void Initialize(string connectionString)
        {
            DatabaseHelper.CreateIfNotExists(connectionString);
            LogDataMigrationRunnerHelper.MigrateToLatest(connectionString);
        }
    }
}
