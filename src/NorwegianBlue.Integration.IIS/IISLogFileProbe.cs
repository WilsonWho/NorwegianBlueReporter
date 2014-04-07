using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NorwegianBlue.Util.Web;

namespace NorwegianBlue.Integration.IIS
{
    public class IisLogFileProbe
    {
        private const string DateTimeRegex = @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}";
        private const string IISLogHeaderRegex = @"#Fields: .*";
        private readonly List<string> _validLogs;
        private readonly IisLogSearchParameters _logSearchParameters;

        public IisLogFileProbe(IisLogSearchParameters logSearchParameters)
        {
            _validLogs = new List<string>();
            _logSearchParameters = logSearchParameters;
        }
        
        public List<string> CollectLogsFromTimeInterval(List<FileInfo> files)
        {
            if (files.Count == 0)
            {
                throw new ArgumentException("No files specified ...");
            }

            var headers = ExtractHeaders(files[0]);
            _validLogs.Add(headers);

            foreach (FileInfo file in files)
            {
                Crop(file);
            }

            return _validLogs;
        }

        private string ExtractHeaders(FileInfo file)
        {
            string headers = string.Empty;
            using (StreamReader reader = file.OpenText())
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    headers = ReadHeaders(line);

                    if (!string.IsNullOrEmpty(headers))
                    {
                        break;
                    }
                }

                reader.Close();
            }

            return headers;
        }

        private string ReadHeaders(string line)
        {
            var match = Regex.Match(line, IISLogHeaderRegex);
            var headers = match.Groups[0].Value;

            return headers;
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

            var startTimeCompare = dateTime.CompareTo(_logSearchParameters.StartTime);
            var endTimeCompare = dateTime.CompareTo(_logSearchParameters.EndTime);

            bool betweenStartAndEndTime = startTimeCompare == 1 && endTimeCompare == -1;
            bool isStartTime = startTimeCompare == 0;
            bool isEndTime = endTimeCompare == 0;

            return betweenStartAndEndTime || isStartTime || isEndTime;
        }
    }
}