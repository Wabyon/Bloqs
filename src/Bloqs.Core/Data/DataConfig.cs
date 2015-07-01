using System;
using System.Data;
using Bloqs.BlobStorage.Data;
using Bloqs.Data.Commands;
using Bloqs.Data.Internals;
using Bloqs.Data.Migrations;
using Dapper;

namespace Bloqs.Data
{
    public class DataConfig
    {
        /// <summary></summary>
        /// <param name="connectionString"></param>
        /// <param name="migrationEnable"></param>
        public static void Start(string connectionString, bool migrationEnable = true)
        {
            SqlMapper.AddTypeMap(typeof(DateTime), DbType.DateTime2);
            SqlMapper.AddTypeMap(typeof(DateTime?), DbType.DateTime2);

            if (!migrationEnable) return;

            DatabaseHelper.CreateIfNotExists(connectionString);

            MigrationRunnerHelper.MigrateToLatest(connectionString);

            var storages = new StorageDbCommand(connectionString).GetAllAsync().Result;

            foreach (var storage in storages)
            {
                try
                {
                    BlobStorageConfig.Initialize(storage);
                }
                catch(Exception){}
            }
        }
    }
}
