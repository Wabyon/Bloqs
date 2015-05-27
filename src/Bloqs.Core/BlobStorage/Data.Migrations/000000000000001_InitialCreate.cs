using FluentMigrator;

namespace Bloqs.BlobStorage.Data.Migrations
{
    [Migration(1)]
    public class InitialCreate : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
CREATE TABLE [dbo].[Blobs]
(
    [AccountId] NVARCHAR(256) NOT NULL,
    [ContainerId] NVARCHAR(256) NOT NULL,
    [BlobId] NVARCHAR(256) NOT NULL,
    [Image] VARBINARY(MAX) NOT NULL,

    CONSTRAINT [PK_Blobs] PRIMARY KEY ([AccountId], [ContainerId], [BlobId])
)
");
        }

        public override void Down()
        {
            Delete.Table("Blobs").InSchema("dbo");
        }
    }
}
