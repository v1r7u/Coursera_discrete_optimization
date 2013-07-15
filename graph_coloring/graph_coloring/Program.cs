using System;
using System.IO;

namespace graph_coloring
{
    class Program
    {
        static void Main(string[] args)
        {
            ColoringProcessor cp;

            FileStream fs = new FileStream(args[0], FileMode.Open, FileAccess.Read);
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
        }
    }
}
