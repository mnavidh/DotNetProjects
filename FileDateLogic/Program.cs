using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


namespace FileDateLogic
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dtdeptDate;
            DateTime fileDate;
           
            string strDate = "31DEC";
            string FileDateWithHyphen = "2013-02-01";

            string strdepDate = null;

            string date = strDate.Substring(0, strDate.Length - 3);
            string month = strDate.Substring(strDate.Length - 3, strDate.Length - date.Length);
            fileDate = DateTime.ParseExact(FileDateWithHyphen, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            fileDate = fileDate.AddDays(0);
            string strFileDateYear = GetFileDateYear(FileDateWithHyphen, "yyyy-MM-dd");
            int intFileDateMonth = Convert.ToUInt16(GetFileDateMonth(FileDateWithHyphen, "yyyy-MM-dd")); 

            dtdeptDate = DateTime.ParseExact(strFileDateYear + "-" + month + "-" + date, "yyyy-MMM-d", CultureInfo.InvariantCulture);
            
            //To check if 31DEC should be processed as previous year of the file date year
            if (strDate == "31DEC")
            {
                DateTime temp2 = dtdeptDate.AddYears(-1);
                TimeSpan tmp;               

                tmp = fileDate - temp2;

                if (tmp.Days == 1)
                {
                    dtdeptDate = temp2;
                }          
            }

            //To capture -1 day
            if (dtdeptDate < fileDate.AddDays(-1))
            {
                dtdeptDate = dtdeptDate.AddYears(1);
            }

            strdepDate = dtdeptDate.ToString("yyyy-MM-dd");

        }

        public static string GetFileDateYear(string strDate, string format)
        {
            string stryear = null;

            DateTime date = new DateTime();
            date = DateTime.ParseExact(strDate, format, null);
            stryear = Convert.ToInt16(date.Year).ToString();

            return stryear;
        }

        public static string GetFileDateMonth(string strDate, string format)
        {
            string month = null;

            DateTime date = new DateTime();
            date = DateTime.ParseExact(strDate, format, null);
            month = Convert.ToInt16(date.Month).ToString();

            return month;
        }
    }
}

