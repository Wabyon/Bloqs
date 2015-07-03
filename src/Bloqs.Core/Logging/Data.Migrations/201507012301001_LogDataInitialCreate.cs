using FluentMigrator;

namespace Bloqs.Logging.Data.Migrations
{
    [Migration(201507012301001)]
    public class LogDataInitialCreate : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
CREATE TABLE [dbo].[TraceLogs]
(
    [Id] BIGINT IDENTITY(1,1) NOT NULL,
    [LogUtcDateTime] DATETIME2(7) NOT NULL DEFAULT(SYSUTCDATETIME()),
    [Logger] [NVARCHAR](500) NOT NULL,
    [Level] [NVARCHAR](10) NOT NULL,
    [ThreadId] [INT] NOT NULL,
    [MachineName] [NVARCHAR](100) NOT NULL,
    [Message] [NVARCHAR](MAX) NOT NULL,

    CONSTRAINT [PK_TraceLogs] PRIMARY KEY ([Id])
)
");

            Execute.Sql(@"
CREATE TABLE [dbo].[WebAccessLogs]
(
    [Id] BIGINT IDENTITY(1,1) NOT NULL,
    [AccessUtcDateTime] DATETIME2(7) NOT NULL DEFAULT(SYSUTCDATETIME()),
    [ServerName] [NVARCHAR](256) NOT NULL,
    [UserName] [NVARCHAR](256) NULL,
    [Url] [NVARCHAR](MAX) NOT NULL,
    [HttpMethod] [NVARCHAR](16) NOT NULL,
    [Path] [NVARCHAR](MAX) NOT NULL,
    [Query] [NVARCHAR](MAX) NULL,
    [Form] [NVARCHAR](MAX) NULL,
    [Controller] [NVARCHAR](256) NULL,
    [Action] [NVARCHAR](256) NULL,
    [UserHostAddress] [NVARCHAR](256) NULL,
    [UserAgent] [NVARCHAR](MAX) NULL,

    CONSTRAINT [PK_WebAccessLogs] PRIMARY KEY ([Id])
)
");
            Execute.Sql(@"
CREATE TABLE [dbo].[ApiAccessLogs]
(
    [Id] BIGINT IDENTITY(1,1) NOT NULL,
    [AccessUtcDateTime] DATETIME2(7) NOT NULL DEFAULT(SYSUTCDATETIME()),
    [ServerName] [NVARCHAR](256) NOT NULL,
    [UserName] [NVARCHAR](256) NULL,
    [Url] [NVARCHAR](MAX) NOT NULL,
    [HttpMethod] [NVARCHAR](16) NOT NULL,
    [Path] [NVARCHAR](MAX) NOT NULL,
    [Query] [NVARCHAR](MAX) NULL,
    [Form] [NVARCHAR](MAX) NULL,
    [Controller] [NVARCHAR](256) NULL,
    [Action] [NVARCHAR](256) NULL,
    [UserHostAddress] [NVARCHAR](256) NULL,
    [UserAgent] [NVARCHAR](MAX) NULL,

    CONSTRAINT [PK_ApiAccessLogs] PRIMARY KEY ([Id])
)
");

        }

        public override void Down()
        {
            Delete.Table("TraceLogs").InSchema("dbo");
            Delete.Table("WebAccessLogs").InSchema("dbo");
        }
    }
}
