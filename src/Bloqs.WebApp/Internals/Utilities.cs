using System.Configuration;

namespace Bloqs.Internals
{
    public static class Utilities
    {
        public static readonly string ApiAddress = ConfigurationManager.AppSettings["bloqs:ApiAddress"];

        public static string CreateApiAddress(string account, string container = null, string blob = null)
        {
            string apiAddress;
            if (!ApiAddress.EndsWith("/")) apiAddress = ApiAddress + "/" + account;
            else apiAddress = ApiAddress + account;

            if (container == null) return apiAddress;

            apiAddress += "/" + container;

            if (blob == null) return apiAddress;

            return apiAddress + ("/" + blob);
        }

        public static string FormatSizeString(long size)
        {
            string[] suffix = { "", "K", "M", "G", "T", "P", "E", "Z", "Y" };
            var index = 0;

            while (size >= 1024)
            {
                size /= 1024;
                index++;
            }

            return string.Format(
                "{0} {1}B",
                size.ToString("N1"),
                index < suffix.Length ? suffix[index] : "-");
        }
    }
}