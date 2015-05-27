using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Bloqs.Models
{
    public class AccountIndexModel
    {
        public string Id { get; set; }

        [DisplayName(@"名前")]
        public string Name { get; set; }

        [DisplayName(@"所有者")]
        public string Owner { get; set; }

        [DisplayName(@"プライマリアクセスキー")]
        public string PrimaryAccessKey { get; set; }

        [DisplayName(@"セカンダリアクセスキー")]
        public string SecondaryAccessKey { get; set; }

        public StorageType StorageType { get; set; }

        [DisplayName(@"ストレージの種類")]
        public string StorageTypeDisplay { get; set; }

        [DisplayName(@"URL")]
        public string AccountApiAddress { get; set; }

        [DisplayName(@"作成時刻 (UTC)")]
        public DateTime CreatedUtcDateTime { get; set; }

        [DisplayName(@"最終変更時刻 (UTC)")]
        public DateTime LastModifiedUtcDateTime { get; set; }

        [DisplayName(@"ストレージ")]
        public string[] Storages { get; set; }
    }

    public class AccountCreateModel
    {
        [Required]
        [StringLength(128, ErrorMessage = "{0} の長さは {1} までです。")]
        [RegularExpression(@"[0-9a-zA-Z-_]+", ErrorMessage = "半角英数とハイフン、アンダーバーのみ入力できます。")]
        [DisplayName(@"アカウント名")]
        public string Name { get; set; }

        [DisplayName(@"個別ストレージを使用する")]
        public bool UsePersonalStorage { get; set; }

        [DisplayName(@"ストレージ")]
        public string PersonalStorageId { get; set; }

        public List<SelectListItem> PersonalSorages { get; set; }
    }

    public class AccountEditModel
    {
        public string Id { get; set; }

        [DisplayName(@"アカウント名")]
        public string Name { get; set; }

        [Required]
        [DisplayName(@"プライマリアクセスキー")]
        public string PrimaryAccessKey { get; set; }

        [Required]
        [DisplayName(@"セカンダリアクセスキー")]
        public string SecondaryAccessKey { get; set; }
    }

    public class AccountDeleteModel
    {
        public string Id { get; set; }

        [DisplayName(@"アカウント名")]
        public string Name { get; set; }

        [Required]
        [DisplayName("確認のためにアカウントの名前を入力して下さい。")]
        [System.ComponentModel.DataAnnotations.Compare("Name", ErrorMessage = "入力された名前がアカウントの名前と一致しません。")]
        public string ConfirmName { get; set; }
    }
}