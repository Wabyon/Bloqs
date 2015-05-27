using FluentMigrator;

namespace Bloqs.Data.Migrations
{
    [Migration(0)]
    public class InitialCreate : Migration
    {
        public override void Up()
        {
            #region Storages
            Execute.Sql(@"
CREATE TABLE [dbo].[Storages]
(
    [Id] NVARCHAR(256) NOT NULL,
    [Owner] NVARCHAR(256) NOT NULL,
    [Name] NVARCHAR(256) NOT NULL,
    [StorageType] TINYINT NOT NULL DEFAULT (0),
    [ConnectionString] NVARCHAR(512) NOT NULL,
    [ThresholdLength] BIGINT NOT NULL DEFAULT (0),
    [IsSuspended] BIT NOT NULL DEFAULT (0),
    [CreatedUtcDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),
    [LastModifiedUtcDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT [PK_Storages] PRIMARY KEY ([Id])
)
");
            Execute.Sql(@"
CREATE UNIQUE INDEX [IX_Storages_Owner_Name] ON [dbo].[Storages] ([Owner], [Name])
");
            #endregion

            #region Accounts
            Execute.Sql(@"
CREATE TABLE [dbo].[Accounts]
(
    [Id] NVARCHAR(256) NOT NULL,
    [Name] NVARCHAR(256) NOT NULL,
    [Owner] NVARCHAR(256) NOT NULL,
    [PrimaryAccessKey] NVARCHAR(256) NOT NULL,
    [SecondaryAccessKey] NVARCHAR(256) NOT NULL,
    [CryptKey] NVARCHAR(256) NOT NULL,
    [CreatedUtcDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),
    [LastModifiedUtcDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),
    [StorageType] TINYINT NOT NULL DEFAULT (0),

    CONSTRAINT [PK_Accounts] PRIMARY KEY ([Id])
)
");

            Execute.Sql(@"
CREATE INDEX [IX_Accounts_Owner_Id] ON [dbo].[Accounts] ([Owner], [Id])
");
            #endregion

            #region AccountUsingStorages
            Execute.Sql(@"
CREATE TABLE [dbo].[AccountUsingStorages]
(
    [AccountId] NVARCHAR(256) NOT NULL,
    [StorageId] NVARCHAR(256) NOT NULL,

    CONSTRAINT [PK_AccountUsingStorages] PRIMARY KEY ([AccountId], [StorageId])
)
");
            #endregion

            #region DeletedAccounts
            Execute.Sql(@"
CREATE TABLE [dbo].[DeletedAccounts]
(
    [Id] NVARCHAR(256) NOT NULL,
    [Name] NVARCHAR(256) NOT NULL,
    [Owner] NVARCHAR(256) NOT NULL,
    [PrimaryAccessKey] NVARCHAR(256) NOT NULL,
    [SecondaryAccessKey] NVARCHAR(256) NOT NULL,
    [CryptKey] NVARCHAR(256) NOT NULL,
    [CreatedUtcDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),
    [LastModifiedUtcDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),
    [DeletedUtcDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),
    [StorageType] TINYINT NOT NULL DEFAULT (0),

    CONSTRAINT [PK_DeletedAccounts] PRIMARY KEY ([Id], [DeletedUtcDateTime])
)
");
            #endregion

            #region Containers
            Execute.Sql(@"
CREATE TABLE [dbo].[Containers]
(
    [Id] NVARCHAR(256) NOT NULL,
    [AccountId] NVARCHAR(256) NOT NULL,
    [Name] NVARCHAR(256) NOT NULL,
    [IsPublic] BIT NOT NULL DEFAULT (0),
    [Metadata] NVARCHAR(MAX) NOT NULL DEFAULT ('{{}}'), 
    [CreatedUtcDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),
    [LastModifiedUtcDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT [PK_Containers] PRIMARY KEY ([Id])
)
");
            Execute.Sql(@"
CREATE UNIQUE INDEX [IX_Containers_AccountId_Name] ON [dbo].[Containers] ([AccountId], [Name])
");
            #endregion

            #region BlobAttributes
            Execute.Sql(@"
CREATE TABLE [dbo].[BlobAttributes]
(
    [Id] NVARCHAR(256) NOT NULL,
    [ContainerId] NVARCHAR(256) NOT NULL,
    [Name] NVARCHAR(256) NOT NULL,
    [CacheControl] NVARCHAR(256),
    [ContentDisposition] NVARCHAR(256),
    [ContentEncoding] NVARCHAR(256),
    [ContentLanguage] NVARCHAR(256),
    [ContentMD5] NVARCHAR(256),
    [ContentType] NVARCHAR(256),
    [ETag] NVARCHAR(256),
    [Length] BIGINT NOT NULL DEFAULT (0),
    [Metadata] NVARCHAR(MAX) NOT NULL DEFAULT ('{{}}'),
    [CreatedUtcDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),
    [LastModifiedUtcDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),
    [StorageId] NVARCHAR(256) NOT NULL,

    CONSTRAINT [PK_BlobAttributes] PRIMARY KEY ([Id])
)
");
            Execute.Sql(@"
CREATE UNIQUE INDEX [IX_BlobAttributes_AccountId_ContainerId_Name] ON [dbo].[BlobAttributes] ([ContainerId], [Name])
");
            #endregion
        }

        public override void Down()
        {
            Delete.Table("BlobAttributes").InSchema("dbo");
            Delete.Table("Containers").InSchema("dbo");
            Delete.Table("DeletedAccounts").InSchema("dbo");
            Delete.Table("AccountUsingStorages").InSchema("dbo");
            Delete.Table("Accounts").InSchema("dbo");
            Delete.Table("Storages").InSchema("dbo");
        }
    }
}
