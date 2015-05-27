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
    public class BlobDbCommand : DbCommand
    {
        /// <summary></summary>
        /// <param name="connectionString"></param>
        public BlobDbCommand(string connectionString) : base(connectionString)
        {
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Blob> FindAsync(string id)
        {
            return FindAsync(id, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Blob> FindAsync(string id, CancellationToken cancellationToken)
        {
            var attributes = await GetAttributesAsync(id, cancellationToken).ConfigureAwait(false);
            if (attributes == null) return null;

            var image = await GetImageAsync(attributes, cancellationToken).ConfigureAwait(false);

            return new Blob(attributes) {Image = image};
        }

        /// <summary></summary>
        /// <param name="container"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<Blob> FindAsync(Container container, string name)
        {
            return FindAsync(container, name, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="container"></param>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Blob> FindAsync(Container container, string name, CancellationToken cancellationToken)
        {
            var attributes = await GetAttributesAsync(container, name, cancellationToken).ConfigureAwait(false);
            if (attributes == null) return null;


            var image = await GetImageAsync(attributes, cancellationToken).ConfigureAwait(false);

            return new Blob(attributes) { Image = image };
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<BlobAttributes> GetAttributesAsync(string id)
        {
            return GetAttributesAsync(id, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<BlobAttributes> GetAttributesAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id is empty", "id");

            const string sql = @"
SELECT
    BlobAttributes.[Id],
    BlobAttributes.[ContainerId],
    BlobAttributes.[Name],
    BlobAttributes.[CacheControl],
    BlobAttributes.[ContentDisposition],
    BlobAttributes.[ContentEncoding],
    BlobAttributes.[ContentLanguage],
    BlobAttributes.[ContentMD5],
    BlobAttributes.[ContentType],
    BlobAttributes.[ETag],
    BlobAttributes.[Length],
    BlobAttributes.[Metadata],
    BlobAttributes.[CreatedUtcDateTime],
    BlobAttributes.[LastModifiedUtcDateTime],
    [StorageId] = Storages.[Id],
    [StorageOwner] = Storages.[Owner],
    [StorageName] = Storages.[Name],
    [StorageType] = Storages.[StorageType],
    Storages.[ConnectionString],
    Storages.[ThresholdLength],
    Storages.[IsSuspended]
FROM [dbo].[BlobAttributes] BlobAttributes
WITH (NOLOCK)
LEFT OUTER JOIN [dbo].[Storages] Storages
WITH (NOLOCK)
ON  BlobAttributes.[StorageId] = Storages.Id
WHERE
    BlobAttributes.[Id] = @Id
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var results = await cn.QueryAsync<BlobAttributesDataModel>(sql, new { Id = id }).ConfigureAwait(false);
                var array = results as BlobAttributesDataModel[] ?? results.ToArray();
                var data = array.FirstOrDefault();
                if (data == null) return null;

                var containerDbCommand = new ContainerDbCommand(ConnectionString);
                var container =
                    await containerDbCommand.FindAsync(data.ContainerId, CancellationToken.None).ConfigureAwait(false);

                return data.ToAttributes(container);
            }
        }

        /// <summary></summary>
        /// <param name="container"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<BlobAttributes> GetAttributesAsync(Container container, string name)
        {
            return GetAttributesAsync(container, name, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="container"></param>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<BlobAttributes> GetAttributesAsync(Container container, string name, CancellationToken cancellationToken)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (container.Account == null)
                throw new ArgumentException("account that parent of container is null", "container");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name is empty", "name");

            const string sql = @"
SELECT
    BlobAttributes.[Id],
    BlobAttributes.[ContainerId],
    BlobAttributes.[Name],
    BlobAttributes.[CacheControl],
    BlobAttributes.[ContentDisposition],
    BlobAttributes.[ContentEncoding],
    BlobAttributes.[ContentLanguage],
    BlobAttributes.[ContentMD5],
    BlobAttributes.[ContentType],
    BlobAttributes.[ETag],
    BlobAttributes.[Length],
    BlobAttributes.[Metadata],
    BlobAttributes.[CreatedUtcDateTime],
    BlobAttributes.[LastModifiedUtcDateTime],
    [StorageId] = Storages.[Id],
    [StorageOwner] = Storages.[Owner],
    [StorageName] = Storages.[Name],
    [StorageType] = Storages.[StorageType],
    Storages.[ConnectionString],
    Storages.[ThresholdLength],
    Storages.[IsSuspended]
FROM [dbo].[BlobAttributes] BlobAttributes
WITH (NOLOCK)
LEFT OUTER JOIN [dbo].[Storages] Storages
WITH (NOLOCK)
ON  BlobAttributes.[StorageId] = Storages.Id
WHERE
    BlobAttributes.[ContainerId] = @ContainerId
AND BlobAttributes.[Name] = @Name
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var results = await cn.QueryAsync<BlobAttributesDataModel>(sql, new {ContainerId = container.Id, Name = name }).ConfigureAwait(false);
                var array = results as BlobAttributesDataModel[] ?? results.ToArray();
                var data = array.FirstOrDefault();
                return data == null ? null : data.ToAttributes(container);
            }
        }

        /// <summary></summary>
        /// <param name="attributes"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static async Task<byte[]> GetImageAsync(BlobAttributes attributes, CancellationToken cancellationToken)
        {
            if (attributes == null) throw new ArgumentNullException("attributes");

            const string sql = @"
SELECT
    [AccountId],
    [ContainerId],
    [BlobId],
    [Image]
FROM [dbo].[Blobs]
WITH (NOLOCK)
WHERE
    [AccountId] = @AccountId
AND [ContainerId] = @ContainerId
AND [BlobId] = @BlobId
";
            using (var cn = CreateConnection(attributes.Storage.ConnectionProperties.ToConnectionString()))
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);

                var results =
                    await
                        cn.QueryAsync<BlobImageDataModel>(sql, new BlobImageDataModel(attributes)).ConfigureAwait(false);

                var data = results.FirstOrDefault();
                return data == null ? null : data.ToByteArray(attributes.Container.Account);
            }
        }

        /// <summary></summary>
        /// <param name="container"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public Task<IEnumerable<BlobAttributes>> GetAttributesListAsync(Container container, int skip, int take)
        {
            return GetAttributesListAsync(container, skip, take, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="container"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BlobAttributes>> GetAttributesListAsync(Container container, int skip, int take,
            CancellationToken cancellationToken)
        {
            if (skip < 0) throw new InvalidOperationException("skip must be over 1");
            if (take < 0) throw new InvalidOperationException("skip must be over 0");
            if (take == 0) return new BlobAttributes[0];

            if (container == null) throw new ArgumentNullException("container");
            if (container.Account == null)
                throw new ArgumentException("account that parent of container is null", "container");

            const string sql = @"
SELECT
    BlobAttributes.[Id],
    BlobAttributes.[ContainerId],
    BlobAttributes.[Name],
    BlobAttributes.[CacheControl],
    BlobAttributes.[ContentDisposition],
    BlobAttributes.[ContentEncoding],
    BlobAttributes.[ContentLanguage],
    BlobAttributes.[ContentMD5],
    BlobAttributes.[ContentType],
    BlobAttributes.[ETag],
    BlobAttributes.[Length],
    BlobAttributes.[Metadata],
    BlobAttributes.[CreatedUtcDateTime],
    BlobAttributes.[LastModifiedUtcDateTime],
    [StorageId] = Storages.[Id],
    [StorageOwner] = Storages.[Owner],
    [StorageName] = Storages.[Name],
    [StorageType] = Storages.[StorageType],
    Storages.[ConnectionString],
    Storages.[ThresholdLength],
    Storages.[IsSuspended]
FROM [dbo].[BlobAttributes] BlobAttributes
WITH (NOLOCK)
LEFT OUTER JOIN [dbo].[Storages] Storages
WITH (NOLOCK)
ON  BlobAttributes.[StorageId] = Storages.Id
WHERE
    BlobAttributes.[ContainerId] = @ContainerId
ORDER BY
    BlobAttributes.[Name] ASC
OFFSET @Skip ROWS
FETCH NEXT @Take ROWS ONLY
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var results =
                    await cn.QueryAsync<BlobAttributesDataModel>(sql, new { ContainerId = container.Id, Skip = skip, Take = take }).ConfigureAwait(false);

                return results.Select(x => x.ToAttributes(container));
            }
        }

        /// <summary></summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public Task<int> CountAsync(Container container)
        {
            return CountAsync(container, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="container"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> CountAsync(Container container, CancellationToken cancellationToken)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (container.Account == null)
                throw new ArgumentException("account that parent of container is null", "container");

            const string sql = @"
SELECT
    COUNT(Id)
FROM [dbo].[BlobAttributes]
WITH (NOLOCK)
WHERE
    [ContainerId] = @ContainerId
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var result =
                    await cn.QueryAsync<int>(sql, new {ContainerId = container.Id}).ConfigureAwait(false);
                var array = result as int[] ?? result.ToArray();
                return array.First();
            }
        }

        /// <summary></summary>
        /// <param name="blob"></param>
        /// <returns></returns>
        public Task SaveAsync(Blob blob)
        {
            return SaveAsync(blob, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="blob"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SaveAsync(Blob blob, CancellationToken cancellationToken)
        {
            if (blob == null) throw new ArgumentNullException("blob");

            const string sqlBlobAttributes = @"
MERGE INTO [dbo].[BlobAttributes] [Target]
USING (
    SELECT
        [Id] = @Id,
        [ContainerId] = @ContainerId,
        [Name] = @Name,
        [CacheControl] = @CacheControl,
        [ContentDisposition] = @ContentDisposition,
        [ContentEncoding] = @ContentEncoding,
        [ContentLanguage] = @ContentLanguage,
        [ContentMD5] = @ContentMD5,
        [ContentType] = @ContentType,
        [ETag] = @ETag,
        [Length] = @Length,
        [Metadata] = @Metadata,
        [CreatedUtcDateTime] = @CreatedUtcDateTime,
        [LastModifiedUtcDateTime] = @LastModifiedUtcDateTime,
        [StorageId] = @StorageId
) [Source]
ON  [Target].[Id] = [Source].[Id]
WHEN MATCHED THEN
UPDATE
SET [Target].[ContainerId] = [Source].[ContainerId],
    [Target].[Name] = [Source].[Name],
    [Target].[CacheControl] = [Source].[CacheControl],
    [Target].[ContentDisposition] = [Source].[ContentDisposition],
    [Target].[ContentEncoding] = [Source].[ContentEncoding],
    [Target].[ContentLanguage] = [Source].[ContentLanguage],
    [Target].[ContentMD5] = [Source].[ContentMD5],
    [Target].[ContentType] = [Source].[ContentType],
    [Target].[ETag] = [Source].[ETag],
    [Target].[Length] = [Source].[Length],
    [Target].[Metadata] = [Source].[Metadata],
    [Target].[CreatedUtcDateTime] = [Source].[CreatedUtcDateTime],
    [Target].[LastModifiedUtcDateTime] = [Source].[LastModifiedUtcDateTime],
    [Target].[StorageId] = [Source].[StorageId]
WHEN NOT MATCHED THEN
INSERT VALUES (
    [Source].[Id],
    [Source].[ContainerId],
    [Source].[Name],
    [Source].[CacheControl],
    [Source].[ContentDisposition],
    [Source].[ContentEncoding],
    [Source].[ContentLanguage],
    [Source].[ContentMD5],
    [Source].[ContentType],
    [Source].[ETag],
    [Source].[Length],
    [Source].[Metadata],
    [Source].[CreatedUtcDateTime],
    [Source].[LastModifiedUtcDateTime],
    [Source].[StorageId]
);
";
            const string sqlBlobImage = @"
MERGE INTO [dbo].[Blobs] [Target]
USING (
    SELECT
        [AccountId] = @AccountId,
        [ContainerId] = @ContainerId,
        [BlobId] = @BlobId,
        [Image] = @Image
) [Source]
ON  [Target].[AccountId] = [Source].[AccountId]
AND [Target].[ContainerId] = [Source].[ContainerId]
AND [Target].[BlobId] = [Source].[BlobId]
WHEN MATCHED THEN
UPDATE
SET [Target].[Image] = [Source].[Image]
WHEN NOT MATCHED THEN
INSERT VALUES (
    [Source].[AccountId],
    [Source].[ContainerId],
    [Source].[BlobId],
    [Source].[Image]
);
";

            var provider = new StorageProvider(ConnectionString);
            var storage = await provider.GetStorageAsync(blob, cancellationToken).ConfigureAwait(false);
            blob.Storage = storage;

            using (var cnBase = CreateConnection())
            using (var cnStorage = CreateConnection(blob.Storage.ConnectionProperties.ToConnectionString()))
            {
                await cnBase.OpenAsync(cancellationToken).ConfigureAwait(false);
                await cnStorage.OpenAsync(cancellationToken).ConfigureAwait(false);

                using (var trBase = cnBase.BeginTransaction())
                using (var trStorage = cnStorage.BeginTransaction())
                {
                    try
                    {
                        await
                            cnBase.ExecuteAsync(sqlBlobAttributes, blob.ToAttributesDataModel(), trBase)
                                .ConfigureAwait(false);
                        await
                            cnStorage.ExecuteAsync(sqlBlobImage, new BlobImageDataModel(blob), trStorage)
                                .ConfigureAwait(false);

                        trStorage.Commit();
                        trBase.Commit();
                    }
                    catch (Exception)
                    {
                        trStorage.Rollback();
                        trBase.Rollback();
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
            var target = await FindAsync(id, cancellationToken).ConfigureAwait(false);
            if (target == null) throw new InvalidOperationException("target not found.");

            const string sqlBlobAttributes = @"
DELETE FROM [dbo].[BlobAttributes]
WHERE
    [Id] = @Id
";

            const string sqlBlobImage = @"
DELETE FROM [dbo].[Blobs]
WHERE
    [AccountId] = @AccountId
AND [ContainerId] = @ContainerId
AND [BlobId] = @BlobId
";

            using (var cnBlobAttributes = CreateConnection())
            using (var cnBlobImage = CreateConnection(target.Storage.ConnectionProperties.ToConnectionString()))
            {
                await cnBlobAttributes.OpenAsync(cancellationToken).ConfigureAwait(false);
                await cnBlobImage.OpenAsync(cancellationToken).ConfigureAwait(false);

                using (var trBlobAttributes = cnBlobAttributes.BeginTransaction())
                using (var trBlobImage = cnBlobImage.BeginTransaction())
                {
                    try
                    {
                        await
                            cnBlobAttributes.ExecuteAsync(sqlBlobAttributes, new {Id = id}, trBlobAttributes)
                                .ConfigureAwait(false);
                        await
                            cnBlobImage.ExecuteAsync(sqlBlobImage, new BlobImageDataModel(target), trBlobImage)
                                .ConfigureAwait(false);

                        trBlobImage.Commit();
                        trBlobAttributes.Commit();
                    }
                    catch (Exception)
                    {
                        trBlobImage.Rollback();
                        trBlobAttributes.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary></summary>
        /// <param name="blobAttribute"></param>
        /// <returns></returns>
        public Task UpdateAttributesAsync(BlobAttributes blobAttribute)
        {
            return UpdateAttributesAsync(blobAttribute, CancellationToken.None);
        }

        /// <summary></summary>
        /// <param name="blobAttribute"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task UpdateAttributesAsync(BlobAttributes blobAttribute, CancellationToken cancellationToken)
        {
            if (blobAttribute == null) throw new ArgumentNullException("blobAttribute");

            const string sql = @"
UPDATE [dbo].[BlobAttributes]
SET [ContainerId] = @ContainerId,
    [Name] = @Name,
    [CacheControl] = @CacheControl,
    [ContentDisposition] = @ContentDisposition,
    [ContentEncoding] = @ContentEncoding,
    [ContentLanguage] = @ContentLanguage,
    [ContentMD5] = @ContentMD5,
    [ContentType] = @ContentType,
    [ETag] = @ETag,
    [Length] = @Length,
    [Metadata] = @Metadata,
    [CreatedUtcDateTime] = @CreatedUtcDateTime,
    [LastModifiedUtcDateTime] = @LastModifiedUtcDateTime,
    [StorageId] = @StorageId
WHERE
    [Id] = @Id
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                await cn.ExecuteAsync(sql, blobAttribute.ToAttributesDataModel()).ConfigureAwait(false);
            }
        }
    }
}