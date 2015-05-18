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

        public string ETag { get; internal set; }

        public DateTime LastModifiedUtcDateTime { get; internal set; }

        public long Length { get; internal set; }
    }
}