using System;

namespace Bloqs
{
    /// <summary></summary>
    public class BlobAttributes
    {
        private readonly Metadata _metadata = new Metadata();

        /// <summary></summary>
        public Container Container { get; internal set; }

        /// <summary></summary>
        public string Id { get; set; }

        /// <summary></summary>
        public string Name { get; set; }

        /// <summary></summary>
        public BlobProperties Properties { get; internal set; }

        /// <summary></summary>
        public Metadata Metadata
        {
            get { return _metadata; }
        }

        /// <summary></summary>
        internal Storage Storage { get; set; }

        /// <summary></summary>
        internal BlobAttributes()
            : this(null, new BlobProperties())
        {
        }

        /// <summary></summary>
        internal BlobAttributes(Container container)
            : this(container, new BlobProperties())
        {
            Container = container;
        }

        /// <summary></summary>
        public BlobAttributes(Container container, BlobProperties properties)
        {
            Container = container;
            Properties = properties;
            Id = Guid.NewGuid().ToString("N");
        }
    }
}
