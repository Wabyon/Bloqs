using System;
using System.Linq;
using System.Threading.Tasks;
using Bloqs.Data.Commands;
using NUnit.Framework;

namespace Bloqs.Data
{
    [TestFixture]
    public class AccountDbCommandTest : DbCommandTestBase
    {
        private readonly AccountDbCommand _dbCommand = new AccountDbCommand(Constants.ConnectionString);

        [TestCase]
        public async Task CreateTest()
        {
            var account = Account.NewAccount("Create Test", "Owner", StorageType.Common);

            await _dbCommand.CreateAsync(account);
            var registered = await _dbCommand.FindAsync(account.Id);
            registered.IsStructuralEqual(account);
        }

        [TestCase]
        public async Task CreateWithUsingStorageTest()
        {
            var account = Account.NewAccount("CreateWithUsingStorageTest", "Owner", StorageType.Personal);
            account.Storages.Add(MockStorages.Personal1);

            await _dbCommand.CreateAsync(account);
            var registered = await _dbCommand.FindAsync(account.Id);
            registered.IsStructuralEqual(account);
        }

        [TestCase]
        public async Task CreateWithUsingMultipleStoragesTest()
        {
            var account = Account.NewAccount("CreateWithUsingStorageTest", "Owner", StorageType.Personal);
            
            account.Storages.Add(MockStorages.Personal1);
            account.Storages.Add(MockStorages.Personal2);

            await _dbCommand.CreateAsync(account);
            var registered = await _dbCommand.FindAsync(account.Id);
            registered.IsStructuralEqual(account);
        }

        [TestCase]
        public async Task UpdateTest()
        {
            var account = Account.NewAccount("Update Test", "Owner", StorageType.Common);
            await _dbCommand.CreateAsync(account);
            var target = await _dbCommand.FindAsync(account.Id);
            target.Name = "UpdatedTestRename";
            target.CreatedUtcDateTime = new DateTime(2001,12,31);
            target.LastModifiedUtcDateTime = new DateTime(2002,1,1);
            target.PrimaryAccessKey = Account.CreateNewAccessKeyString();
            target.SecondaryAccessKey = Account.CreateNewAccessKeyString();
            target.Owner = "UpdateTest";
            target.StorageType = StorageType.Personal;

            await _dbCommand.UpdateAsync(target.Id, target);
            var updated = await _dbCommand.FindAsync(target.Id);
            updated.IsStructuralEqual(target);
        }

        [TestCase]
        public async Task UpdateWithUsingStorageTest()
        {
            var account = Account.NewAccount("UpdateWithUsingStorageTest", "Owner", StorageType.Personal);

            account.Storages.Add(MockStorages.Personal1);
            account.Storages.Add(MockStorages.Personal2);

            await _dbCommand.CreateAsync(account);
            var target = await _dbCommand.FindAsync(account.Id);
            target.Name = "UpdateWithUsingStorageTestRename";
            target.CreatedUtcDateTime = new DateTime(2001, 12, 31);
            target.LastModifiedUtcDateTime = new DateTime(2002, 1, 1);
            target.PrimaryAccessKey = Account.CreateNewAccessKeyString();
            target.SecondaryAccessKey = Account.CreateNewAccessKeyString();
            target.Owner = "UpdateTest";
            target.StorageType = StorageType.Personal;
            target.Storages.Clear();

            target.Storages.Add(MockStorages.Personal2);
            target.Storages.Add(MockStorages.Personal3);

            await _dbCommand.UpdateAsync(target.Id, target);
            var updated = await _dbCommand.FindAsync(target.Id);
            updated.IsStructuralEqual(target);
        }

        [TestCase]
        public async Task FindByNameTest()
        {
            var account = Account.NewAccount("FindByNameTest", "Owner", StorageType.Common);
            await _dbCommand.CreateAsync(account);
            var found = await _dbCommand.FindByNameAsync(account.Name);
            found.IsStructuralEqual(account);
        }

        [TestCase]
        public async Task FindByNameWithUsingStorageTest()
        {
            var account = Account.NewAccount("FindByNameWithUsingStorageTest", "Owner", StorageType.Common);

            account.Storages.Add(MockStorages.Personal2);
            account.Storages.Add(MockStorages.Personal3);

            await _dbCommand.CreateAsync(account);
            var found = await _dbCommand.FindByNameAsync(account.Name);
            found.IsStructuralEqual(account);
        }

