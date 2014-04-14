using System;
using System.Collections.Generic;
using NorwegianBlue.Samples;
using NorwegianBlue.Util.Configuration;
using NorwegianBlue.Util.Web;

namespace NorwegianBlue.Integration.IIS.Samples
{
    public class IisSampleSet : CommonSampleSetBase<IisSample>
    {

        public IisSampleSet()
        {
            Dictionary<object, object> configuration = YamlParser.GetConfiguration();

            AnalysisScratchPad.ignorableFields = configuration.ContainsKey("FieldsToIgnore")
                                                     ? configuration["FieldsToIgnore"]
                                                     : new List<string>();
        }

        public override List<IisSample> DoParse(TimeZone desiredTimeZone, DateTime? startTime, DateTime? endTime,
                                                 dynamic dataSourceConfigObject)
        {
            Dictionary<object, object> configuration = YamlParser.GetConfiguration();
            
            var url = configuration["ServerUrl"].ToString();
            var configCredentials = (IDictionary<object, object>) configuration["Credentials"];
            var creds = new FtpCredentials
                {
                    Username = configCredentials["Username"].ToString(),
                    Password = configCredentials["Password"].ToString()
                };
            
            var ftpClient = new FtpClient(url, creds);
            ftpClient.Connect();
            ftpClient.ChangeWorkingDirectory(configuration["WorkingDirectory"].ToString());
            var localSaveFiles = ftpClient.PullLogFiles();
            
            var searchParameters = new IisLogSearchParameters
                {
                    StartTime = startTime,
                    EndTime = endTime
                };
            
            var fileProbe = new IisLogFileProbe(searchParameters);
            var logs = fileProbe.CollectLogsFromTimeInterval(localSaveFiles);
            
            var headers = logs[0].Split(null);
            logs.RemoveAt(0);

            var iisSamples = new List<IisSample>();

            foreach (var log in logs)
            {
                var logData = log.Split();
                var fields = new List<Tuple<string, string>>();
                var date = string.Empty;
                var time = string.Empty;

                for (var index = 0; index < logData.Length; index++)
                {
                    var header = headers[index + 1];
                    var field = logData[index];
                    if ("date" == header)
                    {
                        date = field;
                    }
                    else if ("time" == header)
                    {
                        time = field;
                    }
                    else
                    {
                        fields.Add(new Tuple<string, string>(header, field));
                    }
                }

                var dateTimeString = string.Format("{0} {1}", date, time);
                var timeStamp = Convert.ToDateTime(dateTimeString);

                var iisSample = new IisSample(timeStamp, fields);

                iisSamples.Add(iisSample); 
            }

            return iisSamples;

        }

    }
}
