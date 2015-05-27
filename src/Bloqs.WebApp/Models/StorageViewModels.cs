using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bloqs.Models
{
    public class StorageIndexModel
    {
        public string Id { get; set; }
        
        [DisplayName("名前")]
        public string Name { get; set; }

        [DisplayName("サーバー名")]
        public string DataSource { get; set; }

        [DisplayName("所有者")]
        public string Owner { get; set; }

        [DisplayName("データベース名")]
        public string InitialCatalog { get; set; }

        [DisplayName("ストレージの種類")]
        public string StorageType { get; set; }

        [DisplayName("容量のしきい値")]
        public Size ThresholdSize { get; set; }
        
        [DisplayName("使用済サイズ")]
        public Size UsedSize { get; set; }

        [DisplayName("状態")]
        public string Status { get; set; }

        [DisplayName(@"作成時刻 (UTC)")]
        public DateTime CreatedUtcDateTime { get; set; }

        [DisplayName(@"最終変更時刻 (UTC)")]
        public DateTime LastModifiedUtcDateTime { get; set; }
    }

    public class PersonalStorageCreateModel
    {
        [Required]
        [DisplayName("名前")]
        public string Name { get; set; }

        [Required]
        [DisplayName("サーバー名")]
        public string DataSource { get; set; }

        [Required]
        [DisplayName("データベース名")]
        public string InitialCatalog { get; set; }

        [Required]
        [DisplayName("ユーザーID")]
        public string UserID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("パスワード")]
        public string Password { get; set; }

        [Required]
        [DisplayName("容量のしきい値")]
        public ThresholdSize ThresholdSize { get; set; }
    }

    public class PersonalStorageEditModel
    {
        public string Id { get; set; }

        [DisplayName("名前")]
        public string Name { get; set; }

        [Required]
        [DisplayName("サーバー名")]
        public string DataSource { get; set; }

        [Required]
        [DisplayName("データベース名")]
        public string InitialCatalog { get; set; }

        [Required]
        [DisplayName("ユーザーID")]
        public string UserID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("パスワード")]
        public string Password { get; set; }

        [DisplayName("容量のしきい値")]
        public Size ThresholdSize { get; set; }

        [DisplayName("使用済サイズ")]
        public Size UsedSize { get; set; }

        public bool IsAlreadyUsed { get; set; }

        [DisplayName("停止する。")]
        public bool IsSuspended { get; set; }

        [Required]
        [DisplayName("容量のしきい値(リサイズ)")]
        public ThresholdSize ThresholdReSize { get; set; }
    }

    public class PersonalStorageDeleteModel
    {
        public string Id { get; set; }

        [DisplayName("名前")]
        public string Name { get; set; }

        public bool IsAlreadyUsed { get; set; }

        [Required]
        [DisplayName("確認のためにアカウントの名前を入力して下さい。")]
        [System.ComponentModel.DataAnnotations.Compare("Name", ErrorMessage = "入力された名前がアカウントの名前と一致しません。")]
        public string ConfirmName { get; set; }
    }

    public enum ThresholdSize
    {
        [Display(Name = "100MB")]
        OneHundredMegabytes,
        [Display(Name = "200MB")]
        TwoHundredMegabytes,
        [Display(Name = "500MB")]
        FiveHundredMegabytes,
        [Display(Name = "1GB")]
        OneGigabytes,
    }
}