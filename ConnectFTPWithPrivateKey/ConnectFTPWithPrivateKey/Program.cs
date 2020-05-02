using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace ConnectFTPWithPrivateKey
{
    class Program
    {
        //How to use putty.exe and psftp.exe and .key file to connect to FTP?
        //http://support.hoststore.com/index.php?_m=knowledgebase&_a=viewarticle&kbarticleid=87
        
        static void Main(string[] args)
        {

            string downloadpath = "D:\\download";

            using (StreamWriter sw = new StreamWriter("ftpcmds.txt", false))
            {
                sw.WriteLine("cd /DSA200/arastkdm/tkm/hawaii2/current/");
                sw.WriteLine("lcd \"" + downloadpath + "\"");
                sw.WriteLine("mget 1S_HA_VIPC_20120204_0922*");
                sw.WriteLine("close");
                sw.WriteLine("bye");
                sw.WriteLine("exit");
                sw.Close();
            }

            Process p = new Process();

            string a = "-l tkm_hawaii2 -2 -i \"" + Environment.CurrentDirectory + "\\HA-Sabre_pri.key\" 151.193.132.144 -b \"" + Environment.CurrentDirectory + "\\ftpcmds.txt\"";
                      
            p = Process.Start("C:\\psftp.exe",a);

            //p = Process.Start(Environment.CurrentDirectory + "psftp");
            p.WaitForExit();
        }

        public static void FileDownload_From_SFTP(string strBatchFilePath, string strBatchFileDirectory)
        {
            Process objProcess = new Process();

            objProcess.StartInfo.WorkingDirectory = strBatchFileDirectory;
            objProcess.StartInfo.FileName = strBatchFilePath;
            objProcess.StartInfo.UseShellExecute = false;
            objProcess.StartInfo.RedirectStandardOutput = true;
            objProcess.StartInfo.RedirectStandardError = true;
            objProcess.StartInfo.CreateNoWindow = true;
            objProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            objProcess.EnableRaisingEvents = true;

            objProcess.Exited += ProcessExited;
            objProcess.Start();
            //'1000 millisecond = 1 second, 1 mintue = 60,000 milliseconds
            objProcess.WaitForExit(120000);

            if (!objProcess.HasExited)
            {
                objProcess.Kill();
                //'Invalid login name or password. Otherwise, file size is too large on SFTP             

            }
            else
            {
                string strOutput = objProcess.StandardOutput.ReadToEnd();
                string strError = objProcess.StandardError.ReadToEnd();

                objProcess.Close();
                
            }

        }

        private static void ProcessExited(object sender, System.EventArgs e)
        {
            //'MsgBox("Authentication Valid")

        }

    }
}
