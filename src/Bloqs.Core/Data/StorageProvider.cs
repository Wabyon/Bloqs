using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloqs.Data.Commands;
using Bloqs.Data.Models;
using Dapper;

namespace Bloqs.Data
{
    internal class StorageProvider : DbCommand
    {
        public StorageProvider(string connectionString) : base(connectionString)
        {
        }

        public async Task<Storage> GetStorageAsync(BlobAttributes blobAttributes, CancellationToken cancellationToken)
        {
            var blobDbCommand = new BlobDbCommand(ConnectionString);

            var target =
                await
                    blobDbCommand.GetAttributesAsync(blobAttributes.Id, cancellationToken).ConfigureAwait(false);

            if (target != null && target.Storage != null && target.Storage != Storage.None) return target.Storage;

            var account = blobAttributes.Container.Account;
            if (account.StorageType == StorageType.Common)
            {
                return await GetCommonStorageAsync(blobAttributes, cancellationToken).ConfigureAwait(false);
            }

            if (!account.Storages.Any() || account.Storages.All(x => x.IsSuspended || x.ThresholdLength == 0))
            {
                return Storage.None;
            }

            var storage = account.Storages.OrderBy(x =>
            {
                var threshold = x.ThresholdLength * 1.0;
                var used = x.UsedLength;
                var thisTimeUse = blobAttributes.Properties.Length;

                return (used + thisTimeUse) / threshold;
            }).First();

            return storage;
        }

        private async Task<Storage> GetCommonStorageAsync(BlobAttributes blobAttributes, CancellationToken cancellationToken)
        {
            const string sql = @"
SELECT TOP 1
     Storages.[Id]
    ,Storages.[Owner]
    ,Storages.[Name]
    ,Storages.[StorageType]
    ,Storages.[ConnectionString]
    ,Storages.[ThresholdLength]
    ,Storages.[IsSuspended]
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
    Storages.[IsSuspended] = 0
AND Storages.[ThresholdLength] <> 0
AND Storages.[StorageType] = 2
ORDER BY
    (UsedLength.[UsedLength] + @ThisTimeUseLength) / (Storages.[ThresholdLength] * 1.0)
";

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync(cancellationToken).ConfigureAwait(false);
                var resultSets = await cn.QueryAsync<StorageDataModel>(sql, new { ThisTimeUseLength  = blobAttributes.Properties.Length }).ConfigureAwait(false);
                var datamodel = resultSets.FirstOrDefault();
                return datamodel == null ? Storage.None : datamodel.ToStorage();
            }
        }
    }
}
