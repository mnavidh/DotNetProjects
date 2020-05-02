using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace FTPFileDownload
{

    [Serializable()]
    public class FTPDetail
    {
        string _ftpServer;
        string _ftpUser;
        string _ftpPassword;
        string dwnldFileFrmt;
        string _downLoadPath;
        string _ftpFileFormat;
        int _ftpDatediff;
        string _ftpPort;
        bool _decompress;
        string _ftpFileExtn;
        string _dwnldFileExtn;

        public string DownloadFileExtn
        {
            get { return _dwnldFileExtn; }
            set { _dwnldFileExtn = value; }
        }

        public string FTPFileExtn
        {
            get { return _ftpFileExtn; }
            set { _ftpFileExtn = value; }
        }

        public bool Decompress
        {
            get { return _decompress; }
            set { _decompress = value; }
        }

        public string FTPPort
        {
            get { return _ftpPort; }
            set { _ftpPort = value; }
        }

        public int DateDiff
        {
            get { return _ftpDatediff; }
            set { _ftpDatediff = value; }
        }

  
        public string FTPServer
        {
            get { return _ftpServer; }
            set { _ftpServer = value; }
        }

     
        public string FTPUser
        {
            get { return _ftpUser; }
            set { _ftpUser = value; }
        }

     
        public string FTPPassword
        {
            get { return _ftpPassword; }
            set { _ftpPassword = value; }
        }


        public string DownloadFileFormat
        {
            get { return dwnldFileFrmt; }
            set { dwnldFileFrmt = value; }
        }

       
        public string DownLoadPath
        {
            get { return _downLoadPath; }
            set { _downLoadPath = value; }
        }

       
        public string FTPFileFormat
        {
            get { return _ftpFileFormat; }
            set { _ftpFileFormat = value; }
        }


        public static List<FTPDetail> ReadFTPDetailsfromXML()
        {
            List<FTPDetail> myFTPDetails = new List<FTPDetail>();

            XmlSerializer xs = new XmlSerializer(typeof(List<FTPDetail>));

            XmlReader xmr = XmlReader.Create(Environment.CurrentDirectory + "\\" + "FTPServers.xml");

            myFTPDetails = (List<FTPDetail>)xs.Deserialize(xmr);

            return myFTPDetails;
        }

    }

}