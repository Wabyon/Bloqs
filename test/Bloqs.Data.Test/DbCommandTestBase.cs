using NUnit.Framework;

namespace Bloqs.Data
{
    public abstract class DbCommandTestBase
    {
        [TestFixtureSetUp]
        public virtual void SetUp()
        {
            TestUtilities.InitializeDatabase();
        }

        [TestFixtureTearDown]
        public virtual void TearDown()
        {
            TestUtilities.TruncateAllTables(Constants.ConnectionString);
            foreach (var storage in MockStorages.Commons)
            {
                TestUtilities.TruncateAllTables(storage.ConnectionProperties.ToConnectionString());
            }
            foreach (var storage in MockStorages.Personals)
            {
                TestUtilities.TruncateAllTables(storage.ConnectionProperties.ToConnectionString());
            }
        }
    }
}
