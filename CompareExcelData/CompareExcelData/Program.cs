using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace CompareExcelData
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection con1 = null;
            SqlConnection con2 = null;
            string constr1 = "";
            string constr2 = "";
            SqlCommand command1;
            SqlCommand command2;
            SqlDataAdapter adapter1 = new SqlDataAdapter();
            SqlDataAdapter adapter2 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            int i = 0;
            //string sql1 = "";


            //string sql1 = "select (Origin + Destination) from dbo.PartnerIncludeCP with(nolock) where PartnerCode = 'DL'";

            string sql1 = "select CityCode from dbo.City with (nolock)";

            string sql2 = "select * from clientData with(nolock) order by OrigdestCheck";

            con1 = new SqlConnection(constr1);
            con2 = new SqlConnection(constr2);

            

            try
            {
                con1.Open();
                command1 = new SqlCommand(sql1, con1);
                adapter1.SelectCommand = command1;
                adapter1.Fill(ds1);
                adapter1.Dispose();
                command1.Dispose();
                con1.Close();

                con2.Open();
                command2 = new SqlCommand(sql2, con2);
                adapter2.SelectCommand = command2;
                adapter2.Fill(ds2);
                adapter2.Dispose();
                command2.Dispose();
                con2.Close();


                DataTable dt1 = ds1.Tables[0];
                DataTable dt2 = ds2.Tables[0];

                int NotFoundRecs = 0;
                int foundRecs = 0;

                StringBuilder sb = new StringBuilder();
                //StringBuilder sb1 = new StringBuilder();
                //StringBuilder sb2 = new StringBuilder();
                    

                foreach (DataRow dr in dt2.Rows)
                {
                    
                    DataRow[] foundRows1;
                    DataRow[] foundRows2;

                    // Use the Select method to find all rows matching the filter.
                    //foundRows = dt1.Select("Column1 = '" + dr[0].ToString().Trim() + "'");
                    string data1 = dr[0].ToString().Substring(0, 3).Trim();
                    string data2 = dr[0].ToString().Substring(3, 3).Trim();

                    foundRows1 = dt1.Select("CityCode = '" + data1 + "'");
                    foundRows2 = dt1.Select("CityCode = '" + data2 + "'");

                                      
                    if (foundRows1.Length == 0)
                    {
                        //Console.WriteLine("Not found - " + dr[0].ToString());
                      
                        //sb1.AppendLine(dr[0].ToString().Substring(0,3));
                        //sb2.AppendLine(dr[0].ToString().Substring(3, 3));

                        if (!sb.ToString().Contains(data1))
                        {
                            sb.AppendLine(data1);
                            NotFoundRecs++;
                        }
                    }
                    else if (foundRows2.Length == 0)
                    {
                        if (!sb.ToString().Contains(data2))
                        {
                            sb.AppendLine(data2);
                            NotFoundRecs++;
                        }
                    }
                }


                //string fileLoc1 = @"c:\Origin.txt";
                //string fileLoc2 = @"c:\Destination.txt";

                string fileLoc = @"c:\MissingCities.txt";

                //if (File.Exists(fileLoc1))
                //{
                //    using (StreamWriter sw = new StreamWriter(fileLoc1))
                //    {
                //      sw.WriteLine(sb1.ToString());
                      
                //    }
                //}

                //if (File.Exists(fileLoc2))
                //{
                //    using (StreamWriter sw = new StreamWriter(fileLoc2))
                //    {
                //        sw.WriteLine(sb2.ToString());
                //    }
                //}

                if (File.Exists(fileLoc))
                {
                    using (StreamWriter sw = new StreamWriter(fileLoc))
                    {
                        sw.WriteLine(sb.ToString());

                    }
                }
              
                Console.WriteLine("Records Not Found = " + NotFoundRecs.ToString());
                //Console.WriteLine("Records Found = " + foundRecs.ToString());
                //Console.WriteLine("Records Processed = ");
                //Console.WriteLine("Records Returned = ");

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open connection ! " + ex.ToString());
                Console.ReadLine();
            }
            finally
            {
                
            }
  
        }
    }
}
