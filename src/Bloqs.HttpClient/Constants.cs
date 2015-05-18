namespace Bloqs.Http.Net
{
    internal class Constants
    {
        /// <summary>X-Bloqs-API-Key</summary>
        public const string RequestHeader = "X-Bloqs-API-Key";

        /// <summary></summary>
        public const string GetUrl = "blob/{0}/{1}";

        /// <summary></summary>
        public const string GetAdditionalUrl = "blob/{0}/{1}/attribute";

        /// <summary></summary>
        public const string PostUrl = "blob/{0}/save";
    }
}
