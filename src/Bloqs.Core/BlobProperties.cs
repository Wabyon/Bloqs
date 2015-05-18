using System;

namespace Bloqs
{
    public class BlobProperties
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
    }
}