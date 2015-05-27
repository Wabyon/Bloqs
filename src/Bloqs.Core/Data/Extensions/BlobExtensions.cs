using Bloqs.Data.Models;

namespace Bloqs.Data.Extensions
{
    internal static class BlobExtensions
    {
        public static BlobAttributesDataModel ToAttributesDataModel(this BlobAttributes blob)
        {
            return new BlobAttributesDataModel
            {
                Id = blob.Id,
                ContainerId = blob.Container.Id,
                Name = blob.Name,
                CacheControl = blob.Properties.CacheControl,
                ContentDisposition = blob.Properties.ContentDisposition,
                ContentEncoding = blob.Properties.ContentEncoding,
                ContentLanguage = blob.Properties.ContentLanguage,
                ContentMD5 = blob.Properties.ContentMD5,
                ContentType = blob.Properties.ContentType,
                ETag = blob.Properties.ETag,
                Length = blob.Properties.Length,
                Metadata = blob.Metadata.ToJson(),
                CreatedUtcDateTime = blob.Properties.CreatedUtcDateTime,
                LastModifiedUtcDateTime = blob.Properties.LastModifiedUtcDateTime,
                StorageId = blob.Storage.Id,
            };
        }
    }
}