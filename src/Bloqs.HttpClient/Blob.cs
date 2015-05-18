using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bloqs.Http.Net
{
    public sealed class Blob
    {
        internal BloqsClient Client { get; set; }

        private readonly IDictionary<string, string> _metadata = new Dictionary<string, string>();
        private readonly BlobProperties _properties = new BlobProperties();

        public string Name { get; set; }

        public BlobProperties Properties
        {
            get { return _properties; }
        }

        public IDictionary<string, string> Metadata
        {
            get { return _metadata; }
        }

        internal Blob()
        {
        }

        internal Blob(BloqsClient client)
        {
            if (client == null) throw  new ArgumentNullException("client");
            Client = client;
        }

        public Uri Url { get; internal set; }

        public Task DownloadToStreamAsync(Stream target)
        {
            return DownloadToStreamAsync(target, CancellationToken.None);
        }

        public Task DownloadToStreamAsync(Stream target, CancellationToken cancellationToken)
        {
            return Client.DownloadToStreamAsync(this, target, cancellationToken);
        }

        public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count)
        {
            return UploadFromByteArrayAsync(buffer, index, count, CancellationToken.None);
        }

        public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, CancellationToken cancellationToken)
        {
            _properties.Length = buffer.Skip(index).Take(count).Count();
            return Client.UploadFromByteArrayAsync(this, buffer, index, count, cancellationToken);
        }
    }
}