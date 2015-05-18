using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bloqs
{
    public class Container
    {
        public string Id { get; private set; }

        public string Name { get; set; }

        public bool IsPublic { get; set; }

        public string Owner { get; set; }

        public string PrimaryAccessKey { get; set; }

        public string SecondaryAccessKey { get; set; }

        public DateTime CreatedUtcDateTime { get; set; }

        public DateTime LastModifiedUtcDateTime { get; set; }

        public bool IsUsableName()
        {
            return IsUsableName(Name);
        }

        public Container()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        public static bool IsUsableName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (UnusableNames.Contains(name.ToLower())) return false;
            var regex = new Regex(UsableNamePattern);
            if (!regex.IsMatch(name)) return false;

            return true;
        }

        /// <summary>half-width alphanumeric characters and hyphens (-) only.
        /// [0-9a-zA-Z-]+</summary>
        public const string UsableNamePattern = @"[0-9a-zA-Z-]+";

        private static readonly IList<string> UnusableNames = new[]
        {
            "bloqs",
            "blob",
            "blobs",
            "container",
            "containers",
            "api"
        };
    }
}
