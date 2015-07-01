using System;
using System.Data.Common;

namespace Bloqs.Data.Commands
{
    public abstract class DbCommand
    {
        protected string ConnectionString { get; private set; }

        protected DbCommand(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("connectionString is null or white space", "connectionString");

            ConnectionString = connectionString;
        }

        protected DbConnection CreateConnection()
        {
            return CreateConnection(ConnectionString);
        }

        protected static DbConnection CreateConnection(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("connectionString is null or white space", "connectionString");

            var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");

            var cn = factory.CreateConnection();
            if (cn == null) throw new InvalidOperationException();

            cn.ConnectionString = connectionString;
            return cn;
        }
    }
}
