using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloqs.Data.Extensions;
using Bloqs.Data.Models;
using Dapper;

namespace Bloqs.Data.Commands
{
    /// <summary></summary>
    public class ContainerDbCommand : DbCommand
    {
        /// <summary></summary>
        /// <param name="connectionString"></param>
        public ContainerDbCommand(string connectionString) : base(connectionString)
        {
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Container> FindAsync(string id)
        {
            return FindAsync(id, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Container> FindAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id is empty", "id");

            const string sql = @"
SELECT
    [Id],
    [AccountId],
    [Name],
    [IsPublic],
    [Metadata],
    [CreatedUtcDateTime],
    [LastModifiedUtcDateTime]
FROM [dbo].[Containers]
WITH (NOLOCK)
WHERE
    [Id] = @Id
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var result = await cn.QueryAsync<ContainerDataModel>(sql, new { Id = id }).ConfigureAwait(false);
                var data = result.FirstOrDefault();
                if (data == null) return null;

                var account =
                    await
                        new AccountDbCommand(ConnectionString).FindAsync(data.AccountId, cancellationToken)
                            .ConfigureAwait(false);

                var ret = data.ToContainer(account);
                return ret;
            }
        }

        /// <summary></summary>
        /// <param name="account"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<Container> FindAsync(Account account, string name)
        {
            return FindAsync(account, name, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="account"></param>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Container> FindAsync(Account account, string name, CancellationToken cancellationToken)
        {
            if (account == null) throw new ArgumentNullException("account");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("id is empty", "name");

            const string sql = @"
SELECT
    [Id],
    [Name],
    [IsPublic],
    [Metadata],
    [CreatedUtcDateTime],
    [LastModifiedUtcDateTime]
FROM [dbo].[Containers]
WITH (NOLOCK)
WHERE
    [AccountId] = @AccountId
AND [Name] = @Name
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var result = await cn.QueryAsync<ContainerDataModel>(sql, new { AccountId = account.Id, Name = name }).ConfigureAwait(false);
                var data = result.FirstOrDefault();
                if (data == null) return null;
                var ret = data.ToContainer(account);
                return ret;
            }
        }

        /// <summary></summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Task<int> CountAsync(Account account)
        {
            return CountAsync(account, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> CountAsync(Account account, CancellationToken cancellationToken)
        {
            if (account == null) throw new ArgumentNullException("account");

            const string sql = @"
SELECT
    COUNT(*)
FROM [dbo].[Containers]
WITH (NOLOCK)
WHERE
    [AccountId] = @AccountId
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var ret = await cn.QueryAsync<int>(sql, new {AccountId = account.Id}).ConfigureAwait(false);
                var array = ret as int[] ?? ret.ToArray();
                return array.Any() ? array.First() : 0;
            }
        }

        /// <summary></summary>
        /// <param name="account"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public Task<IEnumerable<Container>> GetListAsync(Account account, int skip, int take)
        {
            return GetListAsync(account, skip, take, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="account"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Container>> GetListAsync(Account account, int skip, int take,
            CancellationToken cancellationToken)
        {
            if (skip < 0) throw new InvalidOperationException("skip must be over 1");
            if (take < 0) throw new InvalidOperationException("skip must be over 0");
            if (take == 0) return new Container[0];
            
            if (account == null) throw new ArgumentNullException("account");

            const string sql = @"
SELECT
    [Id],
    [Name],
    [IsPublic],
    [Metadata],
    [CreatedUtcDateTime],
    [LastModifiedUtcDateTime]
FROM [dbo].[Containers]
WITH (NOLOCK)
WHERE
    [AccountId] = @AccountId
ORDER BY
     [Name] ASC
OFFSET @Skip ROWS
FETCH NEXT @Take ROWS ONLY
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var results =
                    await cn.QueryAsync<ContainerDataModel>(sql, new {AccountId = account.Id, Skip = skip, Take = take }).ConfigureAwait(false);

                return results.Select(x => x.ToContainer(account));
            }
        }

        /// <summary></summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public Task SaveAsync(Container container)
        {
            return SaveAsync(container, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="container"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SaveAsync(Container container, CancellationToken cancellationToken)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (container.Account == null)
                throw new ArgumentException("account that parent of container is null", "container");

            const string sql = @"
MERGE INTO [dbo].[Containers] [Target]
USING (
SELECT
    [Id] = @Id,
    [AccountId] = @AccountId,
    [Name] = @Name,
    [IsPublic] = @IsPublic,
    [Metadata] = @Metadata,
    [CreatedUtcDateTime] = @CreatedUtcDateTime,
    [LastModifiedUtcDateTime] = @LastModifiedUtcDateTime
) [Source]
ON  [Target].[AccountId] = [Source].[AccountId]
AND [Target].[Id] = [Source].[Id]
WHEN MATCHED THEN
UPDATE
SET [Target].[IsPublic] = [Source].[IsPublic],
    [Target].[Metadata] = [Source].[Metadata],
    [Target].[CreatedUtcDateTime] = [Source].[CreatedUtcDateTime],
    [Target].[LastModifiedUtcDateTime] = [Source].[LastModifiedUtcDateTime]
WHEN NOT MATCHED THEN
INSERT (
    [Id],
    [AccountId],
    [Name],
    [IsPublic],
    [Metadata],
    [CreatedUtcDateTime],
    [LastModifiedUtcDateTime]
) VALUES (
    [Source].[Id],
    [Source].[AccountId],
    [Source].[Name],
    [Source].[IsPublic],
    [Source].[Metadata],
    [Source].[CreatedUtcDateTime],
    [Source].[LastModifiedUtcDateTime]
);
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                await cn.ExecuteAsync(sql, container.ToDataModel()).ConfigureAwait(false);
            }
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteAsync(string id)
        {
            return DeleteAsync(id, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new InvalidOperationException("id is empty");

            var target = await FindAsync(id, cancellationToken).ConfigureAwait(false);
            if (target == null) throw new InvalidOperationException("target is not found.");

            const string sql = @"
DELETE FROM [dbo].[Containers]
WHERE
    [Id] = @Id
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                await cn.ExecuteAsync(sql, new {AccountId = target.Account.Id, target.Id}).ConfigureAwait(false);
            }
        }

        /// <summary></summary>
        /// <param name="account"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(Account account, string name)
        {
            return ExistsAsync(account, name, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="account"></param>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Account account, string name, CancellationToken cancellationToken)
        {
            if (account == null) throw new ArgumentNullException("account");
            if (string.IsNullOrWhiteSpace(name)) throw new InvalidOperationException("name is empty");

            const string sql = @"
SELECT
    [Name]
FROM [dbo].[Containers]
WITH (NOLOCK)
WHERE
    [AccountId] = @AccountId
AND [Name] = @Name
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                return (await cn.QueryAsync<string>(sql, new {AccountId = account.Id, Name = name}).ConfigureAwait(false)).Any();
            }
        }
    }
}