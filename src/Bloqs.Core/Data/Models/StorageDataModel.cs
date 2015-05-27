using System;
using System.Security.Cryptography;
using System.Text;

namespace Bloqs.Data.Models
{
    internal class StorageDataModel
    {
        public string Id { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public StorageType StorageType { get; set; }
        public string ConnectionString { get; set; }
        public long ThresholdLength { get; set; }
        public bool IsSuspended { get; set; }
        public long UsedLength { get; set; }
        public DateTime CreatedUtcDateTime { get; set; }
        public DateTime LastModifiedUtcDateTime { get; set; }

        public StorageDataModel()
        {
        }

        public StorageDataModel(Storage storage)
        {
            if (storage == null) throw new ArgumentNullException("storage");

            Id = storage.Id;
            Owner = storage.Owner;
            Name = storage.Name;
            StorageType = storage.StorageType;
            ConnectionString = Encrypt(storage.ConnectionProperties.ToConnectionString(), storage.Id);
            ThresholdLength = storage.ThresholdLength;
            IsSuspended = storage.IsSuspended;
            UsedLength = UsedLength;
            CreatedUtcDateTime = storage.CreatedUtcDateTime;
            LastModifiedUtcDateTime = storage.LastModifiedUtcDateTime;
        }

        public Storage ToStorage()
        {
            var connecitonString = Decrypt(ConnectionString, Id);

            return new Storage
            {
                Id = Id,
                Name = Name,
                Owner = Owner,
                ConnectionProperties = new ConnectionProperties(connecitonString),
                StorageType = StorageType,
                ThresholdLength = ThresholdLength,
                IsSuspended = IsSuspended,
                UsedLength = UsedLength,
                CreatedUtcDateTime = CreatedUtcDateTime,
                LastModifiedUtcDateTime = LastModifiedUtcDateTime,
            };
        }

        private static string Encrypt(string s, string password)
        {
            using (var rijndael = new RijndaelManaged())
            {
                byte[] key, iv;
                GenerateKeyFromPassword(password, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
                rijndael.Key = key;
                rijndael.IV = iv;

                var strBytes = Encoding.UTF8.GetBytes(s);

                using (var encryptor = rijndael.CreateEncryptor())
                {
                    var encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
                    return Convert.ToBase64String(encBytes);
                }
            }
        }

        private static string Decrypt(string s, string password)
        {
            using (var rijndael = new RijndaelManaged())
            {
                byte[] key, iv;
                GenerateKeyFromPassword(password, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
                rijndael.Key = key;
                rijndael.IV = iv;

                var strBytes = Convert.FromBase64String(s);

                using (var decryptor = rijndael.CreateDecryptor())
                {
                    var decBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
                    return Encoding.UTF8.GetString(decBytes);
                }
            }
        }

        private static void GenerateKeyFromPassword(string password, int keySize, out byte[] key, int blockSize,
            out byte[] iv)
        {
            var salt = Encoding.UTF8.GetBytes(@"R\PT;4K8jY4YUdXxEbkj");
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt) { IterationCount = 1000 })
            {
                key = deriveBytes.GetBytes(keySize / 8);
                iv = deriveBytes.GetBytes(blockSize / 8);
            }
        }
    }
}
