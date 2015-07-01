using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bloqs.Http.Net
{
    public sealed class Container
    {
        private IDictionary<string, string> _metadata = new Dictionary<string, string>();
        private readonly Client _client;

        public string Name { get; internal set; }
        
        public bool IsPublic { get; set; }

        public IDictionary<string, string> Metadata
        {
            get { return _metadata; }
        }

        public DateTime? CreatedUtcDateTime { get; set; }
        
        public DateTime? LastModifiedUtcDateTime { get; set; }

        internal Container(Client client)
        {
            _client = client;
        }

        public Task CreateIfNotExistsAsync()
        {
            return CreateIfNotExistsAsync(CancellationToken.None);
        }

        public Task CreateIfNotExistsAsync(CancellationToken cancellationToken)
        {
            return _client.CreateContainerIfNotExistsAsync(this, cancellationToken);
        }

        public Task<Blob> GetBlobReferenceAsync(string name)
        {
            return GetBlobReferenceAsync(name, CancellationToken.None);
        }

        public Task<Blob> GetBlobReferenceAsync(string name, CancellationToken cancellationToken)
        {
            return _client.GetBlobReferenceAsync(this, name, cancellationToken);
        }

        public Task<IReadOnlyCollection<Blob>> ListBlobsAsync()
        {
            return ListBlobsAsync(CancellationToken.None);
        }

        public Task<IReadOnlyCollection<Blob>> ListBlobsAsync(CancellationToken cancellationToken)
        {
            return _client.ListBlobsAsync(this, cancellationToken);
        }

        internal class ResponseModel
        {
            public string Name { get; set; }
            public bool IsPublic { get; set; }
            public Dictionary<string, string> Metadata { get; set; }
            public DateTime? CreatedUtcDateTime { get; set; }
            public DateTime? LastModifiedUtcDateTime { get; set; }

            public ResponseModel()
            {
                Metadata = new Dictionary<string, string>();
            }

            public Container ToModel(Client client)
            {
                var container = new Container(client)
                {
                    Name = Name,
                    IsPublic = IsPublic,
                    CreatedUtcDateTime = CreatedUtcDateTime,
                    LastModifiedUtcDateTime = LastModifiedUtcDateTime,
                    _metadata = Metadata,
                };
                return container;
            }
        }
    }
}
