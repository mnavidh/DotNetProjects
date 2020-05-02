using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogic
{
    class Program
    {
        static void Main(string[] args)
        {
            Process();
        }

        static void Process()
        {
            string nameInDB = string.Empty;
            string nameFromSabre = string.Empty;

            Console.Write("Enter name in DB : ");
            nameInDB = Console.ReadLine();
            Console.Write("Enter name to match : ");
            nameFromSabre = Console.ReadLine();


            if (nameInDB.Contains(nameFromSabre))
            {
                Console.WriteLine("Match found");                
            }
            else
            {
                Console.WriteLine("No Match");               
            }

            string[] arguments = new string[2];

            Main(arguments);
        }
    }
}
