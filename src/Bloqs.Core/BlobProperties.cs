using System;
using System.Security.Cryptography;
using System.Text;

namespace Bloqs
{
    /// <summary></summary>
    public class BlobProperties
    {
        private string _etag;

        /// <summary></summary>
        public string CacheControl { get; set; }

        /// <summary></summary>
        public string ContentDisposition { get; set; }

        /// <summary></summary>
        public string ContentEncoding { get; set; }

        /// <summary></summary>
        public string ContentLanguage { get; set; }

        /// <summary></summary>
        public string ContentMD5 { get; set; }

        /// <summary></summary>
        public string ContentType { get; set; }

        /// <summary></summary>
        public string ETag
        {
            get { return _etag ?? (_etag = CreateETag(LastModifiedUtcDateTime.ToString("O"))); }
            set { _etag = value; }
        }

        /// <summary></summary>
        public DateTime CreatedUtcDateTime { get; set; }

        /// <summary></summary>
        public DateTime LastModifiedUtcDateTime { get; set; }

        /// <summary></summary>
        public long Length { get; set; }

        private static string CreateETag(string s)
        {
            var num = BitConverter.ToInt64(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(s + Guid.NewGuid())), 0);

            return string.Format("0x{0}", num.ToString("X"));
        }
    }
}