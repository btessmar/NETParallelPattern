using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelPattern
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Beispiel für parallele Programmierung in c#.");
            Console.WriteLine();

            Console.WriteLine("Auswahlmöglichkeiten:");
            Console.WriteLine("    [1] Sequentiel");
            Console.WriteLine("    [2] Parallel");
            Console.WriteLine();

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            string key = keyInfo.KeyChar.ToString();

            switch (key)
            {
                case "1":

                    break;
                case "2":

                    break;
                default:
                    Console.WriteLine("'" + key + "' steht nicht zur Auswahl.");
                    break;
            }

            Console.Read();
        }
    }
}
