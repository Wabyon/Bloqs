using System.Data.SqlClient;

namespace Bloqs.Data
{
    /// <summary></summary>
    public class ConnectionProperties
    {
        /// <summary></summary>
        public string DataSource { get; set; }

        /// <summary></summary>
        public string InitialCatalog { get; set; }

        /// <summary></summary>
        public string UserID { get; set; }

        /// <summary></summary>
        public string Password { get; set; }

        /// <summary></summary>
        public int ConnectTimeout { get; set; }

        internal ConnectionProperties()
        {
            ConnectTimeout = 30;
        }

        /// <summary></summary>
        /// <param name="connectionString"></param>
        public ConnectionProperties(string connectionString)
        {
            var sb = new SqlConnectionStringBuilder(connectionString);
            DataSource = sb.DataSource;
            InitialCatalog = sb.InitialCatalog;
            UserID = sb.UserID;
            Password = sb.Password;
            ConnectTimeout = sb.ConnectTimeout;
        }

        /// <summary></summary>
        /// <param name="dataSource"></param>
        /// <param name="initialCatalog"></param>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        public ConnectionProperties(string dataSource, string initialCatalog, string userId, string password)
        {
            DataSource = dataSource;
            InitialCatalog = initialCatalog;
            UserID = userId;
            Password = password;
        }

        /// <summary></summary>
        /// <returns></returns>
        public string ToConnectionString()
        {
            var sb = new SqlConnectionStringBuilder
            {
                DataSource = DataSource,
                InitialCatalog = InitialCatalog,
                IntegratedSecurity = false,
                UserID = UserID,
                Password = Password,
                ConnectTimeout = ConnectTimeout,
            };

            return sb.ToString();
        }
    }
}