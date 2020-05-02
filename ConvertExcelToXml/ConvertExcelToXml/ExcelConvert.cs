using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace ConvertExcelToXml
{
    public class ExcelConvert
    {
        private static Application ExcelApp = new Application();

        public static MemoryStream ExcelStreamToXML(string tempFolderPath, Stream InExcelStream)
        {
            string guidString = System.Guid.NewGuid().ToString();
            string XLSXfilePath = tempFolderPath + guidString + ".Bizxlsx";
            string XMLfilePath = tempFolderPath + guidString + ".Bizxml";
            FileStream fileStrm = null;
            BinaryReader br = null;

            try
            {
                //Save Stream into Flat File
                fileStrm = new FileStream(XLSXfilePath, FileMode.Create);
                int Length = (int)InExcelStream.Length;
                br = new BinaryReader(InExcelStream);
                fileStrm.Write(br.ReadBytes(Length), 0, Length);
                fileStrm.Close();

                #region Convert Excel To XML
                Workbook ExcelWorkbook = ExcelApp.Workbooks.Open(XLSXfilePath, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing);
                if (1 == ExcelWorkbook.XmlMaps.Count)
                {
                    ExcelWorkbook.SaveAsXMLData(XMLfilePath, ExcelWorkbook.XmlMaps[1]);

                }
                ExcelApp.Workbooks.Close();
                ExcelApp.Quit();
                ExcelWorkbook = null;
                #endregion

                byte[] bytes = File.ReadAllBytes(XMLfilePath);

                #region Delete Temp Files
                if (File.Exists(XLSXfilePath))
                {
                    File.Delete(XLSXfilePath);
                }
                if (File.Exists(XMLfilePath))
                {
                    File.Delete(XMLfilePath);
                }
                #endregion

                return new MemoryStream(bytes);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (null != fileStrm)
                {
                    fileStrm = null;
                }
                if (null != InExcelStream)
                {
                    InExcelStream = null;
                }
                if (null != br)
                {
                    br = null;
                }
            }
        }
    }
}