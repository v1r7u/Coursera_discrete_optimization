using System;
using System.Collections.Generic;
using System.Linq;

namespace knapsack
{
    public class KP :IDisposable
    {
        public KP(XElement[] elements, int capacity)
        {
            _elements = elements;
            _capacity = capacity;
            _elementsCount = elements.Length;

            values = new int[capacity + 1,_elementsCount + 1];
            //previousCollumn = new int[capacity + 1];
            //currentCollumn = new int[capacity + 1];
        }
        
        private readonly int _elementsCount;
        private readonly int _capacity;
        private XElement[] _elements;

        private int[] previousCollumn;
        private int[] currentCollumn;

        private int[,] values;

        public void FillCells()
        {
            for (int i = 1; i < _elementsCount + 1; i++)
            {
                for (int j = 1; j < _capacity + 1; j++)
                {
                    FillCurrentCell(i,j);
                }
            }
        }

        private void FillCurrentCell(int column, int row)
        {
            int prevColumnIndex = column - 1;
            if (_elements[prevColumnIndex].Weight > row)
            {
                values[row, column] = values[row, prevColumnIndex];
            }
            else
            {
                values[row, column] = GetBestValue(prevColumnIndex,row);
            }
        }

        private int GetBestValue(int column, int row)
        {
            XElement currentElement = _elements[column];
            int tempRow = row >= currentElement.Weight
                              ? row - currentElement.Weight
                              : 0;

            int newValue = values[tempRow, column] + currentElement.Value;
            int previousValue = values[row, column];

            return previousValue >= newValue 
                ? previousValue 
                : newValue;
        }

        public string Solution()
        {


            IEnumerable<int> enumerable = _elements.Select(i => i.IsIncluded ? 1 : 0);

            return string.Join(" ", enumerable);
        }

        public void Dispose()
        {
            _elements = null;
            values = null;
            previousCollumn = null;
            currentCollumn = null;
        }
    }
}