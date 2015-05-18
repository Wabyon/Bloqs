using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bloqs.Http.Net
{
    public sealed class BloqsClient
    {
        private readonly Dictionary<string, Blob> _blobs = new Dictionary<string, Blob>();

        public Uri BaseAddress { get; private set; }

        public string ContainerName { get; private set; }

        internal string AccessKey { get; private set; }

        public BloqsClient(string baseAddress, string containerName, string accessKey)
            : this(new Uri(baseAddress), containerName, accessKey)
        {
        }

        public BloqsClient(Uri baseAddress, string containerName, string accessKey)
        {
            BaseAddress = baseAddress;
            ContainerName = containerName;
            AccessKey = accessKey;
        }

        public async Task<Blob> GetBlobReferenceAsync(string name)
        {
            if (_blobs.ContainsKey(name)) return _blobs[name];

            using (var httpClient = CreateClient())
            using (var resAdditional =
                    await
                        httpClient.GetAsync(string.Format(Constants.GetAdditionalUrl, ContainerName, name))
                            .ConfigureAwait(false))
            {
                resAdditional.EnsureSuccessStatusCode();

                var json = await resAdditional.Content.ReadAsStringAsync().ConfigureAwait(false);
                var blob = JsonConvert.DeserializeObject<Blob>(json);

                blob.Client = this;

                _blobs.Add(name, blob);

                return blob;
            }
        }

        internal async Task UploadFromByteArrayAsync(Blob blob, byte[] buffer, int index, int count, CancellationToken cancellationToken)
        {
            using (var httpClient = CreateClient())
            using (var content = new MultipartFormDataContent())
            using (var attributeContent = new StringContent(JsonConvert.SerializeObject(blob)))
            using (var fileContent = new ByteArrayContent(buffer, index, count))
            {
                attributeContent.Headers.ContentType.MediaType = "application/json";
                attributeContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "attribute",
                };
                content.Add(attributeContent);
                
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                };

                content.Add(fileContent);

                using (var res =
                    await
                        httpClient.PostAsync(string.Format(Constants.PostUrl, ContainerName), content, cancellationToken)
                            .ConfigureAwait(false))
                {
                    res.EnsureSuccessStatusCode();
                }
            }
        }

        internal async Task DownloadToStreamAsync(Blob blob, Stream target, CancellationToken cancellationToken)
        {
            using (var httpClient = CreateClient())
            using (var res =
                    await
                        httpClient.GetAsync(string.Format(Constants.GetUrl, ContainerName, blob.Name), cancellationToken)
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
            var httpClient = new HttpClient { BaseAddress = BaseAddress };
            httpClient.DefaultRequestHeaders.Add(Constants.RequestHeader, AccessKey);

            return httpClient;
        }
    }
}
