using System;
using System.Collections.Generic;
using NorwegianBlue.Data.Sample;

namespace NorwegianBlue.Data.SampleSet
{
    public class SampleSetComparisons<T> where T: ISampleValues
    {
        public static T GetNearestToTime(IEnumerable<T> samples, DateTime time,
                                                     TimeSpan tolerance, bool absolute)
        {
            T closest = default(T);
            var smallestDelta = TimeSpan.MaxValue;

            foreach (var sample in samples)
            {
                var delta = sample.TimeStamp - time;
                if (absolute && (delta < TimeSpan.Zero))
                {
                    delta = -delta;
                }

                if (delta < smallestDelta)
                {
                    smallestDelta = delta;
                    closest = sample;
                }
            }

            if (smallestDelta > tolerance)
            {
                closest = default(T);
            }
            return closest;
        }

        public static T GetNearestToTime(IEnumerable<T> samples, DateTime time)
        {
            return GetNearestToTime(samples, time, TimeSpan.Zero, false);
        }
    }
}
