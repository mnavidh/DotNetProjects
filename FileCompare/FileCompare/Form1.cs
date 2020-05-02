using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FileCompare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamReader objstream1 = new StreamReader("C:\\Compare\\File1.txt");
            StreamReader objstream2 = new StreamReader("C:\\Compare\\File2.txt");

            string text1 = objstream1.ReadLine();
            string text2 = objstream2.ReadLine();

            int maxlength = text1.Length > text2.Length ? text1.Length : text2.Length;

          
                //string [] arr1 = text1.Split('');
                char[] thechars1 = text1.ToCharArray();
                char[] thechars2 = text2.ToCharArray();
                
                DataTable workTable = new DataTable("Mismatch");

                DataColumn workCol = workTable.Columns.Add("Error Column No", typeof(int));
                workCol.AllowDBNull = false;
                workCol.Unique = true;

                workTable.Columns.Add("File 1 Value", typeof(string));
                workTable.Columns.Add("File 2 Value", typeof(string));



                try
                {
               
                for (int i = 0; i < maxlength; i++)
                {

                    if (text1.Length > text2.Length)
                    {
                        if ((i + 1) > text2.Length)
                        {
                            DataRow workRow = workTable.NewRow();

                            workRow[0] = i + 1;

                            workRow[1] = thechars1[i].ToString();

                            workRow[2] = "Missing";

                            workTable.Rows.Add(workRow);
                        }
                        else
                        {
                            if (thechars1[i] != thechars2[i])
                            {
                                DataRow workRow = workTable.NewRow();

                                workRow[0] = i + 1;

                                workRow[1] = thechars1[i].ToString();

                                workRow[2] = thechars2[i].ToString();

                                workTable.Rows.Add(workRow);
                            }

                        }
                        
                    }
                    else
                    {
                        if ((i + 1) > text1.Length)
                        {
                            DataRow workRow = workTable.NewRow();

                            workRow[0] = i + 1;

                            workRow[1] = "Missing";

                            workRow[2] = thechars2[i].ToString();

                            workTable.Rows.Add(workRow);
                        }
                        else
                        {
                            if (thechars2[i] != thechars1[i])
                            {
                                DataRow workRow = workTable.NewRow();

                                workRow[0] = i + 1;

                                workRow[1] = thechars1[i].ToString();

                                workRow[2] = thechars2[i].ToString();

                                workTable.Rows.Add(workRow);
                            }

                        }
                        
                    }
                        

                }


                dataGridView1.DataSource = workTable;
                //dataGridView1.
                //label2.Visible = true;
                //label2.Text = msg;

                }
                catch (Exception ex)
                {
                    label2.Visible = true;
                    label2.Text = ex.ToString();
                }

        }
    }
}
