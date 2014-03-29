using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.FtpClient;

namespace NorwegianBlue.Util.Web
{
    public class FtpClient
    {
        private string _serverUri;
        private readonly FtpCredentials _ftpCredentials;
        private readonly System.Net.FtpClient.FtpClient _ftpClient;

        public FtpClient(string serverUri, FtpCredentials ftpCredentials)
        {
            _serverUri = serverUri;
            _ftpCredentials = ftpCredentials;
            _ftpClient = new System.Net.FtpClient.FtpClient();
        }

        public void SetServerUri(string serverUri)
        {
            _serverUri = serverUri;
        }

        public List<FileInfo> DownloadLogFiles(string directory)
        {
            var localSaveFiles = new List<FileInfo>();

            using (_ftpClient)
            {
                var files = _ftpClient.GetListing(_ftpClient.GetWorkingDirectory(), FtpListOption.AllFiles);

                localSaveFiles.AddRange(from file in files
                                        where file.Type == FtpFileSystemObjectType.File
                                        select SaveToFile(directory, file));

                _ftpClient.Disconnect();
            }

            return localSaveFiles;
        }

        public List<FileInfo> PullLogFiles()
        {
            //if (_serverUri.Scheme != Uri.UriSchemeFtp)
            //{
            //    throw new UriFormatException("Uri is not in FTP format ...");
            //}

            var localSaveFiles = new List<FileInfo>();

            using (_ftpClient)
            {
                var ftpCwd = _ftpClient.GetWorkingDirectory();
                var files = _ftpClient.GetListing(ftpCwd);

                localSaveFiles.AddRange(from file in files
                                    where file.Type == FtpFileSystemObjectType.File
                                    select SaveToTemporaryFile(file));

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
            // Using forced passive as we had trouble with double NAT 
            // (eg windows running in a VM with a shared (NAT) network setting.
            _ftpClient.DataConnectionType = FtpDataConnectionType.PASV;

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

        private FileInfo SaveToFile(string directory, FtpListItem ftpListItem)
        {
            string path = Path.Combine(directory, ftpListItem.Name);
            WriteToFile(ftpListItem.FullName, path);

            return new FileInfo(path);
        }

        private FileInfo SaveToTemporaryFile(FtpListItem ftpListItem)
        {
            var tmp = Path.GetTempFileName();
            WriteToFile(ftpListItem.FullName, tmp);

            return new FileInfo(tmp);
        }

        private void WriteToFile(string src, string dest)
        {
            using (Stream ftpStream = _ftpClient.OpenRead(src))
            {
                using (FileStream localStream = File.OpenWrite(dest))
                {
                    var bytesInStream = new byte[ftpStream.Length];
                    ftpStream.Read(bytesInStream, 0, bytesInStream.Length);
                    localStream.Write(bytesInStream, 0, bytesInStream.Length);
                    localStream.Close();
                }

                ftpStream.Close();
            }
        }
    }
}