using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;


namespace HSTConversion
{
    class Program
    {
        static void Main(string[] args)
        {

            AccessSabreWebService.AccessSabreWebService ws = new AccessSabreWebService.AccessSabreWebService();
            string arrAirportCode = string.Empty;
            string depAirportCode = string.Empty;
            string securityToken = string.Empty;
            DateTime localDepDateTime = new DateTime();
            DateTime localArrDateTime = new DateTime();
            DateTime localDepDateTimeHST = new DateTime();
            DateTime localArrDateTimeHST = new DateTime();
            string sabreResponseHSTDepDateTime = string.Empty;
            string sabreResponseHSTArrDateTime = string.Empty;
            //string storetoDBDepTime = string.Empty;
            //string storetoDBArrTime = string.Empty;

            DateTime storetoDBDepTime = new DateTime();
            DateTime storetoDBArrTime = new DateTime();


            object matchText;
            int hours;
            int localHours;

            ws.PreAuthenticate = true;
            ws.Credentials = System.Net.CredentialCache.DefaultCredentials;
            securityToken = ws.InitialSetup("").ToString();

            //airportCode = row["Flight_Departure_Airport"].ToString();
            arrAirportCode = "HNL";
            localDepDateTime = Convert.ToDateTime("2013-01-31 15:10:00.000");
            sabreResponseHSTDepDateTime = ws.getHSTTimeDiff(arrAirportCode, localDepDateTime.ToString(), securityToken);
            if (sabreResponseHSTDepDateTime.Contains("P"))
            {
                matchText = Regex.Match(sabreResponseHSTDepDateTime, @"\d{3,4}");
                if (Convert.ToString(matchText).Length > 3)
                    hours = Convert.ToInt16(matchText.ToString().Substring(0, 2)) + 12;
                else
                    hours = Convert.ToInt16(matchText.ToString().Substring(0, 1)) + 12;
                matchText = Regex.Match(sabreResponseHSTDepDateTime, @"\s+[+-]\d");
                if (Convert.ToString(matchText).Length > 0)
                    localDepDateTime = localDepDateTime.AddDays(Convert.ToDouble(matchText.ToString().Trim()));
                localHours = localDepDateTime.Hour;
                if (localHours > hours)
                {
                    hours = localHours - hours;
                    localDepDateTimeHST = localDepDateTime.AddHours(-Convert.ToDouble(hours.ToString()));
                }
                else if (localHours < hours)
                {
                    hours = hours - localHours;
                    localDepDateTimeHST = localDepDateTime.AddHours(Convert.ToDouble(hours.ToString()));
                }
                else
                    localDepDateTimeHST = localDepDateTime;
                //localDepDateTimeHST = localDepDateTime.Date.Add(new TimeSpan(hours, minutes, 0));

            }
            else if (sabreResponseHSTDepDateTime.Contains("A"))
            {
                matchText = Regex.Match(sabreResponseHSTDepDateTime, @"\d{3,4}");
                if (Convert.ToString(matchText).Length > 3)
                    hours = Convert.ToInt16(matchText.ToString().Substring(0, 2));
                else
                    hours = Convert.ToInt16(matchText.ToString().Substring(0, 1));
                matchText = Regex.Match(sabreResponseHSTDepDateTime, @"\s+[+-]\d");
                if (Convert.ToString(matchText).Length > 0)
                    localDepDateTime = localDepDateTime.AddDays(Convert.ToDouble(matchText.ToString().Trim()));
                localHours = localDepDateTime.Hour;
                if (localHours > hours)
                {
                    hours = localHours - hours;
                    localDepDateTimeHST = localDepDateTime.AddHours(-Convert.ToDouble(hours.ToString()));
                }
                else if (localHours < hours)
                {
                    hours = hours - localHours;
                    localDepDateTimeHST = localDepDateTime.AddHours(Convert.ToDouble(hours.ToString()));
                }
                else
                    localDepDateTimeHST = localDepDateTime;
            }
            storetoDBDepTime= localDepDateTimeHST;
            localArrDateTime = Convert.ToDateTime("2013-01-31 22:30:00.000");
            depAirportCode = "LAX";

            sabreResponseHSTArrDateTime = ws.getHSTTimeDiff(depAirportCode, localArrDateTime.ToString(), securityToken);
            if (sabreResponseHSTArrDateTime.Contains("P"))
            {
                matchText = Regex.Match(sabreResponseHSTArrDateTime, @"\d{3,4}");
                if (Convert.ToString(matchText).Length > 3)
                    hours = Convert.ToInt16(matchText.ToString().Substring(0, 2)) + 12;
                else
                    hours = Convert.ToInt16(matchText.ToString().Substring(0, 1)) + 12;
                matchText = Regex.Match(sabreResponseHSTArrDateTime, @"\s+[+-]\d");
                if (Convert.ToString(matchText).Length > 0)
                    localArrDateTime = localArrDateTime.AddDays(Convert.ToDouble(matchText.ToString().Trim()));
                localHours = localArrDateTime.Hour;
                if (localHours > hours)
                {
                    hours = localHours - hours;
                    localArrDateTimeHST = localArrDateTime.AddHours(-Convert.ToDouble(hours.ToString()));
                }
                else if (localHours < hours)
                {
                    hours = hours - localHours;
                    localArrDateTimeHST = localArrDateTime.AddHours(Convert.ToDouble(hours.ToString()));
                }
                else
                    localArrDateTimeHST = localArrDateTime;
            }
            else if (sabreResponseHSTArrDateTime.Contains("A"))
            {
                matchText = Regex.Match(sabreResponseHSTArrDateTime, @"\d{3,4}");
                if (Convert.ToString(matchText).Length > 3)
                    hours = Convert.ToInt16(matchText.ToString().Substring(0, 2));
                else
                    hours = Convert.ToInt16(matchText.ToString().Substring(0, 1));
                matchText = Regex.Match(sabreResponseHSTArrDateTime, @"\s+[+-]\d");
                if (Convert.ToString(matchText).Length > 0)
                    localArrDateTime = localArrDateTime.AddDays(Convert.ToDouble(matchText.ToString().Trim()));
                localHours = localArrDateTime.Hour;
                if (localHours > hours)
                {
                    hours = localHours - hours;
                    localArrDateTimeHST = localArrDateTime.AddHours(-Convert.ToDouble(hours.ToString()));
                }
                else if (localHours < hours)
                {
                    hours = hours - localHours;
                    localArrDateTimeHST = localArrDateTime.AddHours(Convert.ToDouble(hours.ToString()));
                }
                else
                    localArrDateTimeHST = localArrDateTime;
            }
            storetoDBArrTime = localArrDateTimeHST;
            //row.AcceptChanges();
           
       
        }
    }
}
