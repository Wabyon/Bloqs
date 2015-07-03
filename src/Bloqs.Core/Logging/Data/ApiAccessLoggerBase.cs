using System;
using System.Data.SqlClient;
using Dapper;

namespace Bloqs.Logging.Data
{
    public abstract class ApiAccessLoggerBase : IApiAccessLogger
    {
        private readonly string _connectionString;
        protected ApiAccessLoggerBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected abstract Func<ApiAccessLog> GetApiAccessLog { get; }

        public void Write()
        {
            SqlConnection cn = null;
            try
            {
                var log = GetApiAccessLog();

                using (cn = new SqlConnection(_connectionString))
                {
                    cn.Open();
                    cn.Execute(@"
INSERT INTO [dbo].[ApiAccessLogs] (
    [ServerName],
    [UserName],
    [Url],
    [HttpMethod],
    [Path],
    [Query],
    [Form],
    [Controller],
    [Action],
    [UserHostAddress],
    [UserAgent]
) VALUES (
    @ServerName,
    @UserName,
    @Url,
    @HttpMethod,
    @Path,
    @Query,
    @Form,
    @Controller,
    @Action,
    @UserHostAddress,
    @UserAgent
)
", log);
                }
            }
            catch
            {
            }
            finally
            {
                if (cn != null)
                {
                    cn.Close();
                    cn.Dispose();
                }
            }
        }
    }
}