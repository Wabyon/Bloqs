using System.Linq;
using AutoMapper;
using Bloqs.Data;
using Bloqs.Internals;
using Bloqs.Models;

namespace Bloqs
{
    public class MappingConfig
    {
        public static void CreateMaps()
        {
            #region Account
            Mapper.CreateMap<Account, AccountIndexModel>()
                .AfterMap((s, d) =>
                {
                    d.AccountApiAddress = Utilities.CreateApiAddress(s.Name);
                    d.StorageTypeDisplay = s.StorageType == StorageType.Common ? @"共有ストレージ" : @"個別ストレージ";
                    d.Storages = s.Storages.Select(x => x.Name).ToArray();
                });
            Mapper.CreateMap<Account, AccountCreateModel>();
            Mapper.CreateMap<Account, AccountEditModel>();
            Mapper.CreateMap<Account, AccountDeleteModel>();

            Mapper.CreateMap<AccountCreateModel, Account>();
            Mapper.CreateMap<AccountEditModel, Account>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Name, o => o.Ignore());
            Mapper.CreateMap<AccountDeleteModel, Account>();
            #endregion

            #region Container
            Mapper.CreateMap<Container, ContainerIndexModel>()
                .ForMember(d => d.AccountId, o => o.MapFrom(s => s.Account.Id))
                .ForMember(d => d.AccountName, o => o.MapFrom(s => s.Account.Name))
                .AfterMap((s, d) =>
                {
                    d.ContainerApiAddress = Utilities.CreateApiAddress(s.Account.Name, s.Name);
                });
            Mapper.CreateMap<Container, ContainerCreateModel>()
                .ForMember(d => d.AccountId, o => o.MapFrom(s => s.Account.Id))
                .AfterMap((s, d) =>
                {
                    d.Metadata.Clear();
                    foreach (var metadata in s.Metadata)
                    {
                        d.Metadata.Add(metadata.Key, metadata.Value);
                    }
                });
            Mapper.CreateMap<Container, ContainerEditModel>()
                .ForMember(d => d.AccountId, o => o.MapFrom(s => s.Account.Id))
                .AfterMap((s, d) =>
                {
                    d.Metadata.Clear();
                    foreach (var metadata in s.Metadata)
                    {
                        d.Metadata.Add(metadata.Key, metadata.Value);
                    }
                });
            Mapper.CreateMap<Container, ContainerDeleteModel>()
                .ForMember(d => d.AccountId, o => o.MapFrom(s => s.Account.Id))
                .AfterMap((s, d) =>
                {
                    d.Metadata.Clear();
                    foreach (var metadata in s.Metadata)
                    {
                        d.Metadata.Add(metadata.Key, metadata.Value);
                    }
                });
            Mapper.CreateMap<ContainerCreateModel, Container>()
                .AfterMap((s, d) =>
                {
                    d.Metadata.Clear();
                    foreach (var metadata in s.Metadata)
                    {
                        d.Metadata.Add(metadata.Key, metadata.Value);
                    }
                });
            Mapper.CreateMap<ContainerEditModel, Container>()
                .ForMember(d => d.Name, o => o.Ignore())
                .AfterMap((s, d) =>
                {
                    d.Metadata.Clear();
                    foreach (var metadata in s.Metadata)
                    {
                        d.Metadata.Add(metadata.Key, metadata.Value);
                    }
                });
            Mapper.CreateMap<ContainerDeleteModel, Container>()
                .AfterMap((s, d) =>
                {
                    d.Metadata.Clear();
                    foreach (var metadata in s.Metadata)
                    {
                        d.Metadata.Add(metadata.Key, metadata.Value);
                    }
                });
            #endregion

