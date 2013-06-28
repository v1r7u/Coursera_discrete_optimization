using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace knapsack
{
    public class KP : IDisposable
    {
        public KP(XElement[] elements, int capacity)
        {
            _elements = elements;
            _capacity = capacity;
            _elementsCount = elements.Length;

            previousCollumn = new int[capacity + 1];
            currentCollumn = new int[capacity + 1];

            Directory.CreateDirectory("./tempData");
        }

        private readonly int _elementsCount;
        private readonly int _capacity;
        private XElement[] _elements;

        private int[] previousCollumn;
        private int[] currentCollumn;

        public void FillCells()
        {
            for (int i = 1; i < _elementsCount + 1; i++)
            {
                var str = new FileStream(string.Format("./tempData/temp{0}", i), FileMode.Create, FileAccess.Write);
                using (var sw = new StreamWriter(str))
                {
                    if (_elements[i - 1].Weight > _capacity || _elements[i - 1].Value == 0)
                    {
                        CopyPrevious();
                    }
                    else
                    {
                        for (int j = 1; j < _capacity + 1; j++)
                        {
                            FillCurrentCell(i, j);
                        }
                    }
                    sw.Write(string.Join(" ", currentCollumn));
                    previousCollumn = currentCollumn;
                    currentCollumn = new int[_capacity + 1];
                }
            }
        }

        private void CopyPrevious()
        {
            currentCollumn = previousCollumn;
            previousCollumn = null;
        }

        private void FillCurrentCell(int column, int row)
        {
            int prevColumnIndex = column - 1;
            if (_elements[prevColumnIndex].Weight > row)
            {
                currentCollumn[row] = previousCollumn[row];
            }
            else
            {
                currentCollumn[row] = GetBestValue(prevColumnIndex, row);
            }
        }

        private int GetBestValue(int column, int row)
        {
            XElement currentElement = _elements[column];
            int tempRow = row >= currentElement.Weight
                              ? row - currentElement.Weight
                              : 0;

            int newValue = previousCollumn[tempRow] + currentElement.Value;
            int previousValue = previousCollumn[row];

            return (previousValue >= newValue)
                       ? previousValue
                       : newValue;
        }

        public string Solution()
        {
            //int maxValue = 0;
            //int maxIndex = 0;
            //for (int i = 1; i < _elementsCount + 1; i++)
            //{
            //    int currentValue = values[_capacity, i];
            //    if (maxValue < currentValue)
            //    {
            //        maxValue = currentValue;
            //        maxIndex = i - 1;
            //    }
            //}

            //XElement maxElement = _elements[maxIndex];
            //maxElement.IsIncluded = true;

            //MarkNext(FindLastColumnChange(maxIndex + 1) - maxElement.Weight, maxIndex);

            //string els = string.Join(" ", _elements.Select(i => i.IsIncluded
            //                                                        ? 1
            //                                                        : 0));

            //return string.Format("{0} {1}{2}{3}", maxValue, 0, Environment.NewLine, els);
            return "";
        }

        private int FindLastColumnChange(int maxIndex)
        {
            //for (int i = _capacity; i > 1; i--)
            //{
            //    if (values[i, maxIndex] != values[i - 1, maxIndex])
            //    {
            //        return i;
            //    }
            //}
            return 0;
        }

        private void MarkNext(int currentWeight, int currentIndex)
        {
            //if (values[currentWeight, currentIndex - 1] == values[currentWeight, currentIndex])
            //{
            //    MarkNext(currentWeight, currentIndex - 1);
            //}
            //else
            //{
            //    var element = _elements[currentIndex - 1];
            //    element.IsIncluded = true;
            //    if (currentWeight > element.Weight && currentIndex > 1)
            //        MarkNext(currentWeight - element.Weight, currentIndex - 1);
            //}
        }

        public void Dispose()
        {
            Directory.Delete("./TempData",true);
            _elements = null;
            previousCollumn = null;
            currentCollumn = null;
        }
    }
}