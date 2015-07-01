using System;
using System.Threading.Tasks;
using Bloqs.Data.Commands;
using NUnit.Framework;

namespace Bloqs.Data
{
    [TestFixture]
    public class ContainerDbCommandTest : DbCommandTestBase
    {
        private readonly AccountDbCommand _accountDbCommand = new AccountDbCommand(Constants.ConnectionString);
        private readonly ContainerDbCommand _dbCommand = new ContainerDbCommand(Constants.ConnectionString);

        [TestCase]
        public async Task CreateCommonTest()
        {
            var container = Mock.CommonStorageAccount.CreateContainer("CreateCommonTest");
            container.Metadata["key1"] = "value1";
            container.Metadata["key2"] = "value2";
            container.IsPublic = true;
            container.LastModifiedUtcDateTime = new DateTime(2015,12,31);

            await _dbCommand.SaveAsync(container);
            var created = await _dbCommand.FindAsync(container.Id);
            created.IsStructuralEqual(container);
        }

        [TestCase]
        public async Task CreatePersonalTest()
        {
            var container = Mock.PersonalStorageAccount.CreateContainer("CreatePersonalTest");
            container.Metadata["key1"] = "value1";
            container.Metadata["key2"] = "value2";
            container.IsPublic = true;
            container.LastModifiedUtcDateTime = new DateTime(2015, 12, 31);

            await _dbCommand.SaveAsync(container);
            var created = await _dbCommand.FindAsync(container.Id);
            created.IsStructuralEqual(container);
        }

        [TestCase]
        public async Task UpdateTest()
        {
            var container = Mock.CommonStorageAccount.CreateContainer("UpdatTest");
            container.Metadata["key1"] = "value1";
            container.Metadata["key2"] = "value2";
            container.IsPublic = true;
            container.LastModifiedUtcDateTime = new DateTime(2015, 12, 31);

            await _dbCommand.SaveAsync(container);
            var created = await _dbCommand.FindAsync(container.Id);

            created.CreatedUtcDateTime = new DateTime(2000,1,1);
            created.LastModifiedUtcDateTime = new DateTime(2020,12,31);
            created.IsPublic = false;
            created.Metadata["key2"] = "value222";
            created.Metadata["key3"] = "value3";
            created.Metadata["key4"] = "value4";
            created.Metadata.Remove("key1");

            await _dbCommand.SaveAsync(created);
            var updated = await _dbCommand.FindAsync(created.Id);

            updated.IsStructuralEqual(created);
        }

        [TestCase]
        public async Task DeleteTest()
        {
            var container = Mock.CommonStorageAccount.CreateContainer("DeleteTest");
            container.Metadata["key1"] = "value1";
            container.Metadata["key2"] = "value2";
            container.IsPublic = true;
            container.LastModifiedUtcDateTime = new DateTime(2015, 12, 31);

            await _dbCommand.SaveAsync(container);

            var exists = await _dbCommand.ExistsAsync(Mock.CommonStorageAccount, container.Name);
            exists.IsTrue();

            await _dbCommand.DeleteAsync(container.Id);
            exists = await _dbCommand.ExistsAsync(Mock.CommonStorageAccount, container.Name);
            exists.IsFalse();
        }

        [TestCase(@"森鷗外")]
        [TestCase("% %")]
        [TestCase("_a_")]
        [TestCase("AAA")]
        [TestCase("%")]
        [TestCase("_")]
        [TestCase("name")]
        public async Task FindByNameTest(string name)
        {
            var container = Mock.CommonStorageAccount.CreateContainer(name);
            container.Metadata["key1"] = "value1";
            container.Metadata["key2"] = "value2";
            container.IsPublic = true;
            container.LastModifiedUtcDateTime = new DateTime(2015, 12, 31);

            await _dbCommand.SaveAsync(container);

            var created = await _dbCommand.FindAsync(Mock.CommonStorageAccount, container.Name);
            created.IsStructuralEqual(container);
        }
    }
}
