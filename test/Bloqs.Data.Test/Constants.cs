namespace Bloqs.Data
{
    internal class Constants
    {
        public const string ConnectionString = @"Data Source=(localdb)\v11.0;Initial Catalog=BloqsTest;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public const string LoggingDbConnectionString = @"Data Source=(localdb)\v11.0;Initial Catalog=BloqsTest_Log;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public const string AllTableTruncateCommandText = @"
declare @name sysname
declare cur cursor for select name from sys.tables where (name <> 'VersionInfo' and name <> 'Logs') order by 1
open cur fetch next from cur into @name
while @@fetch_status = 0
begin
declare @sql nvarchar(max) = ''
set @sql = 'truncate table ' + @name
execute (@sql)
fetch next from cur into @name
end
";
    }
}
