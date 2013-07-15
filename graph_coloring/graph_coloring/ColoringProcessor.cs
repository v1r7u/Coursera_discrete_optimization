using System;
using System.Collections.Generic;
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
            var enumerable = GetCombinations(GetInitialArray(), _vertexNumber);
            foreach (var colors in enumerable)
            {
                if(CheckConstraints(colors))
                {
                    _bestColors = colors.ToArray();
                    return;
                }
            }
        }

        public string FormatAnswer()
        {
            int colorsCount = _bestColors.Max() + 1;
            return string.Format("{0} {1}{2}{3}", colorsCount, 0, Environment.NewLine, string.Join(" ", _bestColors));
        }

        private bool CheckConstraints(IEnumerable<int> colors)
        {
            return _nodes.All(t => colors.ElementAt(t.Vertex1) != colors.ElementAt(t.Vertex2));
        }

        private IEnumerable<int> GetInitialArray()
        {
            int[] initial = new int[_vertexNumber];
            for (int i = 0; i < _vertexNumber; i++)
            {
                initial[i] = i;
            }
            return initial;
        }

        private IEnumerable<IEnumerable<int>> GetCombinations(IEnumerable<int> list, int length)
        {
            if (length == 1) return list.Select(i => new int[] {i});

            return GetCombinations(list, length - 1)
                .SelectMany(i => list, (i1, i2) => i1.Concat(new int[] {i2}));
        }
    }
}