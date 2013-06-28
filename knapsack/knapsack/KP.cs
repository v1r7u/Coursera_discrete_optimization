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
            int maxValue = 0;
            int maxIndex = 0;
            for (int i = 1; i < _elementsCount + 1; i++)
            {
                int currentValue = values[_capacity, i];
                if (maxValue < currentValue)
                {
                    maxValue = currentValue;
                    maxIndex = i - 1;
                }
            }

            XElement maxElement = _elements[maxIndex];
            maxElement.IsIncluded = true;

            MarkNext(FindLastColumnChange(maxIndex + 1) - maxElement.Weight, maxIndex);

            string els = string.Join(" ", _elements.Select(i => i.IsIncluded ? 1 : 0));

            return string.Format("{0} {1}{2}{3}", maxValue, 0, Environment.NewLine, els);
        }

        private int FindLastColumnChange(int maxIndex)
        {
            for (int i = _capacity; i > 1; i--)
            {
                if (values[i,maxIndex] != values[i-1, maxIndex])
                {
                    return i;
                }
            }
            return 0;
        }

        private void MarkNext(int currentWeight, int currentIndex)
        {
            if (values[currentWeight, currentIndex - 1] == values[currentWeight, currentIndex])
            {
                MarkNext(currentWeight, currentIndex - 1);
            }
            else
            {
                var element = _elements[currentIndex - 1];
                element.IsIncluded = true;
                if (currentWeight > element.Weight && currentIndex > 1)
                    MarkNext(currentWeight - element.Weight, currentIndex - 1);
            }
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