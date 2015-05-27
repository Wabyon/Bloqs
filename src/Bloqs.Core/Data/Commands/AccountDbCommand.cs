using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloqs.Data.Models;
using Dapper;

namespace Bloqs.Data.Commands
{
    /// <summary></summary>
    public class AccountDbCommand : DbCommand
    {
        /// <summary></summary>
        /// <param name="connectionString"></param>
        public AccountDbCommand(string connectionString) : base(connectionString)
        {
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Account> FindAsync(string id)
        {
            return FindAsync(id, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Account> FindAsync(string id, CancellationToken cancellationToken)
        {
            const string sql = @"
SELECT
    Accounts.[Id],
    Accounts.[Name],
    Accounts.[PrimaryAccessKey],
    Accounts.[SecondaryAccessKey],
    Accounts.[CryptKey],
    Accounts.[Owner],
    Accounts.[CreatedUtcDateTime],
    Accounts.[LastModifiedUtcDateTime],
    Accounts.[StorageType],
    Storages.Storages
FROM [dbo].[Accounts] Accounts
WITH (NOLOCK)
OUTER APPLY (
    SELECT (
        SELECT * FROM (
            SELECT
                StorageDataModel.[Id],
                StorageDataModel.Owner,
                StorageDataModel.[Name],
                StorageDataModel.[StorageType],
                StorageDataModel.[ConnectionString],
                StorageDataModel.[ThresholdLength],
                [IsSuspended] = CASE WHEN StorageDataModel.IsSuspended = 1 THEN 'true' ELSE 'false' END,
                UsedLength.[UsedLength],
                StorageDataModel.[CreatedUtcDateTime],
                StorageDataModel.[LastModifiedUtcDateTime]
            FROM [dbo].[AccountUsingStorages] AccountUsingStorages
            WITH (NOLOCK)
            INNER JOIN [dbo].[Storages] StorageDataModel
            WITH (NOLOCK)
            ON  AccountUsingStorages.StorageId = StorageDataModel.Id
            OUTER APPLY (
                SELECT
                    [SumLength] = SUM(BlobAttributes.[Length])
                FROM [dbo].[BlobAttributes] BlobAttributes
                WITH (NOLOCK)
                WHERE
                    BlobAttributes.StorageId = StorageDataModel.[Id]
            ) SumLength
            CROSS APPLY (
                SELECT
                    [UsedLength] = ISNULL(SumLength.[SumLength], 0)
            ) UsedLength
            WHERE
                AccountUsingStorages.AccountId = Accounts.Id
        ) StorageDataModel
        ORDER BY StorageDataModel.Name
        FOR XML AUTO, ROOT('Storages')
    ) Storages
) Storages
WHERE
    Accounts.[Id] = @Id
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);

                var result = await cn.QueryAsync<AccountDataModel>(sql, new { Id = id }).ConfigureAwait(false);
                var accountDataModels = result as AccountDataModel[] ?? result.ToArray();
                var datamodel = accountDataModels.FirstOrDefault();
                return datamodel == null ? null : datamodel.ToAccount();
            }
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<Account> FindByNameAsync(string name)
        {
            return FindByNameAsync(name, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Account> FindByNameAsync(string name, CancellationToken cancellationToken)
        {
            const string sql = @"
SELECT
    Accounts.[Id],
    Accounts.[Name],
    Accounts.[PrimaryAccessKey],
    Accounts.[SecondaryAccessKey],
    Accounts.[CryptKey],
    Accounts.[Owner],
    Accounts.[CreatedUtcDateTime],
    Accounts.[LastModifiedUtcDateTime],
    Accounts.[StorageType],
    Storages.Storages
FROM [dbo].[Accounts] Accounts
WITH (NOLOCK)
OUTER APPLY (
    SELECT (
        SELECT * FROM (
            SELECT
                StorageDataModel.[Id],
                StorageDataModel.Owner,
                StorageDataModel.[Name],
                StorageDataModel.[StorageType],
                StorageDataModel.[ConnectionString],
                StorageDataModel.[ThresholdLength],
                [IsSuspended] = CASE WHEN StorageDataModel.IsSuspended = 1 THEN 'true' ELSE 'false' END,
                UsedLength.[UsedLength],
                StorageDataModel.[CreatedUtcDateTime],
                StorageDataModel.[LastModifiedUtcDateTime]
            FROM [dbo].[AccountUsingStorages] AccountUsingStorages
            WITH (NOLOCK)
            INNER JOIN [dbo].[Storages] StorageDataModel
            WITH (NOLOCK)
            ON  AccountUsingStorages.StorageId = StorageDataModel.Id
            OUTER APPLY (
                SELECT
                    [SumLength] = SUM(BlobAttributes.[Length])
                FROM [dbo].[BlobAttributes] BlobAttributes
                WITH (NOLOCK)
                WHERE
                    BlobAttributes.StorageId = StorageDataModel.[Id]
            ) SumLength
            CROSS APPLY (
                SELECT
                    [UsedLength] = ISNULL(SumLength.[SumLength], 0)
            ) UsedLength
            WHERE
                AccountUsingStorages.AccountId = Accounts.Id
        ) StorageDataModel
        ORDER BY StorageDataModel.Name
        FOR XML AUTO, ROOT('Storages')
    ) Storages
) Storages
WHERE
    Accounts.[Name] = @Name
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);

                var result = await cn.QueryAsync<AccountDataModel>(sql, new { Name = name }).ConfigureAwait(false);
                var datamodel = result.FirstOrDefault();
                return datamodel == null ? null : datamodel.ToAccount();
            }
        }

        /// <summary></summary>
        /// <param name="owner"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public Task<IEnumerable<Account>> GetListByOwnerAsync(string owner, int skip, int take)
        {
            return GetListByOwnerAsync(owner, skip, take, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="owner"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Account>> GetListByOwnerAsync(string owner, int skip, int take, CancellationToken cancellationToken)
        {
            if (skip < 0) throw new InvalidOperationException("skip must be over 1");
            if (take < 0) throw new InvalidOperationException("skip must be over 0");
            if (take == 0) return new Account[0];
            
            const string sql = @"
SELECT
    Accounts.[Id],
    Accounts.[Name],
    Accounts.[PrimaryAccessKey],
    Accounts.[SecondaryAccessKey],
    Accounts.[CryptKey],
    Accounts.[Owner],
    Accounts.[CreatedUtcDateTime],
    Accounts.[LastModifiedUtcDateTime],
    Accounts.[StorageType],
    Storages.Storages
FROM [dbo].[Accounts] Accounts
WITH (NOLOCK)
OUTER APPLY (
    SELECT (
        SELECT * FROM (
            SELECT
                StorageDataModel.[Id],
                StorageDataModel.Owner,
                StorageDataModel.[Name],
                StorageDataModel.[StorageType],
                StorageDataModel.[ConnectionString],
                StorageDataModel.[ThresholdLength],
                [IsSuspended] = CASE WHEN StorageDataModel.IsSuspended = 1 THEN 'true' ELSE 'false' END,
                UsedLength.[UsedLength],
                StorageDataModel.[CreatedUtcDateTime],
                StorageDataModel.[LastModifiedUtcDateTime]
            FROM [dbo].[AccountUsingStorages] AccountUsingStorages
            WITH (NOLOCK)
            INNER JOIN [dbo].[Storages] StorageDataModel
            WITH (NOLOCK)
            ON  AccountUsingStorages.StorageId = StorageDataModel.Id
            OUTER APPLY (
                SELECT
                    [SumLength] = SUM(BlobAttributes.[Length])
                FROM [dbo].[BlobAttributes] BlobAttributes
                WITH (NOLOCK)
                WHERE
                    BlobAttributes.StorageId = StorageDataModel.[Id]
            ) SumLength
            CROSS APPLY (
                SELECT
                    [UsedLength] = ISNULL(SumLength.[SumLength], 0)
            ) UsedLength
            WHERE
                AccountUsingStorages.AccountId = Accounts.Id
        ) StorageDataModel
        ORDER BY StorageDataModel.Name
        FOR XML AUTO, ROOT('Storages')
    ) Storages
) Storages
WHERE
    Accounts.[Owner] = @Owner
ORDER BY
    Accounts.[Name]
OFFSET @Skip ROWS
FETCH NEXT @Take ROWS ONLY
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var results = await cn.QueryAsync<AccountDataModel>(sql, new {Owner = owner, Skip = skip, Take = take}).ConfigureAwait(false);
                return results.Select(x => x.ToAccount());
            }
        }

        /// <summary></summary>
        /// <param name="storage"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public Task<IEnumerable<Account>> GetListByStorageAsync(Storage storage, int skip, int take)
        {
            return GetListByStorageAsync(storage, skip, take, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="storage"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Account>> GetListByStorageAsync(Storage storage, int skip, int take, CancellationToken cancellationToken)
        {
            if (storage == null) throw new ArgumentNullException("storage");

            if (skip < 0) throw new InvalidOperationException("skip must be over 1");
            if (take < 0) throw new InvalidOperationException("skip must be over 0");
            if (take == 0) return new Account[0];

            const string sql = @"
SELECT
    Accounts.[Id],
    Accounts.[Name],
    Accounts.[PrimaryAccessKey],
    Accounts.[SecondaryAccessKey],
    Accounts.[CryptKey],
    Accounts.[Owner],
    Accounts.[CreatedUtcDateTime],
    Accounts.[LastModifiedUtcDateTime],
    Accounts.[StorageType],
    Storages.Storages
FROM [dbo].[Accounts] Accounts
WITH (NOLOCK)
OUTER APPLY (
    SELECT (
        SELECT * FROM (
            SELECT
                StorageDataModel.[Id],
                StorageDataModel.Owner,
                StorageDataModel.[Name],
                StorageDataModel.[StorageType],
                StorageDataModel.[ConnectionString],
                StorageDataModel.[ThresholdLength],
                [IsSuspended] = CASE WHEN StorageDataModel.IsSuspended = 1 THEN 'true' ELSE 'false' END,
                UsedLength.[UsedLength],
                StorageDataModel.[CreatedUtcDateTime],
                StorageDataModel.[LastModifiedUtcDateTime]
            FROM [dbo].[AccountUsingStorages] AccountUsingStorages
            WITH (NOLOCK)
            INNER JOIN [dbo].[Storages] StorageDataModel
            WITH (NOLOCK)
            ON  AccountUsingStorages.StorageId = StorageDataModel.Id
            OUTER APPLY (
                SELECT
                    [SumLength] = SUM(BlobAttributes.[Length])
                FROM [dbo].[BlobAttributes] BlobAttributes
                WITH (NOLOCK)
                WHERE
                    BlobAttributes.StorageId = StorageDataModel.[Id]
            ) SumLength
            CROSS APPLY (
                SELECT
                    [UsedLength] = ISNULL(SumLength.[SumLength], 0)
            ) UsedLength
            WHERE
                AccountUsingStorages.AccountId = Accounts.Id
        ) StorageDataModel
        ORDER BY StorageDataModel.Name
        FOR XML AUTO, ROOT('Storages')
    ) Storages
) Storages
WHERE EXISTS (
    SELECT *
    FROM [dbo].[AccountUsingStorages] AccountUsingStorages
    WHERE
        AccountUsingStorages.[AccountId] = Accounts.[Id]
    AND AccountUsingStorages.[StorageId] = @StorageId
)
ORDER BY
    Accounts.[Name]
OFFSET @Skip ROWS
FETCH NEXT @Take ROWS ONLY
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var results = await cn.QueryAsync<AccountDataModel>(sql, new { StorageId = storage.Id, Skip = skip, Take = take }).ConfigureAwait(false);
                return results.Select(x => x.ToAccount());
            }
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(string name)
        {
            return ExistsAsync(name, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
        {
            const string sql = @"
SELECT
    [Name]
FROM [dbo].[Accounts]
WITH (NOLOCK)
WHERE
    [Name] = @Name
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);

                return (await cn.QueryAsync<string>(sql, new { Name = name }).ConfigureAwait(false)).Any();
            }
        }

        /// <summary></summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public Task<int> CountByOwnerAsync(string owner)
        {
            return CountByOwnerAsync(owner, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="owner"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> CountByOwnerAsync(string owner, CancellationToken cancellationToken)
        {
            const string sql = @"
SELECT
    COUNT(*)
FROM [dbo].[Accounts]
WITH (NOLOCK)
WHERE
    [Owner] = @Owner
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);

                var ret = await cn.QueryAsync<int>(sql, new { Owner = owner }).ConfigureAwait(false);
                var array = ret as int[] ?? ret.ToArray();
                return array.Any() ? array.First() : 0;
            }
        }

        /// <summary></summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Task CreateAsync(Account account)
        {
            return CreateAsync(account, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CreateAsync(Account account, CancellationToken cancellationToken)
        {
            if (account == null) throw new ArgumentNullException("account");

            const string sql = @"
INSERT INTO [dbo].[Accounts] (
    [Id],
    [Name],
    [PrimaryAccessKey],
    [SecondaryAccessKey],
    [CryptKey],
    [Owner],
    [CreatedUtcDateTime],
    [LastModifiedUtcDateTime],
    [StorageType]
) VALUES (
    @Id,
    @Name,
    @PrimaryAccessKey,
    @SecondaryAccessKey,
    @CryptKey,
    @Owner,
    @CreatedUtcDateTime,
    @LastModifiedUtcDateTime,
    @StorageType
)
";

            const string linksql = @"
INSERT INTO [dbo].[AccountUsingStorages] (
    [AccountId],
    [StorageId]
) VALUES (
    @AccountId,
    @StorageId
)
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                using (var tr = cn.BeginTransaction())
                {
                    try
                    {
                        await cn.ExecuteAsync(sql, account, tr).ConfigureAwait(false);
                        foreach (var storage in account.Storages)
                        {
                            await
                                cn.ExecuteAsync(linksql, new {AccountId = account.Id, StorageId = storage.Id}, tr)
                                    .ConfigureAwait(false);
                        }
                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public Task UpdateAsync(string id, Account account)
        {
            return UpdateAsync(id, account, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task UpdateAsync(string id, Account account, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new InvalidOperationException("id is empty");
            if (account == null) throw new ArgumentNullException("account");
            if (id != account.Id) throw new InvalidOperationException("storage account is wrong");

            const string sql = @"
DELETE [dbo].[AccountUsingStorages] WHERE [AccountId] = @Id;

UPDATE [dbo].[Accounts]
SET [Name] = @Name,
    [PrimaryAccessKey] = @PrimaryAccessKey,
    [SecondaryAccessKey] = @SecondaryAccessKey,
    [CryptKey] = @CryptKey,
    [Owner] = @Owner,
    [CreatedUtcDateTime] = @CreatedUtcDateTime,
    [LastModifiedUtcDateTime] = @LastModifiedUtcDateTime,
    [StorageType] = @StorageType
WHERE
    [Id] = @Id
";

            const string linksql = @"
INSERT INTO [dbo].[AccountUsingStorages] (
    [AccountId],
    [StorageId]
) VALUES (
    @AccountId,
    @StorageId
)
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);

                using (var tr = cn.BeginTransaction())
                {
                    try
                    {
                        await cn.ExecuteAsync(sql, account, tr).ConfigureAwait(false);
                        foreach (var storage in account.Storages)
                        {
                            await
                                cn.ExecuteAsync(linksql, new { AccountId = account.Id, StorageId = storage.Id }, tr)
                                    .ConfigureAwait(false);
                        }
                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
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
INSERT INTO [dbo].[DeletedAccounts] (
    [Id],
    [Name],
    [Owner],
    [PrimaryAccessKey],
    [SecondaryAccessKey],
    [CryptKey],
    [CreatedUtcDateTime],
    [LastModifiedUtcDateTime],
    [DeletedUtcDateTime],
    [StorageType]
)
SELECT
    [Id],
    [Name],
    [Owner],
    [PrimaryAccessKey],
    [SecondaryAccessKey],
    [CryptKey],
    [CreatedUtcDateTime],
    [LastModifiedUtcDateTime],
    [DeletedUtcDateTime] = SYSUTCDATETIME(),
    [StorageType]
FROM [dbo].[Accounts]
WHERE
    [Id] = @Id

DELETE [dbo].[AccountUsingStorages] WHERE [AccountId] = @Id;

DELETE [dbo].[Accounts] WHERE [Id] = @Id
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                await cn.ExecuteAsync(sql, new {target.Id}).ConfigureAwait(false);
            }
        }
    }
}
