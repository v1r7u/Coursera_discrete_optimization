using System;
using System.Linq;

namespace graph_coloring
{
    public class ColoringProcessor
    {
        private readonly int _vertexNumber;
        private readonly Node[] _nodes;
        private int[] _bestColors;

        public ColoringProcessor(int vertexes, Node[] nodes)
        {
            _nodes = nodes;
            _vertexNumber = vertexes;
            _bestColors = new int[_vertexNumber];
        }

        public void ColorGraph()
        {
            int[] vertexColors1 = new int[] { 0, 1, 0, 0 };
            int[] vertexColors2 = new int[] { 0, 1, 1, 0 };
            int[] vertexColors3 = new int[] { 0, 1, 0, 2 };
            bool checkConstraints1 = CheckConstraints(vertexColors1);
            bool checkConstraints2 = CheckConstraints(vertexColors2);
            bool checkConstraints3 = CheckConstraints(vertexColors3);
        }

        public string FormatAnswer()
        {
            int colorsCount = _bestColors.Max();
            return string.Format("{0} {1}{2}{3}", colorsCount, 0, Environment.NewLine, string.Join(" ", _bestColors));
        }

        private bool CheckConstraints(int[] colors)
        {
            return _nodes.All(t => colors[t.Vertex1] != colors[t.Vertex2]);
        }
    }
}