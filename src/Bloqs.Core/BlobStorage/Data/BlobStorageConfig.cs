using Bloqs.BlobStorage.Data.Migrations;
using Bloqs.Data.Internals;

namespace Bloqs.BlobStorage.Data
{
    public class BlobStorageConfig
    {
        public static void Initialize(Storage storage)
        {
            Initialize(storage.ConnectionProperties.ToConnectionString());
        }

        public static void Initialize(string connectionString)
        {
            DatabaseHelper.CreateIfNotExists(connectionString);
            BlobDataMigrationRunnerHelper.MigrateToLatest(connectionString);
        }
    }
}
