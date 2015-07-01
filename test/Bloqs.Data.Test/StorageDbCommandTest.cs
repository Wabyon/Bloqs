using System;
using System.Linq;
using System.Threading.Tasks;
using Bloqs.Data.Commands;
using NUnit.Framework;

namespace Bloqs.Data
{
    [TestFixture]
    public class StorageDbCommandTest : DbCommandTestBase
    {
        private readonly StorageDbCommand _dbCommand = new StorageDbCommand(Constants.ConnectionString);

        [TestCase]
        public async Task CreateCommonTest()
        {
            var storage = Storage.NewStorage("CreateCommonTest", "Owner", StorageType.Common);
            storage.ConnectionProperties = new ConnectionProperties(@"(localdb)\v11.0", "BloqsTest_CreateCommon", "sa", "password");
            storage.IsSuspended = true;
            storage.ThresholdLength = 20000;

            await _dbCommand.CreateAsync(storage);
            var created = await _dbCommand.FindAsync(storage.Id);

            created.IsStructuralEqual(storage);
        }

        [TestCase]
        public async Task CreatePersonalTest()
        {
            var storage = Storage.NewStorage("CreatePersonalTest", "Owner", StorageType.Personal);
            storage.ConnectionProperties = new ConnectionProperties(@"(localdb)\v11.0", "BloqsTest_CreatePersonal", "sa", "password");
            storage.IsSuspended = true;
            storage.ThresholdLength = 20000;

            await _dbCommand.CreateAsync(storage);
            var created = await _dbCommand.FindAsync(storage.Id);

            created.IsStructuralEqual(storage);
        }

        [TestCase]
        public async Task UpdateCommonTest()
        {
            var storage = Storage.NewStorage("UpdateCommonTest", "Owner", StorageType.Common);
            storage.ConnectionProperties = new ConnectionProperties(@"(localdb)\v11.0", "BloqsTest_UpdateCommon", "sa", "password");
            storage.IsSuspended = true;
            storage.ThresholdLength = 20000;

            await _dbCommand.CreateAsync(storage);
            var created = await _dbCommand.FindAsync(storage.Id);

            created.Name = "UpdatedCommonTest";
            created.Owner = "Owner2";
            storage.ConnectionProperties = new ConnectionProperties(@"(localdb)\v12.0", "BloqsTest_UpdatedCommon", "user", "password2");
            created.IsSuspended = false;
            created.ThresholdLength = 30000;
            created.LastModifiedUtcDateTime = DateTime.UtcNow;

            await _dbCommand.UpdateAsync(created.Id, created);
            var updated = await _dbCommand.FindAsync(created.Id);

            updated.IsStructuralEqual(created);
        }

        [TestCase]
        public async Task UpdatePersonalTest()
        {
            var storage = Storage.NewStorage("UpdatePersonalTest", "Owner", StorageType.Personal);
            storage.ConnectionProperties = new ConnectionProperties(@"(localdb)\v11.0", "BloqsTest_UpdatePersonal", "sa", "password");
            storage.IsSuspended = true;
            storage.ThresholdLength = 20000;

            await _dbCommand.CreateAsync(storage);
            var created = await _dbCommand.FindAsync(storage.Id);

            created.Name = "UpdatedPersonalTest";
            created.Owner = "Owner2";
            storage.ConnectionProperties = new ConnectionProperties(@"(localdb)\v12.0", "BloqsTest_UpdatedPersonal", "user", "password2");
            created.IsSuspended = false;
            created.ThresholdLength = 30000;
            created.LastModifiedUtcDateTime = DateTime.UtcNow;

            await _dbCommand.UpdateAsync(created.Id, created);
            var updated = await _dbCommand.FindAsync(created.Id);

            updated.IsStructuralEqual(created);
        }

        [TestCase]
        public async Task DeleteTest()
        {
            var storage = Storage.NewStorage("DeleteTest", "Owner", StorageType.Personal);
            storage.ConnectionProperties = new ConnectionProperties(@"server", "database", "user", "password");
            storage.IsSuspended = true;
            storage.ThresholdLength = 20000;

            await _dbCommand.CreateAsync(storage);

            await _dbCommand.DeleteAsync(storage.Id);

            var deleted = await _dbCommand.FindAsync(storage.Id);

            deleted.IsNull();
        }

        [TestCase]
        public async Task GetListByOwnerTest()
        {
            var storage1 = Storage.NewStorage("Storage1", "Owner1", StorageType.Personal);
            storage1.ConnectionProperties = new ConnectionProperties(@"server", "database", "user", "password");
            storage1.IsSuspended = true;
            storage1.ThresholdLength = 20000;

            var storage2 = Storage.NewStorage("Storage2", "Owner1", StorageType.Personal);
            storage2.ConnectionProperties = new ConnectionProperties(@"server", "database", "user", "password");
            storage2.IsSuspended = true;
            storage2.ThresholdLength = 20000;

            var storage3 = Storage.NewStorage("Storage3", "Owner2", StorageType.Personal);
            storage3.ConnectionProperties = new ConnectionProperties(@"server", "database", "user", "password");
            storage3.IsSuspended = true;
            storage3.ThresholdLength = 20000;

            var storage4 = Storage.NewStorage("Storage4", "Owner1", StorageType.Personal);
            storage4.ConnectionProperties = new ConnectionProperties(@"server", "database", "user", "password");
            storage4.IsSuspended = true;
            storage4.ThresholdLength = 20000;

            await _dbCommand.CreateAsync(storage1);
            await _dbCommand.CreateAsync(storage2);
            await _dbCommand.CreateAsync(storage3);
            await _dbCommand.CreateAsync(storage4);

            var list = (await _dbCommand.GetListByOwnerAsync("Owner1", 1, 2)).ToArray();

            list.IsStructuralEqual(new[] { storage2, storage4 });
        }
    }
}
