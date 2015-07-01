using System;
using Newtonsoft.Json;

namespace Bloqs.Data.Models
{
    internal class BlobAttributesDataModel
    {
        public string Id { get; set; }
        public string ContainerId { get; set; }
        public string Name { get; set; }
        public string CacheControl { get; set; }
        public string ContentDisposition { get; set; }
        public string ContentEncoding { get; set; }
        public string ContentLanguage { get; set; }
        public string ContentMD5 { get; set; }
        public string ContentType { get; set; }
        public string ETag { get; set; }
        public long Length { get; set; }
        public string Metadata { get; set; }
        public DateTime CreatedUtcDateTime { get; set; }
        public DateTime LastModifiedUtcDateTime { get; set; }
        public string StorageId { get; set; }
        public string StorageOwner { get; set; }
        public string StorageName { get; set; }
        public StorageType StorageType { get; set; }
        public string ConnectionString { get; set; }
        public long ThresholdLength { get; set; }
        public bool IsSuspended { get; set; }

        public BlobAttributes ToAttributes(Container container)
        {
            Storage storage;

            if (StorageId != null)
            {
                storage = new StorageDataModel
                {
                    Id = StorageId,
                    Owner = StorageOwner,
                    Name = StorageName,
                    ConnectionString = ConnectionString,
                    StorageType = StorageType,
                    ThresholdLength = ThresholdLength,
                    IsSuspended = IsSuspended,
                }.ToStorage();
            }
            else
            {
                storage = Storage.None;
            }

            var ret = new BlobAttributes
            {
                Id = Id,
                Container = container,
                Name = Name,
                Properties =
                {
                    CacheControl = CacheControl,
                    ContentDisposition = ContentDisposition,
                    ContentEncoding = ContentEncoding,
                    CreatedUtcDateTime = CreatedUtcDateTime,
                    ContentLanguage = ContentLanguage,
                    ContentMD5 = ContentMD5,
                    ContentType = ContentType,
                    ETag = ETag,
                    Length = Length,
                    LastModifiedUtcDateTime = LastModifiedUtcDateTime,
                },
                Storage = storage,
            };

            ret.Metadata.Add(JsonConvert.DeserializeObject<Metadata>(Metadata));

            return ret;
        }
    }
}