using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Configuration;

namespace LogFileMonitor
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                string file = ConfigurationManager.AppSettings["LogFilePath"].ToString();
                string searchDate = DateTime.Now.ToString("MM/dd/yyyy");
                string tempText = "";
                List<string> _errorDetail = new List<string>();
                string emailBody = "";

               file += "Exception_" + DateTime.Today.ToString("yyyyMMdd") + ".log";

               if (File.Exists(file))
               {

                   using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                   using (var reader = new StreamReader(fs, Encoding.Default))
                   //using (var reader = new StreamReader(file))
                   {
                       var hasSearchDate = false;
                       var hasTimeoutError = false;
                       while (!reader.EndOfStream)
                       {
                           var line = reader.ReadLine();
                           if (!hasSearchDate)
                           {
                               if (line.StartsWith(searchDate))
                               {
                                   tempText = line.ToString();

                                   if (Convert.ToDateTime(tempText) >= DateTime.Now.AddMinutes(-15) && Convert.ToDateTime(tempText) <= DateTime.Now)
                                   {
                                       hasSearchDate = true;
                                   }
                               }
                           }
                           else if (!hasTimeoutError)
                           {
                               if (line.StartsWith("Message : Timeout expired"))
                               {
                                   hasTimeoutError = true;
                                   tempText = tempText + " " + line.ToString();
                                   if (hasSearchDate && hasTimeoutError)
                                   {
                                       _errorDetail.Add(tempText);
                                   }
                                   hasSearchDate = false;
                                   hasTimeoutError = false;
                               }
                           }

                       }

                   }
               }

                if (_errorDetail != null && _errorDetail.Count > 0)
                {

                    foreach (string t in _errorDetail)
                    {
                        emailBody = emailBody + "<br />" + t;

                    }
                    string[] strToAddressCollection = ConfigurationManager.AppSettings["toEmail"].ToString().Split(',');

                    MailMessage _emailmsg = new MailMessage();
                    _emailmsg.From = new MailAddress(ConfigurationManager.AppSettings["fromEmail"].ToString());


                    foreach (string strToAddress in strToAddressCollection)
                    {
                        _emailmsg.To.Add(new MailAddress(strToAddress));
                    }


                    _emailmsg.IsBodyHtml = true;
                    _emailmsg.Body = emailBody;
                    _emailmsg.Subject = ConfigurationManager.AppSettings["emailSubject"].ToString();

                    SmtpClient mailSmtpClient = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"].ToString());

                    mailSmtpClient.Send(_emailmsg);

                    _emailmsg.Dispose();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Message.ToString());
                Console.Read();
            }
    }
    }

}
