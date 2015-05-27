namespace Bloqs.Models
{
    public class BlobResponseModel
    {
        public string Name { get; set; }
        public BlobProperties Properties { get; set; }
        public Metadata Metadata { get; set; }
    }

    public class BlobRequestModel
    {
        public string Name { get; set; }
        public BlobProperties Properties { get; set; }
        public Metadata Metadata { get; set; }
    }
}