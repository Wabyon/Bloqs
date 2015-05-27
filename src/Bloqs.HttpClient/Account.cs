using System;

namespace Bloqs.Http.Net
{
    public sealed class Account
    {
        internal const string RequestHeader = "X-Bloqs-API-Key";
        private const string ContainerReference = "api/{0}/attributes";
        private const string ContainerList = "api/containers/list";
        private const string ContainerCreate = "api/{0}/create";
        private const string BlobImage = "{0}/{1}";
        private const string BlobReference = "api/{0}/{1}/attributes";
        private const string BlobList = "api/{0}/blobs/list";
        private const string BlobSave = "api/{0}/save";

        private readonly Credentials _credentials;
        private readonly Uri _baseAddress;

        private Uri StorageEndpointUri
        {
            get { return new Uri(_baseAddress, _credentials.AccountName); }
        }

        public Account(Credentials credentials, string baseAddress)
            : this(credentials, new Uri(baseAddress))
        {
        }

        public Account(Credentials credentials, Uri baseAddress)
        {
            if (credentials == null) throw new ArgumentNullException("credentials");
            if (baseAddress == null) throw new ArgumentNullException("baseAddress");
            _credentials = credentials;
            _baseAddress = baseAddress;
        }

        public Client CreateClient()
        {
            return new Client(StorageEndpointUri, _credentials);
        }

        internal static Uri ContainerReferenceEndpoint(Uri accountBaseAddress, string containerName)
        {
            return CombineUri(accountBaseAddress, string.Format(ContainerReference, containerName));
        }

        internal static Uri ContainerListEndpoint(Uri accountBaseAddress)
        {
            return CombineUri(accountBaseAddress, ContainerList);
        }

        internal static Uri ContainerCreateEndpoint(Uri accountBaseAddress, string containerName)
        {
            return CombineUri(accountBaseAddress, string.Format(ContainerCreate, containerName));
        }

        internal static Uri BlobImageEndpoint(Uri accountBaseAddress, string containerName, string blobName)
        {
            return CombineUri(accountBaseAddress, string.Format(BlobImage, containerName, blobName));
        }

        internal static Uri BlobReferenceEndpoint(Uri accountBaseAddress, string containerName, string blobName)
        {
            return CombineUri(accountBaseAddress, string.Format(BlobReference, containerName, blobName));
        }

        internal static Uri BlobListEndpoint(Uri accountBaseAddress, string containerName)
        {
            return CombineUri(accountBaseAddress, string.Format(BlobList, containerName));
        }

        internal static Uri BlobSaveEndpoint(Uri accountBaseAddress, string containerName)
        {
            return CombineUri(accountBaseAddress, string.Format(BlobSave, containerName));
        }

        private static Uri CombineUri(Uri baseAddress, string path)
        {
            // don't use system.web...
            var uri1 = baseAddress.OriginalString.TrimEnd('/');
            var uri2 = path.TrimStart('/');
            return new Uri(string.Format("{0}/{1}", uri1, uri2));
        }
    }
}
