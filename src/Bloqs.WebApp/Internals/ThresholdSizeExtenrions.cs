using System;
using Bloqs.Models;

namespace Bloqs.Internals
{
    internal static class ThresholdSizeExtenrions
    {
        public static long ToInt64(this ThresholdSize thresholdSize)
        {
            Int64 longLength;

            switch (thresholdSize)
            {
                case ThresholdSize.OneHundredMegabytes:
                    longLength = 100*1024*1024;
                    break;
                case ThresholdSize.TwoHundredMegabytes:
                    longLength = 200*1024*1024;
                    break;
                case ThresholdSize.FiveHundredMegabytes:
                    longLength = 500*1024*1024;
                    break;
                case ThresholdSize.OneGigabytes:
                    longLength = 1024*1024*1024;
                    break;
                default:
                    longLength = 0;
                    break;
            }

            return longLength;
        }
    }
}