using System;

namespace NorwegianBlue.Util.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Missing code, saved elsewhere for now

            var ftpClient = new Ftp(url, creds);
            ftpClient.Connect();
            // Missing code, saved elsewhere for now
            var localSaveFiles = ftpClient.DownloadFiles();

            var logSearchParameters = new IISLogSearchParameters
            {
                StartTime = new DateTime(2014, 3, 12, 18, 10, 0),
                EndTime = new DateTime(2014, 3, 12, 20, 11, 0)
            };

            var fileProbe = new IISLogFileProbe(logSearchParameters);
            var validLogs = fileProbe.CollectLogsFromTimeInterval(localSaveFiles);
        }
    }
}