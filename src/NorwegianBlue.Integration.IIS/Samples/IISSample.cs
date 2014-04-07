using System;
using NorwegianBlue.Samples;

namespace NorwegianBlue.Integration.IIS.Samples
{
    public class IisSample : CommonSampleBase
    {

        public void Parse(string log, string[] headers)
        {
            var logData = log.Split(null);

            var date = string.Empty;
            var time = string.Empty;
            for (var index = 0; index < logData.Length; index++)
            {
                var header = headers[index + 1];
                var logContent = logData[index];

                if (string.Equals(header, "date"))
                {
                    date = logContent;
                }
                else if (string.Equals(header, "time"))
                {
                    time = logContent;
                }
                else
                {
                    AddParsedData(header, logContent);
                }
            }

            string dateTimeString = string.Format("{0} {1}", date, time);
            TimeStamp = Convert.ToDateTime(dateTimeString);
        }
    }
}