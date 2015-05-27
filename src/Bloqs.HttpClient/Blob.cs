using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bloqs.Http.Net
{
    public sealed class Blob
    {
        private readonly Client _client;
        private readonly Container _container;
        private IDictionary<string, string> _metadata = new Dictionary<string, string>();
        private BlobProperties _properties = new BlobProperties();

        public string Name { get; set; }

        public BlobProperties Properties
        {
            get { return _properties; }
        }

        public IDictionary<string, string> Metadata
        {
            get { return _metadata; }
        }

        internal Container Container { get { return _container; } }

        internal Blob(Client client, Container container)
        {
            if (client == null) throw  new ArgumentNullException("client");
            _client = client;
            _container = container;
        }

        public Task DownloadToStreamAsync(Stream target)
        {
            return DownloadToStreamAsync(target, CancellationToken.None);
        }

        public Task DownloadToStreamAsync(Stream target, CancellationToken cancellationToken)
        {
            return _client.DownloadBlobToStreamAsync(this, target, cancellationToken);
        }

        public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count)
        {
            return UploadFromByteArrayAsync(buffer, index, count, CancellationToken.None);
        }

        public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, CancellationToken cancellationToken)
        {
            return _client.UploadBlobFromByteArrayAsync(this, buffer, index, count, cancellationToken);
        }

        internal class ResponseModel
        {
            public string Name { get; set; }
            public BlobProperties.ResponseModel Properties { get; set; }
            public Dictionary<string,string> Metadata { get; set; }

            public ResponseModel()
            {
                Metadata = new Dictionary<string, string>();
            }

            public Blob ToModel(Client client, Container container)
            {
                var blob = new Blob(client, container)
                {
                    Name = Name,
                    _properties = Properties.ToModel(),
                    _metadata = Metadata,
                };
                return blob;
            }
        }
    }
}