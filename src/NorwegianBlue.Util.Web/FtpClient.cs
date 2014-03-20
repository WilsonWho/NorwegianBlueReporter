using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.FtpClient;

namespace NorwegianBlue.Util.Web
{
    public class Ftp
    {
        private string _serverUri;
        private readonly FtpCredentials _ftpCredentials;
        private readonly FtpClient _ftpClient;

        public Ftp(string serverUri, FtpCredentials ftpCredentials)
        {
            _serverUri = serverUri;
            _ftpCredentials = ftpCredentials;
            _ftpClient = new FtpClient();
        }

        public void SetServerUri(string serverUri)
        {
            _serverUri = serverUri;
        }

        public List<FileInfo> DownloadFiles()
        {
            //if (_serverUri.Scheme != Uri.UriSchemeFtp)
            //{
            //    throw new UriFormatException("Uri is not in FTP format ...");
            //}

            var localSaveFiles = new List<FileInfo>();

            using (_ftpClient)
            {
                var files = _ftpClient.GetListing(_ftpClient.GetWorkingDirectory(), FtpListOption.AllFiles);

                localSaveFiles.AddRange(from file in files
                                    where file.Type == FtpFileSystemObjectType.File
                                    select SaveToFile(file));

                _ftpClient.Disconnect();
            }

            return localSaveFiles;
        }

        public void Connect()
        {
            _ftpClient.Credentials = new NetworkCredential
            {
                UserName = _ftpCredentials.Username,
                Password = _ftpCredentials.Password,
            };
            _ftpClient.Host = _serverUri;
            _ftpClient.Port = 21;
            _ftpClient.DataConnectionType = FtpDataConnectionType.PORT;

            _ftpClient.Connect();
        }

        public void ChangeWorkingDirectory(string directory)
        {
            var reply = _ftpClient.Execute(@"CWD " + directory);

            if (!reply.Success)
            {
                throw new FtpCommandException(reply);
            }
        }

        private FileInfo SaveToFile(FtpListItem ftpListItem)
        {
            var tmp = Path.GetTempFileName();

            using (Stream ftpStream = _ftpClient.OpenRead(ftpListItem.FullName))
            {
                using (FileStream localStream = File.OpenWrite(tmp))
                {
                    var bytesInStream = new byte[ftpStream.Length];
                    ftpStream.Read(bytesInStream, 0, bytesInStream.Length);
                    localStream.Write(bytesInStream, 0, bytesInStream.Length);
                    localStream.Close();
                }

                ftpStream.Close();
            }

            return new FileInfo(tmp);
        }
    }
}