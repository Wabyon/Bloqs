namespace Bloqs.Data
{
    public class MockStorages
    {
        public static readonly TestStorageCollection Commons = new TestStorageCollection();
        public static readonly TestStorageCollection Personals = new TestStorageCollection();

        public static Storage Common1 { get; private set; }
        public static Storage Common2 { get; private set; }
        public static Storage Common3 { get; private set; }

        public static Storage Personal1 { get; private set; }
        public static Storage Personal2 { get; private set; }
        public static Storage Personal3 { get; private set; }

        static MockStorages()
        {
            Common1 = Storage.NewStorage("Common1", "Owner", StorageType.Common);
            Common1.ConnectionProperties = new ConnectionProperties(@"(localdb)\v11.0", "BloqsTest_Common_1", "sa", "password");
            Common1.ThresholdLength = 10000000;

            Common2 = Storage.NewStorage("Common2", "Owner", StorageType.Common);
            Common2.ConnectionProperties = new ConnectionProperties(@"(localdb)\v11.0", "BloqsTest_Common_2", "sa", "password");
            Common2.ThresholdLength = 20000000;

            Common3 = Storage.NewStorage("Common3", "Owner", StorageType.Common);
            Common3.ConnectionProperties = new ConnectionProperties(@"(localdb)\v11.0", "BloqsTest_Common_3", "sa", "password");
            Common3.ThresholdLength = 30000000;

            Personal1 = Storage.NewStorage("Personal1", "Owner", StorageType.Personal);
            Personal1.ConnectionProperties = new ConnectionProperties(@"(localdb)\v11.0", "BloqsTest_Personal_1", "sa", "password");
            Personal1.ThresholdLength = 10000000;

            Personal2 = Storage.NewStorage("Personal2", "Owner", StorageType.Personal);
            Personal2.ConnectionProperties = new ConnectionProperties(@"(localdb)\v11.0", "BloqsTest_Personal_2", "sa", "password");
            Personal2.ThresholdLength = 20000000;

            Personal3 = Storage.NewStorage("Personal3", "Owner", StorageType.Personal);
            Personal3.ConnectionProperties = new ConnectionProperties(@"(localdb)\v11.0", "BloqsTest_Personal_3", "sa", "password");
            Personal3.ThresholdLength = 30000000;

            Commons.Add(Common1);
            Commons.Add(Common2);
            Commons.Add(Common3);

            Personals.Add(Personal1);
            Personals.Add(Personal2);
            Personals.Add(Personal3);
        }

        public static void Initialize()
        {
            Commons.InitializeDatabases();
            Personals.InitializeDatabases();
        }
    }
}
