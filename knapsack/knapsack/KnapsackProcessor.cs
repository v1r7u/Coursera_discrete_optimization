using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace knapsack
{
    public class KnapsackProcessor
    {
        public KnapsackProcessor(XElement[] elements, int capacity)
        {
            _elements = elements;
            _capacity = capacity;
            _elementsCount = elements.Length;

            previousCollumn = new int[capacity + 1];
            currentCollumn = new int[capacity + 1];
        }
        
        private readonly int _elementsCount;
        private readonly int _capacity;
        private readonly XElement[] _elements;

        private int[] previousCollumn;
        private int[] currentCollumn;

        public void FillCells()
        {
            var str = new FileStream("temp", FileMode.Truncate, FileAccess.Write);
            using (var sw = new StreamWriter(str))
            {
                for (int i = 0; i < _elementsCount; i++)
                {
                    if (_elements[i].Weight <= _capacity && _elements[i].Value > 0)
                    {
                        for (int j = 1; j <= _capacity; j++)
                        {
                            FillCurrentCell(_elements[i], j);
                        }
                    }

                    sw.WriteLine(string.Join(" ", currentCollumn));
                    previousCollumn = currentCollumn;
                    currentCollumn = new int[_capacity + 1];
                }
            }
            str.Close();
            str.Dispose();
        }

        public string Solution()
        {
            var maxValues = new int[_elementsCount + 1];

            var str = new FileStream("temp", FileMode.Open, FileAccess.Read);
            using (var sr = new StreamReader(str))
            {
                for (int i = 1; i <= _elementsCount; i++)
                {
                    string[] values = sr.ReadLine().Split(' ');
                    maxValues[i] = Convert.ToInt32(values.Last());
                }
            }
            str.Close();
            str.Dispose();

            int max = maxValues.Max();
            int elNumber = Array.IndexOf(maxValues, max);

            MarkElements(elNumber - 1);

            return BuildDecisionString(max);
        }

        private void FillCurrentCell(XElement xElement, int row)
        {
            if(row >= xElement.Weight)
            {
                currentCollumn[row] = OptimalCellValue(xElement, row);
            }
            else
            {
                currentCollumn[row] = previousCollumn[row];
            }
        }

        private int OptimalCellValue(XElement xElement, int row)
        {
            int tempRow = xElement.Weight > row 
                ? 0 
                : row - xElement.Weight;
            
            int totalValue = xElement.Value + previousCollumn[tempRow];
            int totalWeight = xElement.Weight + tempRow;

            if (totalWeight <= row && totalValue > previousCollumn[row])
                return totalValue;
            return previousCollumn[row];
        }

        private void MarkElements(int lastElement)
        {
            _elements[lastElement].IsIncluded = true;

            MarkNext(_capacity - _elements[lastElement].Weight, lastElement);
        }

        private void MarkNext(int row, int column)
        {
            int curValue, prevValue;
            var str = new FileStream("temp", FileMode.Open, FileAccess.Read);
            using (var sr = new StreamReader(str))
            {
                for (int i = 0; i < column - 2; i++)
                {
                    sr.ReadLine();
                }
                string[] values = sr.ReadLine().Split(' ');
                prevValue = Convert.ToInt32(values[row]);

                values = sr.ReadLine().Split(' ');
                curValue = Convert.ToInt32(values[row]);
            }

            if (curValue == 0)
                return;
            if (curValue > prevValue)
            {
                _elements[column - 1].IsIncluded = true;
                if (row > _elements[column - 1].Weight)
                    MarkNext(row - _elements[column - 1].Weight, column - 1);
            }
            else
            {
                MarkNext(row, column - 1);
            }
        }

        private string BuildDecisionString(int solution)
        {
            IEnumerable<int> enumerable = _elements.Select(i => i.IsIncluded ? 1 : 0);

            return string.Format("{0} {1} {2}{3}", solution, 0, Environment.NewLine, string.Join(" ", enumerable));
        }
    }
}