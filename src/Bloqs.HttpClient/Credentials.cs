using System;

namespace Bloqs.Http.Net
{
    /// <summary>Storage account credentials</summary>
    public class Credentials
    {
        /// <summary>Storage account name</summary>
        public string AccountName { get; private set; }

        /// <summary>Storage access key</summary>
        public string AccessKey { get; private set; }

        /// <summary></summary>
        /// <param name="accountName"></param>
        /// <param name="accessKey"></param>
        public Credentials(string accountName, string accessKey)
        {
            if (string.IsNullOrWhiteSpace(accountName)) throw new InvalidOperationException();
            if (string.IsNullOrWhiteSpace(accessKey)) throw new InvalidOperationException();

            AccountName = accountName;
            AccessKey = accessKey;
        }
    }
}