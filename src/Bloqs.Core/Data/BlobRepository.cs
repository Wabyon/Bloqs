using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Bloqs.Data.Internals;

namespace Bloqs.Data
{
    public class BlobRepository
    {
        private readonly string _connectionString;

        public BlobRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("connectionString is null or white space", "connectionString");

            _connectionString = connectionString;
        }

        public async Task<Blob> GetAsync(string containerId, string name)
        {
            var sql = string.Format(@"
SELECT
    [Name],
    [Image],
    [Properties],
    [Metadata]
FROM [{0}].[Blobs]
WITH (NOLOCK)
WHERE
    [Name] = @Name
", containerId);

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);

                var results = await cn.QueryAsync<BlobDataModel>(sql, new {Name = name}).ConfigureAwait(false);

                var array = results as BlobDataModel[] ?? results.ToArray();

                return !array.Any() ? null : array.First().ToBlob();
            }
        }

        public async Task<byte[]> GetImageAsync(string containerId, string name)
        {
            var sql = string.Format(@"
SELECT
    [Image]
FROM [{0}].[Blobs]
WITH (NOLOCK)
WHERE
    [Name] = @Name
", containerId);

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);

                var results = await cn.QueryAsync<byte[]>(sql, new { Name = name }).ConfigureAwait(false);
                var array = results as byte[][] ?? results.ToArray();

                return !array.Any() ? null : array.First();
            }
        }
        
        public async Task<BlobAttribute> GetAttributeAsync(string containerId, string name)
        {
            var sql = string.Format(@"
SELECT
    [Name],
    [Properties],
    [Metadata]
FROM [{0}].[Blobs]
WITH (NOLOCK)
WHERE
    [Name] = @Name
", containerId);

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);

                var results = await cn.QueryAsync<BlobDataModel>(sql, new { Name = name }).ConfigureAwait(false);

                var array = results as BlobDataModel[] ?? results.ToArray();

                return !array.Any() ? null : array.First().ToBlobAttribute();
            }
        }

        public async Task<IEnumerable<BlobAttribute>> GetAttributeListAync(string containerId, int skip, int take)
        {
            var sql = string.Format(@"
SELECT
    [Name],
    [Properties],
    [Metadata]
FROM [{0}].[Blobs]
WITH (NOLOCK)
ORDER BY
     [Name] ASC
OFFSET @Skip ROWS
FETCH NEXT @Take ROWS ONLY
", containerId);

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);

                var results = await cn.QueryAsync<BlobDataModel>(sql, new {Skip = skip, Take = take}).ConfigureAwait(false);

                return results.Select(x => x.ToBlobAttribute());
            }
        }
       
        public async Task<int> CountAllAync(string containerId)
        {
            var sql = string.Format(@"
SELECT
    COUNT(Name)
FROM [{0}].[Blobs]
WITH (NOLOCK)
", containerId);

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);

                var result =
                    await cn.QueryAsync<int>(sql).ConfigureAwait(false);

                var array = result as int[] ?? result.ToArray();

                return array.First();
            }
        }

        public async Task SaveAsync(string containerId, Blob blob)
        {
            if (blob == null) throw new ArgumentNullException("blob");
            var data = new BlobDataModel(blob);

            var sql = string.Format(@"
MERGE INTO [{0}].[Blobs] [Target]
USING (
    SELECT
        [Name] = @Name,
        [Image] = @Image,
        [Properties] = @Properties,
        [Metadata] = @Metadata
) [Source]
ON  [Target].[Name] = [Source].[Name]
WHEN MATCHED THEN
UPDATE
SET [Target].[Image] = [Source].[Image],
    [Target].[Properties] = [Source].[Properties],
    [Target].[Metadata] = [Source].[Metadata]
WHEN NOT MATCHED THEN
INSERT VALUES (
    [Source].[Name],
    [Source].[Image],
    [Source].[Properties],
    [Source].[Metadata]
);
", containerId);

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);
                await cn.ExecuteAsync(sql, data).ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(string containerId, string name)
        {
            var target = await GetAsync(containerId, name).ConfigureAwait(false);
            if (target == null) throw new InvalidOperationException("target not found.");

            var sql = string.Format(@"
DELETE FROM [{0}].[Blobs]
WHERE
    [Name] = @Name
", containerId);

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);
                await cn.ExecuteAsync(sql, new { Name = name }).ConfigureAwait(false);
            }
        }

        public async Task UpdateAttributeAsync(string containerId, BlobAttribute blobAttribute)
        {
            if (blobAttribute == null) throw new ArgumentNullException("blobAttribute");
            var data = new BlobDataModel(blobAttribute);

            var sql = string.Format(@"
UPDATE [{0}].[Blobs]
SET [Properties] = @Properties,
    [Metadata] = @Metadata
WHERE
    [Name] = @Name
", containerId);

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);
                await cn.ExecuteAsync(sql, data).ConfigureAwait(false);
            }
        }
    }
}