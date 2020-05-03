using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace GetEmailAdddress
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection con = null;
            string constr = "";
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
       
  
            
            string sql2 = "Select EmailAddress from Member with(nolock) where AccountNo = ";

            con = new SqlConnection(constr);

            try
            {
               con.Open();

                using (StreamReader sr = new StreamReader(@"D:\AccNo.txt"))
                {
                    String line;
                    
                    while ((line = sr.ReadLine()) != null)
                    {                        
                        command = new SqlCommand(sql2 + line.Trim(), con);
                        adapter.SelectCommand = command;
                        adapter.Fill(ds1);
                        adapter.Dispose();
                        command.Dispose();                        
                    }
                }


                con.Close();
                DataTable dt = ds1.Tables[0];
                StringBuilder sb = new StringBuilder();
                
                foreach (DataRow dr in dt.Rows)
                {
                    sb.AppendLine(dr[0].ToString());

                }

                File.WriteAllText(@"C:\Output.txt", sb.ToString());

            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                con.Close();
            }
        }
    }
}
