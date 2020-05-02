using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.ServiceProcess;
using System.Net.Mail;

namespace MoveLogs
{
    class Program
    {
        static void Main(string[] args)
        {

            StringBuilder sb = new StringBuilder();
            //bool processFile = false;

            try
            {
                StopIIS();
            }
            catch (Exception ex)
            {                
                //Console.WriteLine("IIS Stop Failed");
                //Console.WriteLine("Error - " + ex.ToString());
                
                sb.AppendLine("IIS Stop Failed");
                sb.AppendLine("Error - " + ex.ToString());
                SendEmailAlert(sb.ToString());
                return;
            }

            try
            {
                ProcessFile();
                //processFile = true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Log archiving Failed");
                //Console.WriteLine("Error - " + ex.ToString());
                sb.AppendLine("Log archiving Failed");
                sb.AppendLine("Error - " + ex.ToString());
                SendEmailAlert(sb.ToString());                
            }
           

            try
            {
                //if (processFile)
                //{
                    StartIIS();
                //}
            }
            catch (Exception ex)
            {
                //Console.WriteLine("IIS Start Failed");
                //Console.WriteLine("Error - " + ex.ToString());
                sb.AppendLine("IIS Start Failed");
                sb.AppendLine("Error - " + ex.ToString());
                SendEmailAlert(sb.ToString());
                
            }
            

            // Keep console window open in debug mode.
            //Console.WriteLine("Press any key to exit.");
            //Console.ReadKey();

        }

        private static void StopIIS()
        {
            ServiceController iis = new ServiceController("W3SVC");
            if (null != iis)
            {
                do
                {
                    iis.Refresh();
                }
                while
                    (
                    iis.Status ==
                    ServiceControllerStatus.ContinuePending ||
                    iis.Status ==
                    ServiceControllerStatus.PausePending ||
                    iis.Status ==
                    ServiceControllerStatus.StartPending ||
                    iis.Status ==
                    ServiceControllerStatus.StopPending
                    );
                if (ServiceControllerStatus.Running ==
                    iis.Status ||
                    ServiceControllerStatus.Paused == iis.Status)
                {
                    iis.Stop();
                    iis.WaitForStatus(
                        ServiceControllerStatus.Stopped);
                }
                iis.Close();
            }
        }

        private static void StartIIS()
        {
            ServiceController iis = new ServiceController("W3SVC");            
            if (null != iis)
            {
                do
                {
                    iis.Refresh();
                }
                while
                    (
                    iis.Status ==
                    ServiceControllerStatus.ContinuePending ||
                    iis.Status ==
                    ServiceControllerStatus.PausePending ||
                    iis.Status ==
                    ServiceControllerStatus.StartPending ||
                    iis.Status ==
                    ServiceControllerStatus.StopPending
                    );
                if (ServiceControllerStatus.Stopped == iis.Status)
                {
                    iis.Start();
                    iis.WaitForStatus(
                        ServiceControllerStatus.Running);
                }
                else
                {
                    if (ServiceControllerStatus.Paused ==
                        iis.Status)
                    {
                        iis.Continue();
                        iis.WaitForStatus(
                            ServiceControllerStatus.Running);
                    }
                }
                iis.Close();
            }
        }

        private static void ProcessFile()
        {
            string sourceFileName = ConfigurationManager.AppSettings["LogFileName"].ToString();
            string sourcePath = @ConfigurationManager.AppSettings["SourcePath"].ToString();
            string targetPath = @ConfigurationManager.AppSettings["DestinationPath"].ToString();
            string destinationFileName = "MilesAdminLogs_" + DateTime.Now.ToString("MMMdd_yyyy_HHmm") + ".log";

            string sourceFile = System.IO.Path.Combine(sourcePath, sourceFileName);
            string destFile = System.IO.Path.Combine(targetPath, destinationFileName);

            
            File.Move(sourceFile, destFile);

        }

        private static void SendEmailAlert(string EmailContent)
        {
            try
            {
                string fromAddress = ConfigurationManager.AppSettings["EmailFrom"].ToString();
                string toAddress = ConfigurationManager.AppSettings["EmailTo"].ToString();
                string mailSubject = ConfigurationManager.AppSettings["EmailSubject"].ToString();

                MailMessage mailObj = new MailMessage(fromAddress, toAddress, mailSubject, EmailContent);
                SmtpClient mailClient = new SmtpClient(ConfigurationManager.AppSettings["SMTP"].ToString());
                mailClient.Send(mailObj);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email Alert Failed - " + ex.ToString());
                Console.WriteLine("Error - " + EmailContent);
                Console.ReadLine();
            }

        }
    }
}
