using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bloqs.Http.Net
{
    public sealed class Client
    {
        private readonly Credentials _credentials;
        private readonly Uri _baseAddress;

        internal Client(Uri baseAddress, Credentials credentials)
        {
            _baseAddress = baseAddress;
            _credentials = credentials;
        }

        public Task<Container> GetContainerReferenceAsync(string name)
        {
            return GetContainerReferenceAsync(name, CancellationToken.None);
        }

        public async Task<Container> GetContainerReferenceAsync(string name, CancellationToken cancellationToken)
        {
            using (var httpClient = CreateClient())
            using (var res =
                    await
                        httpClient.GetAsync(Account.ContainerReferenceEndpoint(_baseAddress, name), cancellationToken)
                            .ConfigureAwait(false))
            {
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
                var container = JsonConvert.DeserializeObject<Container.ResponseModel>(json).ToModel(this);

                return container;
            }
        }

        public Task<IReadOnlyCollection<Container>> ListContainersAsync()
        {
            return ListContainersAsync(CancellationToken.None);
        }

        public async Task<IReadOnlyCollection<Container>> ListContainersAsync(CancellationToken cancellationToken)
        {
            using (var httpClient = CreateClient())
            using (var res =
                    await
                        httpClient.GetAsync(Account.ContainerListEndpoint(_baseAddress), cancellationToken)
                            .ConfigureAwait(false))
            {
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
                var containers =
                    JsonConvert.DeserializeObject<IEnumerable<Container.ResponseModel>>(json)
                    .Select(x => x.ToModel(this)).ToArray();

                return containers;
            }
        }

        internal async Task CreateContainerIfNotExistsAsync(Container container, CancellationToken cancellationToken)
        {
            if (container == null) throw new ArgumentNullException("container");

            using (var httpClient = CreateClient())
            using (var content = new StringContent(JsonConvert.SerializeObject(container),Encoding.UTF8,"application/json"))
            using (var res =
                    await
                        httpClient.PostAsync(Account.ContainerCreateEndpoint(_baseAddress, container.Name), content, cancellationToken)
                            .ConfigureAwait(false))
            {
                res.EnsureSuccessStatusCode();
            }
        }

        internal async Task<Blob> GetBlobReferenceAsync(Container container, string name, CancellationToken cancellationToken)
        {
            using (var httpClient = CreateClient())
            using (var resAdditional =
                    await
                        httpClient.GetAsync(Account.BlobReferenceEndpoint(_baseAddress, container.Name, name), cancellationToken)
                            .ConfigureAwait(false))
            {
                resAdditional.EnsureSuccessStatusCode();

                var json = await resAdditional.Content.ReadAsStringAsync().ConfigureAwait(false);
                var blob = JsonConvert.DeserializeObject<Blob.ResponseModel>(json).ToModel(this, container);

                return blob;
            }
        }

        internal async Task<IReadOnlyCollection<Blob>> ListBlobsAsync(Container container,
            CancellationToken cancellationToken)
        {
            using (var httpClient = CreateClient())
            using (var res =
                    await
                        httpClient.GetAsync(Account.BlobListEndpoint(_baseAddress,container.Name), cancellationToken)
                            .ConfigureAwait(false))
            {
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
                var blobs =
                    JsonConvert.DeserializeObject<IEnumerable<Blob.ResponseModel>>(json)
                    .Select(x => x.ToModel(this, container));

                return blobs.ToArray();
            }
        }

        internal async Task UploadBlobFromByteArrayAsync(Blob blob, byte[] buffer, int index, int count, CancellationToken cancellationToken)
        {
            using (var httpClient = CreateClient())
            using (var content = new MultipartFormDataContent())
            using (var attributeContent = new StringContent(JsonConvert.SerializeObject(blob)))
            using (var fileContent = new ByteArrayContent(buffer, index, count))
            {
                attributeContent.Headers.ContentType.MediaType = "application/json";
                attributeContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "attributes",
                };
                content.Add(attributeContent);

                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                };

                content.Add(fileContent);

                using (var res =
                    await
                        httpClient.PostAsync(Account.BlobSaveEndpoint(_baseAddress, blob.Container.Name), content, cancellationToken)
                            .ConfigureAwait(false))
                {
                    res.EnsureSuccessStatusCode();
                }
            }
        }

        internal async Task DownloadBlobToStreamAsync(Blob blob, Stream target, CancellationToken cancellationToken)
        {
            using (var httpClient = CreateClient())
            using (var res =
                    await
                        httpClient.GetAsync(Account.BlobImageEndpoint(_baseAddress, blob.Container.Name, blob.Name), cancellationToken)
                            .ConfigureAwait(false))
            {
                res.EnsureSuccessStatusCode();

                using (var stream = await res.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    await stream.CopyToAsync(target).ConfigureAwait(false);
                }
            }
        }

        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(Account.RequestHeader, _credentials.AccessKey);

            return httpClient;
        }
    }
}
