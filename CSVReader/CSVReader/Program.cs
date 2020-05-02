using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace CSVReader
{
    class Program
    {
        static void Main(string[] args)
        {
            
            List<ResultMap> rslt = new List<ResultMap>();

            List<ResultMap> actl = new List<ResultMap>();

            string[] linesChanges = File.ReadAllLines(@"C:\Users\mohamed_navidh\Desktop\Upload.csv");

            string[] linesActual = File.ReadAllLines(@"C:\Users\mohamed_navidh\Desktop\Actual.csv");

            foreach (string line in linesChanges)
            {
                
                    string[] vals = line.Split(',');
                    ResultMap rm = new ResultMap();
                    rm.CountryID = vals[0].Trim();
                    rm.CountryDesc = vals[1].Trim();
                    rslt.Add(rm);
                
            }

            foreach (string line in linesActual)
            {

                string[] vals = line.Split(',');
                ResultMap rm = new ResultMap();
                rm.CountryID = vals[0].Trim();
                rm.CountryDesc = vals[1].Trim();
                actl.Add(rm);

            }

            string newCountryQuery = CreateSQLScript(rslt);

            string updateMemberQuery = CreateMemberScript(rslt,actl);

            File.WriteAllText(@"C:\Users\mohamed_navidh\Desktop\CountryFinalScript_Countries.txt", newCountryQuery);

            File.WriteAllText(@"C:\Users\mohamed_navidh\Desktop\CountryFinalScript_Members.txt", updateMemberQuery);
        }

        public static string CreateSQLScript(List<ResultMap> input)
        {

            StringBuilder sb = new StringBuilder();
           

            foreach (ResultMap rm in input)
            {

                sb.AppendLine("IF NOT EXISTS(SELECT * FROM country where CountryID='" + rm.CountryID + "') INSERT INTO country VALUES('" + rm.CountryID + "','" + rm.CountryDesc + "') ELSE BEGIN UPDATE dbo.Country SET Description = '" + rm.CountryDesc + "' WHERE CountryID = '" + rm.CountryID + "' END GO");

                
            }

            return sb.ToString();

        }


        public static string CreateMemberScript(List<ResultMap> change, List<ResultMap> actual)
        {

            StringBuilder sb = new StringBuilder();


            foreach (ResultMap rm1 in change)
            {

                foreach (ResultMap rm2 in actual)
                {

                    if (rm1.CountryID == rm2.CountryID)
                    {
                        sb.AppendLine("UPDATE dbo.Member SET CountryID = '" + rm1.CountryID + "' WHERE CountryId = '" + rm2.CountryID + "'");

                    }

                }                


            }

            return sb.ToString();

        }
    }
}