            #region Blob
            Mapper.CreateMap<BlobAttributes, BlobIndexModel>()
                .ForMember(d => d.AccountId, o => o.MapFrom(s => s.Container.Account.Id))
                .ForMember(d => d.AccountName, o => o.MapFrom(s => s.Container.Account.Name))
                .ForMember(d => d.ContainerId, o => o.MapFrom(s => s.Container.Id))
                .ForMember(d => d.ContainerName, o => o.MapFrom(s => s.Container.Name))
                .AfterMap((s, d) =>
                {
                    d.Size = new Size(s.Properties.Length);
                    d.BlobApiAddress = Utilities.CreateApiAddress(s.Container.Account.Name, s.Container.Name, s.Name);
                    d.CacheControl = s.Properties.CacheControl;
                    d.ContentDisposition = s.Properties.ContentDisposition;
                    d.ContentEncoding = s.Properties.ContentEncoding;
                    d.ContentLanguage = s.Properties.ContentLanguage;
                    d.ContentMD5 = s.Properties.ContentMD5;
                    d.ContentType = s.Properties.ContentType;
                    d.ETag = s.Properties.ETag;
                    d.CreatedUtcDateTime = s.Properties.CreatedUtcDateTime;
                    d.LastModifiedUtcDateTime = s.Properties.LastModifiedUtcDateTime;
                });
            Mapper.CreateMap<BlobAttributes, BlobEditModel>()
                .ForMember(d => d.AccountId, o => o.MapFrom(s => s.Container.Account.Id))
                .ForMember(d => d.AccountName, o => o.MapFrom(s => s.Container.Account.Name))
                .ForMember(d => d.ContainerId, o => o.MapFrom(s => s.Container.Id))
                .ForMember(d => d.ContainerName, o => o.MapFrom(s => s.Container.Name))
                .ForMember(d => d.CacheControl, o => o.MapFrom(s => s.Properties.CacheControl))
                .ForMember(d => d.ContentDisposition, o => o.MapFrom(s => s.Properties.ContentDisposition))
                .ForMember(d => d.ContentEncoding, o => o.MapFrom(s => s.Properties.ContentEncoding))
                .ForMember(d => d.ContentLanguage, o => o.MapFrom(s => s.Properties.ContentLanguage))
                .ForMember(d => d.ContentMD5, o => o.MapFrom(s => s.Properties.ContentMD5))
                .ForMember(d => d.ContentType, o => o.MapFrom(s => s.Properties.ContentType))
                .ForMember(d => d.ETag, o => o.MapFrom(s => s.Properties.ETag))
                .ForMember(d => d.LastModifiedUtcDateTime, o => o.MapFrom(s => s.Properties.LastModifiedUtcDateTime))
                .AfterMap((s, d) =>
                {
                    d.Size = new Size(s.Properties.Length);
                    d.Metadata.Clear();
                    foreach (var metadata in s.Metadata)
                    {
                        d.Metadata.Add(metadata.Key, metadata.Value);
                    }
                });
            Mapper.CreateMap<BlobAttributes, BlobDeleteModel>()
                .ForMember(d => d.AccountId, o => o.MapFrom(s => s.Container.Account.Id))
                .ForMember(d => d.AccountName, o => o.MapFrom(s => s.Container.Account.Name))
                .ForMember(d => d.ContainerId, o => o.MapFrom(s => s.Container.Id))
                .ForMember(d => d.ContainerName, o => o.MapFrom(s => s.Container.Name))
                .ForMember(d => d.CacheControl, o => o.MapFrom(s => s.Properties.CacheControl))
                .ForMember(d => d.ContentDisposition, o => o.MapFrom(s => s.Properties.ContentDisposition))
                .ForMember(d => d.ContentEncoding, o => o.MapFrom(s => s.Properties.ContentEncoding))
                .ForMember(d => d.ContentLanguage, o => o.MapFrom(s => s.Properties.ContentLanguage))
                .ForMember(d => d.ContentMD5, o => o.MapFrom(s => s.Properties.ContentMD5))
                .ForMember(d => d.ContentType, o => o.MapFrom(s => s.Properties.ContentType))
                .ForMember(d => d.ETag, o => o.MapFrom(s => s.Properties.ETag))
                .ForMember(d => d.LastModifiedUtcDateTime, o => o.MapFrom(s => s.Properties.LastModifiedUtcDateTime))
                .AfterMap((s, d) =>
                {
                    d.Size = new Size(s.Properties.Length);
                    d.Metadata.Clear();
                    foreach (var metadata in s.Metadata)
                    {
                        d.Size = new Size(s.Properties.Length);
                        d.Metadata.Add(metadata.Key, metadata.Value);
                    }
                });
            Mapper.CreateMap<BlobEditModel, BlobAttributes>()
                .AfterMap((s, d) =>
                {
                    d.Properties.CacheControl = s.CacheControl;
                    d.Properties.ContentDisposition = s.ContentDisposition;
                    d.Properties.ContentEncoding = s.ContentEncoding;
                    d.Properties.ContentLanguage = s.ContentLanguage;
                    d.Properties.ContentMD5 = s.ContentMD5;
                    d.Properties.ContentType = s.ContentType;
                    d.Properties.LastModifiedUtcDateTime = s.LastModifiedUtcDateTime;

                    d.Metadata.Clear();
                    foreach (var metadata in s.Metadata)
                    {
                        d.Metadata.Add(metadata.Key, metadata.Value);
                    }
                });
            Mapper.CreateMap<BlobDeleteModel, BlobAttributes>()
                .ForMember(d => d.Properties, o => o.MapFrom(s => new BlobProperties
                {
                    CacheControl = s.CacheControl,
                    ContentDisposition = s.ContentDisposition,
                    ContentEncoding = s.ContentEncoding,
                    ContentLanguage = s.ContentLanguage,
                    ContentMD5 = s.ContentMD5,
                    ContentType = s.ContentType,
                    LastModifiedUtcDateTime = s.LastModifiedUtcDateTime,
                }))
                .AfterMap((s, d) =>
                {
                    d.Properties.Length = s.Size.OriginalLength;
                    d.Metadata.Clear();
                    foreach (var metadata in s.Metadata)
                    {
                        d.Metadata.Add(metadata.Key, metadata.Value);
                    }
                });
            #endregion

