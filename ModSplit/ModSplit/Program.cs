using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModSplit
{
    class Program
    {
        static void Main(string[] args)
        {
            double totalRecords = 987526310999;
            double splitTables = 7;

            double remainder = totalRecords%splitTables;

            double eachTableRecords = (totalRecords - remainder) / splitTables;


            Console.WriteLine("Total records in table is {0}",totalRecords);

            for (double i = 1; i <= splitTables; i++)
            {
                if (i == splitTables)
                {
                    Console.WriteLine("Table {0} should have {1} records", i, (eachTableRecords + remainder));
                }
                else
                {
                    Console.WriteLine("Table {0} should have {1} records", i, eachTableRecords);
                }
            }

            Console.Read();


        }
    }
}
