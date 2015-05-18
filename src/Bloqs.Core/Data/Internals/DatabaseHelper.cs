using System.Data.SqlClient;

namespace Bloqs.Data.Internals
{
    internal class DatabaseHelper
    {
        public static void CreateIfNotExists(string connectionString)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = connectionStringBuilder.InitialCatalog;

            connectionStringBuilder.InitialCatalog = "master";

            using (var connection = new SqlConnection(connectionStringBuilder.ToString()))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = string.Format(@"
SELECT
    name
FROM master.sys.databases
WHERE
    name = '{0}'
",
                 databaseName);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) return;
                    }

                    command.CommandText = string.Format("CREATE DATABASE [{0}]", databaseName);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}