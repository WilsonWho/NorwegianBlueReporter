using System;
using System.Collections.Generic;

namespace NorwegianBlue.Data.Sample
{
    public class SampleTimeComparer<T> : IComparer<T> where T : ISampleValues
    {
        public int Compare(T x, T y)
        {
            return DateTime.Compare(x.TimeStamp, y.TimeStamp);
        }
    }
}
