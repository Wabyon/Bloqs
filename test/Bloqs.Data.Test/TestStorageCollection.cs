using System.Collections.ObjectModel;
using Bloqs.BlobStorage.Data;
using Bloqs.Data.Commands;

namespace Bloqs.Data
{
    public class TestStorageCollection : KeyedCollection<string, Storage>
    {
        protected override string GetKeyForItem(Storage item)
        {
            return item.Id;
        }

        public void InitializeDatabases()
        {
            var storageDbCommand = new StorageDbCommand(Constants.ConnectionString);

            foreach (var storage in this)
            {
                BlobStorageConfig.Initialize(storage);
                TestUtilities.TruncateAllTables(storage.ConnectionProperties.ToConnectionString());
                storageDbCommand.CreateAsync(storage).Wait();
            }
        }
    }
}