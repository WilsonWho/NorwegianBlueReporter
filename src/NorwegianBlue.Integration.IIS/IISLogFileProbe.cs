using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NorwegianBlue.Util.Web;

namespace NorwegianBlue.Integration.IIS
{
    public class IISLogFileProbe
    {
        private const string DateTimeRegex = @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}";
        private readonly List<string> _validLogs;
        private readonly IISLogSearchParameters _logSearchParameters;

        public IISLogFileProbe(IISLogSearchParameters logSearchParameters)
        {
            _validLogs = new List<string>();
            _logSearchParameters = logSearchParameters;
        }
        
        public List<string> CollectLogsFromTimeInterval(List<FileInfo> files)
        {
            foreach (FileInfo file in files)
            {
                Crop(file);
            }

            return _validLogs;
        }

        private void Crop(FileInfo file)
        {
            using (StreamReader reader = file.OpenText())
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (CheckIfLineQualifies(line))
                    {
                        _validLogs.Add(line);
                    }
                }
            }
        }

        private bool CheckIfLineQualifies(string line)
        {
            var match = Regex.Match(line, DateTimeRegex);
            var dateString = match.Groups[0].Value;

            return !string.IsNullOrEmpty(dateString) && CompareDateTime(dateString);
        }

        private bool CompareDateTime(string dateString)
        {
            var dateTime = Convert.ToDateTime(dateString);

            var startCompare = dateTime.CompareTo(_logSearchParameters.StartTime);
            var endCompare = dateTime.CompareTo(_logSearchParameters.EndTime);

            return startCompare == 1 && endCompare == -1;
        }
    }
}