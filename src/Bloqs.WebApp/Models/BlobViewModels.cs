using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bloqs.Models
{
    public class BlobViewModel
    {
        private readonly IDictionary<string, string> _metadata;

        [DisplayName("Blob Name")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z0-9 -/:-\[-\`\{-\~]+", ErrorMessage = "Is only half-width alphanumeric characters and symbols can be used")]
        [DisplayName("Content Type")]
        public string ContentType { get; set; }

        public long Size { get; set; }

        [DisplayName("Last Updated Date (UTC)")]
        public DateTime LastModifiedUtcDateTime { get; set; }

        [DisplayName("Metadata")]
        public IDictionary<string, string> Metadata
        {
            get { return _metadata; }
        }

        [DisplayName("Size")]
        public string SizeString
        {
            get { return FormatSizeString(Size); }
        }

        public BlobViewModel() : this(new BlobAttribute())
        {
        }

        public BlobViewModel(BlobAttribute blob)
        {
            Name = blob.Name;
            ContentType = blob.Properties.ContentType;
            LastModifiedUtcDateTime = blob.Properties.LastModifiedUtcDateTime;
            Size = blob.Properties.Length;
            _metadata = blob.Metadata;
        }

        internal static string FormatSizeString(long size)
        {
            string[] suffix = { "", "K", "M", "G", "T", "P", "E", "Z", "Y" };
            var index = 0;

            while (size >= 1024)
            {
                size /= 1024;
                index++;
            }

            return string.Format(
                "{0} {1}B",
                size.ToString("N1"),
                index < suffix.Length ? suffix[index] : "-");
        }
    }

    public class BlobEditModel
    {
        private readonly IDictionary<string, string> _metadata;

        public string Name { get; set; }

        [ReadOnly(true)]
        public string Url { get; set; }

        public string CacheControl { get; set; }

        public string ContentDisposition { get; set; }

        public string ContentEncoding { get; set; }

        public string ContentLanguage { get; set; }

        public string ContentMD5 { get; set; }

        public string ContentType { get; set; }

        public string ETag { get; set; }

        [DisplayName("Last Updated Date (UTC)")]
        public DateTime LastModifiedUtcDateTime { get; set; }

        public long Length { get; set; }

        [ReadOnly(true)]
        public string Size { get { return BlobViewModel.FormatSizeString(Length); } }

        [DisplayName("Metadata")]
        public IDictionary<string, string> Metadata
        {
            get { return _metadata; }
        }

        public BlobEditModel()
        {
            _metadata = new Dictionary<string, string>();
        }

        public BlobEditModel(BlobAttribute blob)
        {
            Name = blob.Name;
            CacheControl = blob.Properties.CacheControl;
            ContentDisposition = blob.Properties.ContentDisposition;
            ContentEncoding = blob.Properties.ContentEncoding;
            ContentLanguage = blob.Properties.ContentLanguage;
            ContentMD5 = blob.Properties.ContentMD5;
            ContentType = blob.Properties.ContentType;
            ETag = blob.Properties.ETag;
            LastModifiedUtcDateTime = blob.Properties.LastModifiedUtcDateTime;
            Length = blob.Properties.Length;
            _metadata = blob.Metadata;
        }

        public BlobAttribute ToBlobAttribute()
        {
            var properties = new BlobProperties
            {
                CacheControl = CacheControl,
                ContentDisposition = ContentDisposition,
                ContentEncoding = ContentEncoding,
                ContentLanguage = ContentLanguage,
                ContentType = ContentType,
                ContentMD5 = ContentMD5,
                ETag = ETag,
                LastModifiedUtcDateTime = LastModifiedUtcDateTime,
            };

            var attributes = new BlobAttribute(properties)
            {
                Name = Name,
            };

            foreach (var metadata in Metadata)
            {
                attributes.Metadata.Add(metadata);
            }
            return attributes;
        }
    }
}