using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace FTPFileDownload
{
    class Utilities
    {
        const string HTML_TAG_PATTERN = "<.*?>";

        public Utilities()
        {
        }

        public static string GetFileNamePattern(string fileName, int dateDiff)
        {
            try
            {
                string filePattern;
                DateTime dt = DateTime.Today.AddDays(dateDiff);
                filePattern = String.Format("{0:yyyyMMdd}", dt);
                return fileName + filePattern;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public static void DecompressFile(FTPDetail ftp)
        {
            try
            {
                string srcFile = ftp.DownLoadPath + "\\" + GetFileNamePattern(ftp.FTPFileFormat, ftp.DateDiff) + ftp.FTPFileExtn;
                string destFile = ftp.DownLoadPath + "\\" + GetFileNamePattern(ftp.DownloadFileFormat, ftp.DateDiff) + ftp.DownloadFileExtn;
                Process p = Process.Start(Environment.CurrentDirectory + "\\DECOMP.EXE", srcFile + " " + destFile);
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }        

        public static void HandleException(Exception ex, string userMsg)
        {
            try
            {
                string errMsg = "";

                errMsg += userMsg + "<br /><br /> <b>General Exception</b> - " + ex.ToString() + "<br /><br />";

                if (ex.InnerException != null)
                {
                    errMsg += "<b>Inner exception</b> - " + ex.InnerException.ToString() + "<br /><br />";
                }

                if (ex.StackTrace != null)
                {
                    errMsg += "<b>Stack Trace</b> - " + ex.StackTrace.ToString();
                }

                LogMessage(errMsg);

                SendEmail(errMsg, ConfigurationManager.AppSettings["ErrorEmailSubject"]);
            }
            catch (Exception exp)
            {
                LogMessage(exp.ToString());
            }
        }

        public static void SendEmail(string emailMsg, string emailSubject)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["SMTPAddress"]);
                mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"]);
                mail.To.Add(ConfigurationManager.AppSettings["EmailTo"]);
                mail.Subject = emailSubject;
                mail.IsBodyHtml = true;
                mail.Body = emailMsg;
                SmtpServer.Send(mail);
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }

        public static void LogMessage(string Msg)
        {
            try
            {
                string logFilePath = Environment.CurrentDirectory + "\\" + "logs";                
                string logFileName = String.Format("{0:yyyyMMdd}",DateTime.Now);
                string logFileFullPath = logFilePath + "\\" + logFileName + ".txt";

                if(!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }

                if (!File.Exists(logFileFullPath))
                {
                    using (FileStream fs = File.Create(logFileFullPath))
                    {
                    }
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@logFileFullPath, true))
                {                    
                    file.WriteLine(DateTime.Now.ToString() + " - " + StripHTML(Msg));
                    file.WriteLine();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static string StripHTML(string strWithHTML)
        {
            string cleanString = "";

            try
            {
                cleanString = Regex.Replace(strWithHTML, HTML_TAG_PATTERN, string.Empty);
            }
            catch(Exception ex)
            {
                LogMessage("Stripping HTML tags from string failed with the error - " + ex.ToString());
                cleanString = strWithHTML;
            }

            return cleanString;
        }
    }
}