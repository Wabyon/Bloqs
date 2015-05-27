using System;
using Bloqs.Data;

namespace Bloqs
{
    /// <summary></summary>
    public class Storage
    {
        /// <summary></summary>
        public string Id { get; set; }

        /// <summary></summary>
        public string Owner { get; set; }

        /// <summary></summary>
        public string Name { get; set; }

        /// <summary></summary>
        public StorageType StorageType { get; set; }

        /// <summary></summary>
        public ConnectionProperties ConnectionProperties { get; set; }

        /// <summary></summary>
        public long ThresholdLength { get; set; }

        /// <summary></summary>
        public long UsedLength { get; set; }

        /// <summary></summary>
        public bool IsSuspended { get; set; }

        /// <summary></summary>
        public DateTime CreatedUtcDateTime { get; set; }

        /// <summary></summary>
        public DateTime LastModifiedUtcDateTime { get; set; }

        /// <summary></summary>
        public static readonly Storage None = new Storage { Id = null };

        /// <summary></summary>
        internal Storage()
        {
            ConnectionProperties = new ConnectionProperties();
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="storageType"></param>
        /// <returns></returns>
        public static Storage NewStorage(string name, string owner, StorageType storageType)
        {
            var storage = new Storage
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = name,
                Owner = owner,
                StorageType = storageType,
                CreatedUtcDateTime = DateTime.UtcNow,
                LastModifiedUtcDateTime = DateTime.UtcNow,
            };

            return storage;
        }
    }
}
