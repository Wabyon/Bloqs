using System;

namespace Bloqs.Http.Net
{
    public class BlobProperties
    {
        public string CacheControl { get; set; }

        public string ContentDisposition { get; set; }

        public string ContentEncoding { get; set; }

        public string ContentLanguage { get; set; }

        public string ContentMD5 { get; set; }

        public string ContentType { get; set; }

        public string ETag { get; private set; }

        public DateTime LastModifiedUtcDateTime { get; private set; }

        public long Length { get; private set; }

        internal class ResponseModel
        {
            public string CacheControl { get; set; }
            public string ContentDisposition { get; set; }
            public string ContentEncoding { get; set; }
            public string ContentLanguage { get; set; }
            public string ContentMD5 { get; set; }
            public string ContentType { get; set; }
            public string ETag { get; set; }
            public DateTime LastModifiedUtcDateTime { get; set; }
            public long Length { get; set; }

            public BlobProperties ToModel()
            {
                return new BlobProperties
                {
                    CacheControl = CacheControl,
                    ContentDisposition = ContentDisposition,
                    ContentEncoding = ContentEncoding,
                    ContentLanguage = ContentLanguage,
                    ContentMD5 = ContentMD5,
                    ContentType = ContentType,
                    ETag = ETag,
                    LastModifiedUtcDateTime = LastModifiedUtcDateTime,
                    Length = Length,
                };
            }
        }
    }
}