using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ParallelPattern
{
    class Program
    {

        private static bool _Cancel = false;

        static void Main(string[] args)
        {

            Console.WriteLine("Beispiel für parallele Programmierung in c#. ");
            Console.WriteLine();

            Console.WriteLine("Berechnung von Primzahlen Auswahlmöglichkeiten:");
            Console.WriteLine("    [1] Sequentiel");
            Console.WriteLine("    [2] Parallel");
            Console.WriteLine("    [3] Parallel LINQ");
            Console.WriteLine("    [4] Parallel LINQ mit Einschränkung an CPU-Kerne");
            Console.WriteLine("");
            Console.WriteLine("Andere Funktionen:");
            Console.WriteLine("    [5] Parallel Invoke");
            Console.WriteLine("    [6] Task-Klasse");
            Console.WriteLine("");
            Console.WriteLine("Anwendung Auswahlmöglichkeiten:");
            Console.WriteLine("    [7] Exit");
            Console.WriteLine();

            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                string key = keyInfo.KeyChar.ToString();

                switch (key)
                {
                    /// Sqeuentielle Ermittlung aller Primzahlen im bereich 1 bis 10000000
                    case "1":

                        Console.WriteLine("Primzahlen werden sequentiel ermittelt....");

                        Stopwatch swseq = Stopwatch.StartNew();
                        foreach (var i in Enumerable.Range(1, 10000000))
                        {
                            if (IsPrime(i))
                            {
                                //Console.WriteLine(i);
                            }
                        }

                        swseq.Stop();

                        Console.WriteLine("sequentiell fertig nach {0}ms", swseq.ElapsedMilliseconds);
                        Console.WriteLine("");

                        break;

                    /// Parallele Ermittlung aller Primzahlen im bereich 1 bis 10000000
                    case "2":

                        Console.WriteLine("Primzahlen werden parallel ermittelt...");
                        Stopwatch swparallel = Stopwatch.StartNew();
                        System.Threading.Tasks.Parallel.ForEach(Enumerable.Range(1, 10000000), body =>
                        {
                            IsPrime(body);
                        });
                        swparallel.Stop();
                        Console.WriteLine("parellel fertig nach {0}ms", swparallel.ElapsedMilliseconds);
                        Console.WriteLine("");

                        break;

                    /// PLINQ Ermittlung aller Primzahlen im bereich 1 bis 10000000
                    case "3":

                        Console.WriteLine("Primzahlen werden mit parallel LINQ ermittelt...");
                        Stopwatch swparallelLinq = Stopwatch.StartNew();
                        var primes = (from p in Enumerable.Range(1, 10000000).AsParallel()
                                           where IsPrime(p) == true
                                           select p).ToList();

                        swparallelLinq.Stop();
                        Console.WriteLine("parellel fertig nach {0}ms", swparallelLinq.ElapsedMilliseconds);
                        Console.WriteLine("");

                        break;

                    /// PLINQ Ermittlung aller Primzahlen im bereich 1 bis 10000000
                    case "4":

                        Console.WriteLine("Primzahlen werden mit parallel LINQ und beschränkter Anzahl an CPU-Kernen ermitteln...");
                        Console.Write("Anzahl CPU-Kerne: ");
                        ConsoleKeyInfo keyProcessor = Console.ReadKey(false);
                        Console.Write("\n");

                        int anzahlKey = System.Convert.ToInt32(keyProcessor.KeyChar.ToString());
                        
                        Stopwatch swparallelLinqWithProcessor = Stopwatch.StartNew();
                        var primesWithProcessor = (from p in Enumerable.Range(1, 10000000)
                             .AsParallel()
                             .WithDegreeOfParallelism(anzahlKey)
                                      where IsPrime(p) == true
                                      select p).ToList();
                        Console.WriteLine("parellel fertig nach {0}ms", swparallelLinqWithProcessor.ElapsedMilliseconds);
                        Console.WriteLine("");
                        break;

                    case "5":
                        Console.WriteLine("Verarbeitung von drei Methoden die jeweils die Zahlen 1-10 durchlaufen...");
                        System.Threading.Tasks.Parallel.Invoke(TaskOne, TaskTwo, TaskThree);
                        Console.WriteLine("");
                        break;

                    case "6":

                        Console.WriteLine("Verwendung der Task-Klasse...");

                        ///Task #1 über new initialisiert und seperater Methode. Muss explizit gestartet werden.
                        System.Threading.Tasks.Task task1 = new System.Threading.Tasks.Task(DoSomething);
                        task1.Start();

                        Console.WriteLine("");

                        ///Task #2 über new initialisiert und LINQ-Methode. Muss explizit gestartet werden.
                        System.Threading.Tasks.Task task2 = new System.Threading.Tasks.Task(() =>
                        {
                            Console.WriteLine("Task #2 gestartet ...");
                            System.Threading.Thread.Sleep(3000);
                            Console.WriteLine("Task #2: fertig ...");
                        });
                        task2.Start();

                        Console.WriteLine("");

                        ///Task #3 über Task.Factory.StartNew initialisiert und LINQ-Methode. Wird automatisch gestartet.
                        System.Threading.Tasks.Task task3 = System.Threading.Tasks.Task.Factory.StartNew(() =>
                        {
                            Console.WriteLine("Task #3 gestartet ...");
                            System.Threading.Thread.Sleep(6000);
                            Console.WriteLine("Task #3: fertig ...");
                        });
                        System.Threading.Tasks.Task.WaitAll(task1, task2, task3);

                        break;

                    /// Abbruch der Anwendung.
                    default:

                        if (key == "7")
                        {
                            _Cancel = true;
                            Console.WriteLine("ENTER-Taste zum beenden der Anwendung...");
                        }
                        else
                        {
                            Console.WriteLine("'" + key + "' steht nicht zur Auswahl.");
                        }
                        break;
                }

            } while (!_Cancel);

            Console.Read();
        }

        /// <summary>
        /// Bestimmt ob die übergebene Zahl eine Primzahl ist oder nicht.
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public static bool IsPrime(int candidate)
        {
            if ((candidate & 1) == 0)
            {
                if (candidate == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            int num = (int)Math.Sqrt((double)candidate);
            for (int i = 3; i <= num; i += 2)
            {
                if ((candidate % i) == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private static void TaskOne()
        {
            for (int i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(50);
                Console.Write(" #1 ");
            }
        }

        private static void TaskTwo()
        {
            for (int i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(50);
                Console.Write(" #2 ");
            }
        }

        private static void TaskThree()
        {
            for (int i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(50);
                Console.Write(" #3 ");
            }
        }

        private static void DoSomething()
        {
            Console.WriteLine("Task #1 gestartet ...");
            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("Task #1: fertig ...");
        }

    }
}
