using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bloqs
{
    /// <summary></summary>
    public class Account
    {
        private readonly List<Storage> _storages = new List<Storage>();

        /// <summary></summary>
        public string Id { get; internal set; }

        /// <summary></summary>
        public string Name { get; internal set; }

        /// <summary></summary>
        public string Owner { get; set; }

        /// <summary></summary>
        public string PrimaryAccessKey { get; set; }

        /// <summary></summary>
        public string SecondaryAccessKey { get; set; }

        /// <summary></summary>
        public string CryptKey { get; set; }

        /// <summary></summary>
        public DateTime CreatedUtcDateTime { get; set; }

        /// <summary></summary>
        public DateTime LastModifiedUtcDateTime { get; set; }

        /// <summary></summary>
        public StorageType StorageType { get; internal set; }

        /// <summary></summary>
        public ICollection<Storage> Storages { get { return _storages; } }

        /// <summary></summary>
        internal Account()
        {
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="storageType"></param>
        /// <returns></returns>
        public static Account NewAccount(string name, string owner, StorageType storageType)
        {
            return new Account
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = name,
                Owner = owner,
                PrimaryAccessKey = CreateNewAccessKeyString(),
                SecondaryAccessKey = CreateNewAccessKeyString(),
                CreatedUtcDateTime = DateTime.UtcNow,
                LastModifiedUtcDateTime = DateTime.UtcNow,
                StorageType = storageType,
                CryptKey = GeneratePassword(16),
            };
        }

        /// <summary></summary>
        public static string CreateNewAccessKeyString()
        {
            return
                string.Concat(
                    SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()))
                        .Select(x => string.Format("{0:x2}", x)));
        }

        private static string GeneratePassword(int length)
        {
            const string passwordChars = @"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_-+@\/";
            var sb = new StringBuilder(length);
            var r = new Random();

            for (var i = 0; i < length; i++)
            {
                var pos = r.Next(passwordChars.Length);
                var c = passwordChars[pos];
                sb.Append(c);
            }

            return sb.ToString();
        }

        public Container CreateContainer(string name)
        {
            return new Container(this)
            {
                Name = name,
                CreatedUtcDateTime = DateTime.UtcNow,
                LastModifiedUtcDateTime = DateTime.UtcNow,
            };
        }

        public static string[] ReservedNames =
        {
            "account", "accounts", "admin", "admins", "administrator", "administrators", "attribute", "attributes",
            "bloqs", "blob", "blobs", "blobattribute", "blobattributes",
            "container", "containers", "control", "controls",
            "storage", "storages",
        };

        public static bool IsReservedName(string name)
        {
            return ReservedNames.Contains(name.ToLower());
        }
    }
}
