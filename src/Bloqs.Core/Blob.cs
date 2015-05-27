using System;

namespace Bloqs
{
    /// <summary></summary>
    public class Blob : BlobAttributes
    {
        /// <summary></summary>
        public byte[] Image { get; set; }

        /// <summary></summary>
        public Blob()
            : this(new BlobAttributes())
        {
        }

        /// <summary></summary>
        public Blob(BlobAttributes attributes)
        {
            if (attributes == null) throw new ArgumentNullException("attributes");

            Container = attributes.Container;
            Id = attributes.Id;
            Name = attributes.Name;
            Properties = attributes.Properties;
            Storage = attributes.Storage;
            Metadata.Add(attributes.Metadata);
        }
    }
}
