using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace FTPFileDownload
{
    class FTPProcess
    {
        public FTPProcess()
        {
        }

        public static bool CheckFileInFTP(FTPDetail ftp)
        {
            bool isFilePresent = true;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftp.FTPServer + ":" + ftp.FTPPort + "/"));
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(ftp.FTPUser, ftp.FTPPassword);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                if (!reader.ReadToEnd().Contains(Utilities.GetFileNamePattern(ftp.FTPFileFormat, ftp.DateDiff)))
                {
                    isFilePresent = false;
                }

                //For testing
                //if (!reader.ReadToEnd().Contains("SABRE_HA_SCNACT_20090614_1703.CMP"))
                //{
                //    isFilePresent = false;
                //}

                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {                
                throw ex;
            }

            return isFilePresent;
        }

        public static void DownloadFileFromFTP(FTPDetail ftp)
        {

            try
            {                
                FtpWebRequest reqFTP;
                FileStream outputStream = new FileStream(ftp.DownLoadPath + "\\" + Utilities.GetFileNamePattern(ftp.FTPFileFormat,ftp.DateDiff) + ftp.FTPFileExtn, FileMode.Create);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftp.FTPServer + ":" + ftp.FTPPort + "/" + Utilities.GetFileNamePattern(ftp.FTPFileFormat, ftp.DateDiff) + ftp.FTPFileFormat));
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftp.FTPServer + ":" + ftp.FTPPort + "/" + "SABRE_HA_SCNACT_20090614_1703.CMP"));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftp.FTPUser, ftp.FTPPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);

                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}