using System;

namespace Bloqs
{
    /// <summary></summary>
    public class Container
    {
        private readonly Metadata _metadata = new Metadata();

        /// <summary></summary>
        public Account Account { get; internal set; }

        /// <summary></summary>
        public string Id { get; set; }

        /// <summary></summary>
        public string Name { get; set; }

        /// <summary></summary>
        public bool IsPublic { get; set; }

        /// <summary></summary>
        public Metadata Metadata
        {
            get { return _metadata; }
        }

        /// <summary></summary>
        public DateTime CreatedUtcDateTime { get; set; }

        /// <summary></summary>
        public DateTime LastModifiedUtcDateTime { get; set; }

        internal Container()
        {
        }

        internal Container(Account accont)
        {
            if (accont == null) throw new ArgumentNullException("accont");
            Account = accont;

            Id = Guid.NewGuid().ToString("N");
        }

        public BlobAttributes CreateBlobAttributes(string name)
        {
            return new BlobAttributes(this)
            {
                Name = name,
                Properties = { CreatedUtcDateTime = DateTime.UtcNow, LastModifiedUtcDateTime = DateTime.UtcNow },
            };
        }
    }
}
