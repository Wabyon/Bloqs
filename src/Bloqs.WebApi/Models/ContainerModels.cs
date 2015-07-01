using System;

namespace Bloqs.Models
{
    public class ContainerRequestModel
    {
        /// <summary></summary>
        public string Name { get; set; }

        /// <summary></summary>
        public bool IsPublic { get; set; }

        /// <summary></summary>
        public Metadata Metadata { get; set; }

        public ContainerRequestModel()
        {
            Metadata = new Metadata();
        }
    }

    public class ContainerResponseModel
    {
        /// <summary></summary>
        public string Name { get; set; }

        /// <summary></summary>
        public bool IsPublic { get; set; }

        /// <summary></summary>
        public Metadata Metadata { get; set; }

        /// <summary></summary>
        public DateTime? CreatedUtcDateTime { get; set; }

        /// <summary></summary>
        public DateTime? LastModifiedUtcDateTime { get; set; }

        public ContainerResponseModel()
        {
            Metadata = new Metadata();
        }
    }
}