using System;

namespace Bloqs.Models
{
    public struct Size
    {
        public long OriginalLength;
        public long Length;
        public string Unit;
        private static readonly string[] UnitSuffix = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };


        public Size(long originalLength)
        {
            OriginalLength = originalLength;
            var index = 0;
            while (originalLength >= 1024)
            {
                originalLength /= 1024;
                index++;
            }

            Length = originalLength;
            Unit = string.Format("{0}", (index < UnitSuffix.Length ? UnitSuffix[index] : @"?B"));
        }

        public Size(long size, string unit)
        {
            Length = size;
            Unit = unit;

            var index = Array.IndexOf(UnitSuffix, unit.ToUpper());
            if (index <= 0) OriginalLength = size;
            else
            {
                OriginalLength = size * (long)Math.Pow(1024, index);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Length.ToString("N"), Unit);
        }
    }
}