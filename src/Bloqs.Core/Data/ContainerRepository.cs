using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Bloqs.Data
{
    public class ContainerRepository
    {
        private readonly string _connectionString;

        public ContainerRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("connectionString is null or white space", "connectionString");

            _connectionString = connectionString;
        }

        public async Task<Container> FindAsync(string id)
        {
            const string sql = @"
SELECT
    [Id],
    [Name],
    [IsPublic],
    [PrimaryAccessKey],
    [SecondaryAccessKey],
    [Owner],
    [CreatedUtcDateTime],
    [LastModifiedUtcDateTime]
FROM [dbo].[Containers]
WITH (NOLOCK)
WHERE
    [Id] = @Id
";

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);

                var result = await cn.QueryAsync<Container>(sql, new {Id = id}).ConfigureAwait(false);

                return result.FirstOrDefault();
            }
        }

        public async Task<Container> FindByNameAsync(string name)
        {
            const string sql = @"
SELECT
    [Id],
    [Name],
    [IsPublic],
    [PrimaryAccessKey],
    [SecondaryAccessKey],
    [Owner],
    [CreatedUtcDateTime],
    [LastModifiedUtcDateTime]
FROM [dbo].[Containers]
WITH (NOLOCK)
WHERE
    [Name] = @Name
";

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);

                var result = await cn.QueryAsync<Container>(sql, new {Name = name}).ConfigureAwait(false);

                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Container>> GetListByOwnerAsync(string owner, int skip, int take)
        {
            const string sql = @"
SELECT
    [Id],
    [Name],
    [IsPublic],
    [PrimaryAccessKey],
    [SecondaryAccessKey],
    [Owner],
    [CreatedUtcDateTime],
    [LastModifiedUtcDateTime]
FROM [dbo].[Containers]
WITH (NOLOCK)
WHERE
    [Owner] = @Owner
ORDER BY
    [Name]
OFFSET @Skip ROWS
FETCH NEXT @Take ROWS ONLY
";

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);

                return
                    await
                        cn.QueryAsync<Container>(sql, new {Owner = owner, Skip = skip, Take = take})
                            .ConfigureAwait(false);
            }
        }

        public async Task<int> CountByOwnerAsync(string owner)
        {
            const string sql = @"
SELECT
    COUNT(Id)
FROM [dbo].[Containers]
WITH (NOLOCK)
WHERE
    [Owner] = @Owner
";

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);

                var ret = await cn.QueryAsync<int>(sql, new {Owner = owner}).ConfigureAwait(false);
                var array = ret as int[] ?? ret.ToArray();
                return array.Any() ? array.First() : 0;
            }
        }

        public async Task CreateAsync(Container container)
        {
            var schema = string.Format(@"
CREATE SCHEMA [{0}];
", container.Id);

            var blobtable = string.Format(@"
CREATE TABLE [{0}].[Blobs]
(
    [Name] NVARCHAR(256) NOT NULL, 
    [Image] VARBINARY(MAX) NOT NULL, 
    [Properties] NVARCHAR(MAX) NOT NULL DEFAULT ('{{}}'), 
    [Metadata] NVARCHAR(MAX) NOT NULL DEFAULT ('{{}}'), 

    CONSTRAINT [PK_Blobs] PRIMARY KEY ([Name]), 
)
", container.Id);

            const string sql = @"
INSERT INTO [dbo].[Containers] (
    [Id],
    [Name],
    [IsPublic],
    [PrimaryAccessKey],
    [SecondaryAccessKey],
    [Owner],
    [CreatedUtcDateTime],
    [LastModifiedUtcDateTime]
) VALUES (
    @Id,
    @Name,
    @IsPublic,
    @PrimaryAccessKey,
    @SecondaryAccessKey,
    @Owner,
    @CreatedUtcDateTime,
    @LastModifiedUtcDateTime
);
";

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);
                await cn.ExecuteAsync(schema).ConfigureAwait(false);
                await cn.ExecuteAsync(blobtable).ConfigureAwait(false);
                await cn.ExecuteAsync(sql, container).ConfigureAwait(false);
            }
        }

        public async Task UpdateAsync(string id, Container container)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new InvalidOperationException("id is empty");
            if (container == null) throw new ArgumentNullException("container");
            if (id != container.Id) throw new InvalidOperationException("update target is wrong id.");

            const string sql = @"
UPDATE [dbo].[Containers]
SET [IsPublic] = @IsPublic,
    [PrimaryAccessKey] = @PrimaryAccessKey,
    [SecondaryAccessKey] = @SecondaryAccessKey,
    [LastModifiedUtcDateTime] = @LastModifiedUtcDateTime
WHERE
    [Id] = @Id
";

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);

                await cn.ExecuteAsync(sql, container).ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(string id)
        {
            var target = await FindAsync(id).ConfigureAwait(false);
            if (target == null) throw new InvalidOperationException("target is not found.");

            const string sql = @"
DELETE FROM [dbo].[Containers]
WHERE
    [Id] = @Id
";

            var blobtable = string.Format(@"
DROP TABLE [{0}].[Blobs]
", target.Id);

            var schema = string.Format(@"
DROP SCHEMA [{0}];
", target.Id);

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);

                await cn.ExecuteAsync(sql, new {target.Id}).ConfigureAwait(false);

                await cn.ExecuteAsync(blobtable).ConfigureAwait(false);

                await cn.ExecuteAsync(schema).ConfigureAwait(false);
            }
        }

        public async Task<bool> ExistsNameAsync(string name)
        {
            const string sql = @"
SELECT
    [Name]
FROM [dbo].[Containers]
WITH (NOLOCK)
WHERE
    [Name] = @Name
";

            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);

                return (await cn.QueryAsync<string>(sql, new {Name = name}).ConfigureAwait(false)).Any();
            }
        }
    }
}