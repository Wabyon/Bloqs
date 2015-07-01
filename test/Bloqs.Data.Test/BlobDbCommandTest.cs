using System;
using System.Threading.Tasks;
using Bloqs.Data.Commands;
using NUnit.Framework;

namespace Bloqs.Data
{
    [TestFixture]
    public class BlobDbCommandTest : DbCommandTestBase
    {
        private readonly BlobDbCommand _dbCommand = new BlobDbCommand(Constants.ConnectionString);

        [Test]
        public async Task CreateBlobToPersonalStorageTest()
        {
            var image = CreateRandomImage();

            var blobAttributes = Mock.PersonalStorageContainer.CreateBlobAttributes("CreateBlobToPersonalStorageTest");
            blobAttributes.Properties.CacheControl = "CacheControl";
            blobAttributes.Properties.ContentDisposition = "ContentDisposition";
            blobAttributes.Properties.ContentEncoding = "ContentEncoding";
            blobAttributes.Properties.ContentLanguage = "ContentLanguage";
            blobAttributes.Properties.ContentMD5 = "ContentMD5";
            blobAttributes.Properties.ContentType = "ContentType";
            blobAttributes.Properties.ETag = "ETag";
            blobAttributes.Properties.Length = image.LongLength;
            blobAttributes.Properties.LastModifiedUtcDateTime = new DateTime(2016, 1, 1);
            blobAttributes.Metadata["key1"] = "value1";
            blobAttributes.Metadata["key2"] = "value2";

            var blob = new Blob(blobAttributes) { Image = image };

            await _dbCommand.SaveAsync(blob);

            var created = await _dbCommand.FindAsync(blob.Id);

            created.Id.Is(blob.Id);
            created.Name.Is(blob.Name);
            created.Properties.IsStructuralEqual(blob.Properties);
            created.Metadata.IsStructuralEqual(blob.Metadata);

            created.Image.Is(blob.Image);
            created.Properties.IsStructuralEqual(blob.Properties);
        }

        private static byte[] CreateRandomImage()
        {
            var randam = new Random();
            var image = new byte[1024];
            randam.NextBytes(image);
            return image;
        }
    }
}
