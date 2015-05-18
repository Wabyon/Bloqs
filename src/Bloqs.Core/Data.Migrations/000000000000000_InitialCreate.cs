using FluentMigrator;

namespace Bloqs.Data.Migrations
{
    [Migration(0)]
    public class InitialCreate : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
CREATE TABLE [dbo].[Containers]
(
    [Id] NVARCHAR(256) NOT NULL,
	[Name] NVARCHAR(256) NOT NULL,
    [IsPublic] BIT NOT NULL DEFAULT (0),
    [Owner] NVARCHAR(256) NOT NULL,
    [PrimaryAccessKey] NVARCHAR(256) NOT NULL,
    [SecondaryAccessKey] NVARCHAR(256) NOT NULL,
    [CreatedUtcDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),
    [LastModifiedUtcDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT [PK_Containers] PRIMARY KEY ([Id]), 
)
");

            Create.Index("IX_Containers_Name").OnTable("Containers").InSchema("dbo").OnColumn("Name").Unique();
        }

        public override void Down()
        {
            Delete.Table("Containers")
                .InSchema("dbo");
        }
    }
}
