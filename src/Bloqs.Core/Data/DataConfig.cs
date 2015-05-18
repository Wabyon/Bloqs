using System;
using System.Data;
using Bloqs.Data.Internals;
using Dapper;

namespace Bloqs.Data
{
    public class DataConfig
    {
        /// <summary>Initialize Database.
        /// Call Application_Start this method.</summary>
        /// <param name="connectionString"></param>
        /// <param name="migrationEnable"></param>
        public static void Start(string connectionString, bool migrationEnable = true)
        {
            // Mapping DateTime to SqlServer datetime2
            SqlMapper.AddTypeMap(typeof(DateTime), DbType.DateTime2);
            SqlMapper.AddTypeMap(typeof(DateTime?), DbType.DateTime2);

            if (!migrationEnable) return;

            // If database is not exists, create database
            DatabaseHelper.CreateIfNotExists(connectionString);

            // Migration dababase
            MigrationRunnerHelper.MigrateToLatest(connectionString);
        }
    }
}
