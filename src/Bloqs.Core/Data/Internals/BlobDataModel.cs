using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bloqs.Data.Internals
{
    internal class BlobDataModel
    {
        public string Name { get; set; }

        public byte[] Image { get; set; }

        public string Properties { get; set; }

        public string Metadata { get; set; }

        public BlobDataModel()
        {
        }

        public BlobDataModel(Blob blob)
        {
            Name = blob.Name;
            Image = blob.Image;
            Properties = JsonConvert.SerializeObject(blob.Properties);
            Metadata = JsonConvert.SerializeObject(blob.Metadata);
        }

        public BlobDataModel(BlobAttribute blob)
        {
            Name = blob.Name;
            Properties = JsonConvert.SerializeObject(blob.Properties);
            Metadata = JsonConvert.SerializeObject(blob.Metadata);
        }

        public Blob ToBlob()
        {
            return new Blob(ToBlobAttribute()) {Image = Image};
        }

        public BlobAttribute ToBlobAttribute()
        {
            var blobAttribute = new BlobAttribute
            {
                Name = Name,
                Properties = JsonConvert.DeserializeObject<BlobProperties>(Properties),
            };

            foreach (var keyValuePair in JsonConvert.DeserializeObject<Dictionary<string, string>>(Metadata))
            {
                blobAttribute.Metadata.Add(keyValuePair);
            }

            return blobAttribute;
        }
    }
}