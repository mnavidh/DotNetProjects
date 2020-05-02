using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Xml;


namespace ConvertExcelToXml
{
    class Program
    {
        static void Main(string[] args)
        {

            XmlDocument myXmlDocument = new XmlDocument();
            myXmlDocument.Load(@"D:\IFQASYS 6-1-12.xlsx");

        }


    }
}
