using System.Data;
using System.Data.SqlClient;

namespace Bloqs.Data
{
    internal static class TestUtilities
    {
        public static void InitializeDatabase()
        {
            DataConfig.Start(Constants.ConnectionString);

            TruncateAllTables(Constants.ConnectionString);

            MockStorages.Initialize();

            Mock.RegisterDadatabase();
        }

        public static void TruncateAllTables(string connecitonString)
        {
            using (var cn = new SqlConnection(connecitonString))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = Constants.AllTableTruncateCommandText;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
            }            
        }
    }
}
