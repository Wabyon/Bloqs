using System.Collections.Generic;

namespace Bloqs
{
    public class Blob
    {
        private readonly BlobAttribute _attribute;

        public string Name
        {
            get { return _attribute.Name; }
            set { _attribute.Name = value; }
        }

        public byte[] Image { get; set; }

        public BlobProperties Properties
        {
            get { return _attribute.Properties; }
            internal set { _attribute.Properties = value; }
        }

        public IDictionary<string, string> Metadata
        {
            get { return _attribute.Metadata; }
        }

        public Blob() : this(new BlobAttribute())
        {
        }

        public Blob(BlobAttribute attribute)
        {
            _attribute = attribute;
        }
    }
}