            #region Storage
            Mapper.CreateMap<Storage, StorageIndexModel>()
                .ForMember(d => d.ThresholdSize, o => o.MapFrom(s => new Size(s.ThresholdLength)))
                .ForMember(d => d.UsedSize, o => o.MapFrom(s => new Size(s.UsedLength)))
                .ForMember(d => d.DataSource, o => o.MapFrom(s => s.ConnectionProperties.DataSource))
                .ForMember(d => d.InitialCatalog, o => o.MapFrom(s => s.ConnectionProperties.InitialCatalog))
                .AfterMap((s, d) =>
                {
                    d.StorageType = s.StorageType == StorageType.Common ? @"共有" : @"個別";
                    if (s.IsSuspended) d.Status = "停止中";
                    d.Status = "稼働中";
                });

            Mapper.CreateMap<PersonalStorageCreateModel, Storage>()
                .ForMember(d => d.ThresholdLength, o => o.MapFrom(s => s.ThresholdSize.ToInt64()))
                .AfterMap((s, d) =>
                {
                    d.ConnectionProperties =
                        new ConnectionProperties(s.DataSource, s.InitialCatalog, s.UserID, s.Password);
                });

            Mapper.CreateMap<Storage, PersonalStorageEditModel>()
                .ForMember(d => d.ThresholdSize, o => o.MapFrom(s => new Size(s.ThresholdLength)))
                .ForMember(d => d.UsedSize, o => o.MapFrom(s => new Size(s.UsedLength)))
                .ForMember(d => d.DataSource, o => o.MapFrom(s => s.ConnectionProperties.DataSource))
                .ForMember(d => d.InitialCatalog, o => o.MapFrom(s => s.ConnectionProperties.InitialCatalog))
                .ForMember(d => d.UserID, o => o.MapFrom(s => s.ConnectionProperties.UserID));

            Mapper.CreateMap<PersonalStorageEditModel, Storage>()
                .ForMember(d => d.ThresholdLength, o => o.MapFrom(s => s.ThresholdReSize.ToInt64()))
                .AfterMap((s, d) =>
                {
                    d.ConnectionProperties =
                        new ConnectionProperties(s.DataSource, s.InitialCatalog, s.UserID, s.Password);
                });
            Mapper.CreateMap<Storage, PersonalStorageDeleteModel>();
            #endregion
        }
    }
}