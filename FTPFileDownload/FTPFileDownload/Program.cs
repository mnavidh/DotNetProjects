using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace FTPFileDownload
{
    class Program
    {
        static void Main(string[] args)
        {
            List<FTPDetail> ftpDetails = new List<FTPDetail>();
            bool filePresentInFTP = false;            

            try
            {
                ftpDetails = FTPDetail.ReadFTPDetailsfromXML();
            }
            catch (Exception ex)
            {                
                Utilities.HandleException(ex, "Reading FTP details from XML failed with the below error");
                Environment.Exit(1);
            }

            foreach (FTPDetail ftp in ftpDetails)
            {
                string fileToDwnld = Utilities.GetFileNamePattern(ftp.FTPFileFormat, ftp.DateDiff) + ftp.FTPFileExtn;

                try
                {
                    filePresentInFTP = FTPProcess.CheckFileInFTP(ftp);
                }
                catch (Exception ex)
                {
                    Utilities.HandleException(ex,"Checking for file in FTP failed with the below error");
                    Environment.Exit(1);
                }

                if (filePresentInFTP)
                {
                    try
                    {
                        FTPProcess.DownloadFileFromFTP(ftp);
                    }
                    catch (Exception ex)
                    {
                        Utilities.HandleException(ex, "Downloading the file from FTP failed with the below error");
                        Environment.Exit(1);
                    }

                    DirectoryInfo di = new DirectoryInfo(@ftp.DownLoadPath);
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        if (ftp.Decompress)
                        {
                            try
                            {
                                Utilities.DecompressFile(ftp);
                            }
                            catch (Exception ex)
                            {
                                Utilities.HandleException(ex, "File decompression failed with the below error");
                                Environment.Exit(1);
                            }
                        }
                    }
                }
                else
                {
                    string notificationMsg = "The file <b>" + fileToDwnld + "</b> is not available in the FTP server for download.";
                    Utilities.LogMessage(notificationMsg);
                    Utilities.SendEmail(notificationMsg,ConfigurationManager.AppSettings["FileMissingEmailSubject"]);
                }
            }            
        }

    }
}