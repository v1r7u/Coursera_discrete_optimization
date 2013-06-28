using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace knapsack
{
    class Program
    {
        private static KP kp;
        private static string[] data = new []
                                           {
                                               "ks_4_0",
                                               "ks_19_0",
                                               "ks_30_0",
                                               "ks_40_0",
                                               "ks_45_0",
                                               "ks_50_0",
                                               "ks_50_1",
                                               "ks_60_0",
                                               "ks_100_0",
                                               //"ks_100_1",
                                               "ks_100_2",
                                               "ks_200_0",
                                               //"ks_200_1",
                                               //"ks_300_0",
                                               //"ks_400_0",
                                               //"ks_500_0",
                                               //"ks_1000_0",
                                               //"ks_10000_0"
                                           };
        static void Main(string[] args)
        {
            //NormalRun(args);
            TestRun(args);
        }

        private static void NormalRun(string[] args)
        {
            ParseInput(string.Format(args[0]));
            kp.FillCells();
            Console.Write(kp.Solution());
        }

        private static void TestRun(string[] args)
        {
            Directory.CreateDirectory("./tempData");
            foreach (var d in data)
            {
                Console.WriteLine(d);
                Stopwatch sw = new Stopwatch();
                sw.Start();

                ParseInput(string.Format(@"{0}{1}", args[0], d));

                sw.Stop();
                var parsing = sw.Elapsed;
                Console.WriteLine("parse take {0}", parsing);

                sw.Reset();
                sw.Start();

                kp.FillCells();

                sw.Stop();
                var filling = sw.Elapsed;
                Console.WriteLine("filling take {0}", filling);

                sw.Reset();
                sw.Start();

                string solution = kp.Solution();

                sw.Stop();
                var buildingResult = sw.Elapsed;
                Console.WriteLine("building result take {0}", buildingResult);

                Console.WriteLine(solution);
                Console.WriteLine();
                sw.Reset();

                kp.Dispose();
            }
            Console.ReadKey();
        }

        private static void ParseInput(string filePath)
        {
            Stream str = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using (var sr = new StreamReader(str))
            {
                string[] strings = sr.ReadLine().Split(' ');
                int N = Convert.ToInt32(strings.First());
                int K = Convert.ToInt32(strings.Last());

                XElement[] elements = new XElement[N];
                for (int i = 0; i < N; i++)
                {
                    string[] element = sr.ReadLine().Split(' ');
                    int val = Convert.ToInt32(element.First());
                    int weight = Convert.ToInt32(element.Last());
                    elements[i] = new XElement(val, weight);
                }

                kp = new KP(elements, K);
            }
            str.Close();
        }
    }
}