        [TestCase]
        public async Task DeleteTest()
        {
            var account = Account.NewAccount("DeleteTest", "Owner", StorageType.Common);
            await _dbCommand.CreateAsync(account);
            var found = await _dbCommand.FindAsync(account.Id);
            Assert.IsNotNull(found);
            await _dbCommand.DeleteAsync(account.Id);
            found = await _dbCommand.FindAsync(account.Id);
            Assert.IsNull(found);
        }

        [TestCase(@"森鷗外")]
        [TestCase("% %")]
        [TestCase("_a_")]
        [TestCase("AAA")]
        [TestCase("%")]
        [TestCase("_")]
        [TestCase("name")]
        public async Task ExistsTest(string name)
        {
            var account = Account.NewAccount(name, "Owner", StorageType.Common);
            await _dbCommand.CreateAsync(account);

            var exists = await _dbCommand.ExistsAsync(name);
            exists.IsTrue();

            exists = await _dbCommand.ExistsAsync("_" + name);
            exists.IsFalse();
        }

        [TestCase(@"森鷗外")]
        [TestCase("% %")]
        [TestCase("_a_")]
        [TestCase("AAA")]
        [TestCase("%")]
        [TestCase("_")]
        [TestCase("name")]
        public async Task CountByOwnerTest(string ownerName)
        {
            var account1 = Account.NewAccount("CountByOwner1", ownerName, StorageType.Common);
            var account2 = Account.NewAccount("CountByOwner2", ownerName, StorageType.Common);
            var account3 = Account.NewAccount("CountByOwner3", "_" + ownerName, StorageType.Common);
            var account4 = Account.NewAccount("CountByOwner4", ownerName, StorageType.Common);

            await _dbCommand.CreateAsync(account1);
            await _dbCommand.CreateAsync(account2);
            await _dbCommand.CreateAsync(account3);
            await _dbCommand.CreateAsync(account4);

            var count = await _dbCommand.CountByOwnerAsync(ownerName);

            count.Is(3);
        }

        [TestCase]
        public async Task GetListByOwnerTest()
        {
            var account1 = Account.NewAccount("GetListByOwner1", "ListTestOwner1-1", StorageType.Common);
            var account2 = Account.NewAccount("GetListByOwner2", "ListTestOwner1-1", StorageType.Common);
            var account3 = Account.NewAccount("GetListByOwner3", "ListTestOwner1-2", StorageType.Common);
            var account4 = Account.NewAccount("GetListByOwner4", "ListTestOwner1-1", StorageType.Common);

            await _dbCommand.CreateAsync(account1);
            await _dbCommand.CreateAsync(account2);
            await _dbCommand.CreateAsync(account3);
            await _dbCommand.CreateAsync(account4);

            var list = (await _dbCommand.GetListByOwnerAsync("ListTestOwner1-1", 1, 2)).ToArray();

            list.IsStructuralEqual(new []{account2, account4});
        }

        [TestCase]
        public async Task GetListByOwnerWithUsingStorageTest()
        {
            var account1 = Account.NewAccount("GetListByOwner1", "ListTestOwner2-1", StorageType.Personal);
            var account2 = Account.NewAccount("GetListByOwner2", "ListTestOwner2-1", StorageType.Personal);
            var account3 = Account.NewAccount("GetListByOwner3", "ListTestOwner2-2", StorageType.Personal);
            var account4 = Account.NewAccount("GetListByOwner4", "ListTestOwner2-1", StorageType.Personal);

            account1.Storages.Add(MockStorages.Personal1);
            account1.Storages.Add(MockStorages.Personal2);
            account2.Storages.Add(MockStorages.Personal1);
            account2.Storages.Add(MockStorages.Personal3);
            account3.Storages.Add(MockStorages.Personal1);
            account3.Storages.Add(MockStorages.Personal2);
            account3.Storages.Add(MockStorages.Personal3);
            account4.Storages.Add(MockStorages.Personal1);

            await _dbCommand.CreateAsync(account1);
            await _dbCommand.CreateAsync(account2);
            await _dbCommand.CreateAsync(account3);
            await _dbCommand.CreateAsync(account4);

            var list = (await _dbCommand.GetListByOwnerAsync("ListTestOwner2-1", 1, 2)).ToArray();

            list.IsStructuralEqual(new[] { account2, account4 });
        }
    }
}
