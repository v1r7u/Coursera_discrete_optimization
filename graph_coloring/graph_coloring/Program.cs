using System;
using System.IO;

namespace graph_coloring
{
    class Program
    {
        private static readonly string[] Datas = new[]
                                     {
                                         "gc_4_1",
                                         "gc_20_1",
                                         "gc_20_3",
                                         "gc_20_5",
                                         "gc_20_7",
                                         "gc_20_9",
                                         "gc_50_1",
                                         "gc_50_3",
                                         "gc_50_5",
                                         "gc_50_7",
                                         "gc_50_9",
                                         "gc_70_1",
                                         "gc_70_3",
                                         "gc_70_5",
                                         "gc_70_7",
                                         "gc_70_9",
                                         "gc_100_1",
                                         "gc_100_3",
                                         "gc_100_5",
                                         "gc_100_7",
                                         "gc_100_9",
                                         "gc_250_1"
                                     };
        
        static void Main(string[] args)
        {
            string directory = args[0];

            foreach (var s in Datas)
            {
                ColoringProcessor cp;

                string path = directory + s;
                Console.WriteLine(s);
                
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                using (var sr = new StreamReader(fs))
                {
                    // ReSharper disable PossibleNullReferenceException
                    string[] lines = sr.ReadLine().Split(' ');
                    int vertexes = int.Parse(lines[0]);
                    int n = int.Parse(lines[1]);
                
                    Node[] nodes = new Node[n];
                    
                    for (int i = 0; i < n; i++)
                    {
                        string[] strings = sr.ReadLine().Split(' ');
                        nodes[i] = new Node(Convert.ToInt32(strings[0]), Convert.ToInt32(strings[1]));
                    }
                    // ReSharper restore PossibleNullReferenceException
                    
                    cp = new ColoringProcessor(vertexes, nodes);
                }

                cp.ColorGraph();
             
                Console.WriteLine(cp.FormatAnswer());
                Console.WriteLine();
                
                fs.Close();
                fs.Dispose();
            }
            Console.ReadKey();
        }
    }
}
