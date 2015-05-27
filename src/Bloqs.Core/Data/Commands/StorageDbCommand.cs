using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloqs.Data.Models;
using Dapper;

namespace Bloqs.Data.Commands
{
    public class StorageDbCommand : DbCommand
    {
        /// <summary></summary>
        /// <param name="connectionString"></param>
        public StorageDbCommand(string connectionString) : base(connectionString)
        {
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Storage> FindAsync(string id)
        {
            return FindAsync(id, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Storage> FindAsync(string id, CancellationToken cancellationToken)
        {
            const string sql = @"
SELECT
     Storages.[Id]
    ,Storages.[Owner]
    ,Storages.[Name]
    ,Storages.[StorageType]
    ,Storages.[ConnectionString]
    ,Storages.[ThresholdLength]
    ,Storages.[IsSuspended]
    ,Storages.[CreatedUtcDateTime]
    ,Storages.[LastModifiedUtcDateTime]
    ,UsedLength.[UsedLength]
FROM [dbo].[Storages] Storages
WITH (NOLOCK)
OUTER APPLY (
    SELECT
        [SumLength] = SUM(BlobAttributes.[Length])
    FROM [dbo].[BlobAttributes] BlobAttributes
    WITH (NOLOCK)
    WHERE
        BlobAttributes.StorageId = Storages.[Id]
) SumLength
CROSS APPLY (
    SELECT
        [UsedLength] = ISNULL(SumLength.[SumLength], 0)
) UsedLength
WHERE
    Storages.[Id] = @Id
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var results = await cn.QueryAsync<StorageDataModel>(sql, new {Id = id}).ConfigureAwait(false);
                var ret = results.FirstOrDefault();
                return ret == null ? null : ret.ToStorage();
            }
        }

        /// <summary></summary>
        /// <returns></returns>
        public Task<IEnumerable<Storage>> GetAllAsync()
        {
            return GetAllAsync(CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Storage>> GetAllAsync(CancellationToken cancellationToken)
        {
            const string sql = @"
SELECT
     Storages.[Id]
    ,Storages.[Owner]
    ,Storages.[Name]
    ,Storages.[StorageType]
    ,Storages.[ConnectionString]
    ,Storages.[ThresholdLength]
    ,Storages.[IsSuspended]
    ,Storages.[CreatedUtcDateTime]
    ,Storages.[LastModifiedUtcDateTime]
    ,UsedLength.[UsedLength]
FROM [dbo].[Storages] Storages
WITH (NOLOCK)
OUTER APPLY (
    SELECT
        [SumLength] = SUM(BlobAttributes.[Length])
    FROM [dbo].[BlobAttributes] BlobAttributes
    WITH (NOLOCK)
    WHERE
        BlobAttributes.StorageId = Storages.[Id]
) SumLength
CROSS APPLY (
    SELECT
        [UsedLength] = ISNULL(SumLength.[SumLength], 0)
) UsedLength
ORDER BY
    Storages.[Name]
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var results = await cn.QueryAsync<StorageDataModel>(sql).ConfigureAwait(false);
                return results.Select(x => x.ToStorage());
            }
        }

        /// <summary></summary>
        /// <param name="owner"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public Task<IEnumerable<Storage>> GetListByOwnerAsync(string owner, int skip, int take)
        {
            return GetListByOwnerAsync(owner, skip, take, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="owner"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Storage>> GetListByOwnerAsync(string owner, int skip, int take,
            CancellationToken cancellationToken)
        {
            const string sql = @"
SELECT
     Storages.[Id]
    ,Storages.[Owner]
    ,Storages.[Name]
    ,Storages.[StorageType]
    ,Storages.[ConnectionString]
    ,Storages.[ThresholdLength]
    ,Storages.[IsSuspended]
    ,Storages.[CreatedUtcDateTime]
    ,Storages.[LastModifiedUtcDateTime]
    ,UsedLength.[UsedLength]
FROM [dbo].[Storages] Storages
WITH (NOLOCK)
OUTER APPLY (
    SELECT
        [SumLength] = SUM(BlobAttributes.[Length])
    FROM [dbo].[BlobAttributes] BlobAttributes
    WITH (NOLOCK)
    WHERE
        BlobAttributes.StorageId = Storages.[Id]
) SumLength
CROSS APPLY (
    SELECT
        [UsedLength] = ISNULL(SumLength.[SumLength], 0)
) UsedLength
WHERE
    Storages.[Owner] = @Owner
ORDER BY
    Storages.[Name]
OFFSET @Skip ROWS
FETCH NEXT @Take ROWS ONLY
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var results = await cn.QueryAsync<StorageDataModel>(sql, new { Owner = owner, Skip = skip, Take = take }).ConfigureAwait(false);
                return results.Select(x => x.ToStorage());
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
            if (string.IsNullOrWhiteSpace(owner)) throw new ArgumentException("owner is empty");
            const string sql = @"
SELECT COUNT(*)
FROM [dbo].[Storages]
WHERE
    [Owner] = @Owner
";
            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var results = await cn.QueryAsync<int>(sql, new {Owner = owner}).ConfigureAwait(false);
                return results.FirstOrDefault();
            }
        }

        /// <summary></summary>
        /// <param name="storage"></param>
        /// <returns></returns>
        public Task CreateAsync(Storage storage)
        {
            return CreateAsync(storage, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="storage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CreateAsync(Storage storage, CancellationToken cancellationToken)
        {
            if (storage == null) throw new ArgumentNullException("storage");

            const string sql = @"
INSERT INTO [dbo].[Storages] (
    [Id],
    [Owner],
    [Name],
    [StorageType],
    [ConnectionString],
    [ThresholdLength],
    [IsSuspended],
    [CreatedUtcDateTime],
    [LastModifiedUtcDateTime]
) VALUES (
    @Id,
    @Owner,
    @Name,
    @StorageType,
    @ConnectionString,
    @ThresholdLength,
    @IsSuspended,
    @CreatedUtcDateTime,
    @LastModifiedUtcDateTime
)
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                await cn.ExecuteAsync(sql, new StorageDataModel(storage));
            }
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="storage"></param>
        /// <returns></returns>
        public Task UpdateAsync(string id, Storage storage)
        {
            return UpdateAsync(id, storage, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="storage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task UpdateAsync(string id, Storage storage, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new InvalidOperationException("id is empty");
            if (storage == null) throw new ArgumentNullException("storage");
            if (id != storage.Id) throw new InvalidOperationException();
            var target = await FindAsync(id, cancellationToken).ConfigureAwait(false);
            if (target == null) throw new InvalidOperationException("update target is not found.");

            const string sql = @"
UPDATE [dbo].[Storages]
SET [Owner] = @Owner,
    [Name] = @Name,
    [StorageType] = @StorageType,
    [ConnectionString] = @ConnectionString,
    [ThresholdLength] = @ThresholdLength,
    [IsSuspended] = @IsSuspended,
    [CreatedUtcDateTime] = @CreatedUtcDateTime,
    [LastModifiedUtcDateTime] = @LastModifiedUtcDateTime
WHERE
    [Id] = @Id
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                await cn.ExecuteAsync(sql, new StorageDataModel(storage));
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
            if (target == null) throw new InvalidOperationException("update target is not found.");

            const string sql = @"
DELETE FROM [dbo].[Storages]
WHERE
    [Id] = @Id
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                await cn.ExecuteAsync(sql, new {Id = id});
            }
        }
    }
}
