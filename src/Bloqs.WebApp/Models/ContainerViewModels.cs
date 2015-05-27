using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bloqs.Models
{
    public class ContainerIndexModel
    {
        public string Id { get; set; }

        public string AccountId { get; set; }

        public string AccountName { get; set; }

        [DisplayName(@"名前")]
        public string Name { get; set; }

        public bool IsPublic { get; set; }

        [DisplayName(@"アクセシビリティ")]
        public string Accessibility
        {
            get { return IsPublic ? "公開" : "非公開"; }
        }

        [DisplayName("作成時刻 (UTC)")]
        public DateTime CreatedUtcDateTime { get; set; }

        [DisplayName("最終変更時刻 (UTC)")]
        public DateTime LastModifiedUtcDateTime { get; set; }

        [DisplayName(@"URL")]
        public string ContainerApiAddress { get; set; }

        [DisplayName("メタデータ")]
        public Metadata Metadata { get; set; }
    }

    public class ContainerCreateModel
    {
        public string Id { get; set; }

        public string AccountId { get; set; }

        [Required]
        [RegularExpression(@"[0-9a-zA-Z-]+", ErrorMessage = "Is only half-width alphanumeric characters and hyphens (-) can be used")]
        [DisplayName("コンテナ名")]
        public string Name { get; set; }

        [DisplayName("コンテナ内のBLOBを誰でもダウンロードできるようにする。")]
        public bool IsPublic { get; set; }

        [DisplayName("メタデータ")]
        public Metadata Metadata { get; private set; }

        public ContainerCreateModel()
        {
            Metadata = new Metadata();
        }
    }
    
    public class ContainerEditModel
    {
        private readonly Metadata _metadata = new Metadata();

        public string Id { get; set; }

        public string AccountId { get; set; }

        [ReadOnly(true)]
        [DisplayName("コンテナ名")]
        public string Name { get; set; }

        [DisplayName("コンテナ内のBLOBを誰でもダウンロードできるようにする。")]
        public bool IsPublic { get; set; }

        [DisplayName("メタデータ")]
        public Metadata Metadata
        {
            get { return _metadata; }
        }
    }

    public class ContainerDeleteModel
    {
        private readonly Metadata _metadata = new Metadata();

        public string Id { get; set; }

        public string AccountId { get; set; }

        [DisplayName("コンテナ名")]
        public string Name { get; set; }

        public bool IsPublic { get; set; }

        [DisplayName("アクセシビリティ")]
        public string Accessibility
        {
            get { return IsPublic ? "公開" : "非公開"; }
        }

        [DisplayName("作成時刻 (UTC)")]
        public DateTime CreatedUtcDateTime { get; set; }

        [DisplayName("最終変更時刻 (UTC)")]
        public DateTime LastModifiedUtcDateTime { get; set; }

        [DisplayName("メタデータ")]
        public Metadata Metadata
        {
            get { return _metadata; }
        }
    }
}