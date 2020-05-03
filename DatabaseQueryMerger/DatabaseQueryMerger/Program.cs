using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ConsoleApplication1
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
            DataSet ds2 = new DataSet(); 
            int i = 0;
            string sql = "";


            sql = "Select AccountNo,MileBalance from Member with(nolock) where MileBalance < 0";

            string sql2 = "Select SUM(Miles) from MemberActivity with(nolock) where AccountNo=";

            con = new SqlConnection(constr);

            try
            {
                con.Open();
                command = new SqlCommand(sql, con);
                adapter.SelectCommand = command;
                adapter.Fill(ds1);
                adapter.Dispose();
                command.Dispose();
                con.Close();


                con.Open();
                DataTable dt = ds1.Tables[0];

                foreach (DataRow dr in dt.Rows)
               {
                   sql2 = "Select SUM(Miles) from MemberActivity with(nolock) where AccountNo=" + dr[0].ToString();
                   //sql2 += dr[0].ToString();
                   int actMiles = 0;
                   
                   command = new SqlCommand(sql2, con);
                   adapter.SelectCommand = command;
                   adapter.Fill(ds2);
                   adapter.Dispose();
                   command.Dispose();

                   DataTable dt1 = ds2.Tables[0];

                   foreach (DataRow dr1 in dt1.Rows)
                   {
                       actMiles = Convert.ToInt32(dr1[0]);
                   }

                   //Console.WriteLine("AccountNo - " + dr[0].ToString() + " Mile Balance - " + dr[1].ToString() + " Activity Balance - " + actMiles);
                   if (Convert.ToInt32(dr[1]) != actMiles)
                   {                      
                       Console.WriteLine(dr[0].ToString());                       
                       
                   }

                   ds2 = new DataSet();
                   

               }
                con.Close();
                Console.WriteLine("Completed");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open connection ! " + ex.ToString());
                Console.ReadLine();
            }
            finally
            {
                con.Close();
            }
            
        }

       
    }
}
