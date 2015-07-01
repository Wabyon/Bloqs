using Bloqs.Data.Commands;

namespace Bloqs.Data
{
    public class Mock
    {
        public static readonly Account PersonalStorageAccount;
        public static readonly Container PersonalStorageContainer;
        public static readonly Account CommonStorageAccount;
        public static readonly Container CommonStorageContainer;

        static Mock()
        {
            PersonalStorageAccount = Account.NewAccount("_PersonalStorageAccount", "Owner", StorageType.Personal);
            PersonalStorageAccount.Storages.Add(MockStorages.Personal1);
            PersonalStorageAccount.Storages.Add(MockStorages.Personal2);

            PersonalStorageContainer = PersonalStorageAccount.CreateContainer("_PersonalStorageContainer");

            CommonStorageAccount = Account.NewAccount("_CommonStorageAccount", "Owner", StorageType.Common);

            CommonStorageContainer = CommonStorageAccount.CreateContainer("_CommonStorageContainer");
        }

        public static void RegisterDadatabase()
        {
            var accountDbCommand = new AccountDbCommand(Constants.ConnectionString);
            var containerDbCommand = new ContainerDbCommand(Constants.ConnectionString);

            accountDbCommand.CreateAsync(PersonalStorageAccount).Wait();
            accountDbCommand.CreateAsync(CommonStorageAccount).Wait();

            containerDbCommand.SaveAsync(PersonalStorageContainer).Wait();
            containerDbCommand.SaveAsync(CommonStorageContainer).Wait();
        }
    }
}
