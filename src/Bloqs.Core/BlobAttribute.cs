using System.Collections.Generic;

namespace Bloqs
{
    public class BlobAttribute
    {
        private readonly IDictionary<string, string> _metadata = new Dictionary<string, string>();

        public string Name { get; set; }

        public BlobProperties Properties { get; internal set; }

        public IDictionary<string, string> Metadata
        {
            get { return _metadata; }
        }

        public BlobAttribute() : this(new BlobProperties())
        {
        }

        public BlobAttribute(BlobProperties properties)
        {
            Properties = properties;
        }
    }
}
