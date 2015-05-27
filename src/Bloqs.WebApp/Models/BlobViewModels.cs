using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bloqs.Models
{
    public class BlobIndexModel
    {
        public string Id { get; set; }

        public string AccountId { get; set; }

        public string AccountName { get; set; }

        public string ContainerId { get; set; }

        public string ContainerName { get; set; }

        [DisplayName("名前")]
        public string Name { get; set; }

        [DisplayName("キャッシュ制御")]
        public string CacheControl { get; set; }

        [DisplayName("Content-Disposition")]
        public string ContentDisposition { get; set; }

        [DisplayName("Content-Encoding")]
        public string ContentEncoding { get; set; }

        [DisplayName("Content-Language")]
        public string ContentLanguage { get; set; }

        [DisplayName("Content-MD5")]
        public string ContentMD5 { get; set; }

        [DisplayName("Content-Type")]
        public string ContentType { get; set; }

        [DisplayName("ETag")]
        public string ETag { get; set; }

        [DisplayName("作成時刻 (UTC)")]
        public DateTime CreatedUtcDateTime { get; set; }

        [DisplayName("最終変更時刻 (UTC)")]
        public DateTime LastModifiedUtcDateTime { get; set; }

        [DisplayName("サイズ")]
        public Size Size { get; set; }

        [DisplayName(@"URL")]
        public string BlobApiAddress { get; set; }

        [DisplayName(@"メタデータ")]
        public Metadata Metadata { get; set; }
    }

    public class BlobEditModel
    {
        private readonly Metadata _metadata = new Metadata();

        public string Id { get; set; }

        public string AccountId { get; set; }

        public string AccountName { get; set; }

        public string ContainerId { get; set; }

        public string ContainerName { get; set; }

        [DisplayName("名前")]
        public string Name { get; set; }

        [DisplayName("キャッシュ制御")]
        public string CacheControl { get; set; }

        [DisplayName("Content-Disposition")]
        public string ContentDisposition { get; set; }

        [DisplayName("Content-Encoding")]
        public string ContentEncoding { get; set; }

        [DisplayName("Content-Language")]
        public string ContentLanguage { get; set; }

        [DisplayName("Content-MD5")]
        public string ContentMD5 { get; set; }

        [DisplayName("Content-Type")]
        [RegularExpression(@"[a-zA-Z0-9 -/:-\[-\`\{-\~]+", ErrorMessage = "Is only half-width alphanumeric characters and symbols can be used")]
        public string ContentType { get; set; }

        [DisplayName("ETag")]
        public string ETag { get; set; }

        [DisplayName("最終変更時刻 (UTC)")]
        public DateTime LastModifiedUtcDateTime { get; set; }

        [DisplayName("サイズ")]
        public Size Size { get; set; }

        [DisplayName("メタデータ")]
        public Metadata Metadata { get { return _metadata; } }
    }

    public class BlobDeleteModel
    {
        private readonly Metadata _metadata = new Metadata();

        public string Id { get; set; }

        public string AccountId { get; set; }

        public string AccountName { get; set; }

        public string ContainerId { get; set; }

        public string ContainerName { get; set; }

        [DisplayName("名前")]
        public string Name { get; set; }

        [DisplayName("キャッシュ制御")]
        public string CacheControl { get; set; }

        [DisplayName("Content-Disposition")]
        public string ContentDisposition { get; set; }

        [DisplayName("Content-Encoding")]
        public string ContentEncoding { get; set; }

        [DisplayName("Content-Language")]
        public string ContentLanguage { get; set; }

        [DisplayName("Content-MD5")]
        public string ContentMD5 { get; set; }

        [DisplayName("Content-Type")]
        public string ContentType { get; set; }

        [DisplayName("ETag")]
        public string ETag { get; set; }

        [DisplayName("最終変更時刻 (UTC)")]
        public DateTime LastModifiedUtcDateTime { get; set; }

        [DisplayName("サイズ")]
        public Size Size { get; set; }

        [DisplayName("メタデータ")]
        public Metadata Metadata
        {
            get { return _metadata; }
        }
    }
}