using System;
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

            previousColumn = new int[capacity + 1];
            currentColumn = new int[capacity + 1];

            maxIndex = 0;
            maxValue = 0;

            Directory.CreateDirectory("./tempData");
        }

        private readonly int _elementsCount;
        private readonly int _capacity;
        private XElement[] _elements;

        private int[] previousColumn;
        private int[] currentColumn;

        private int maxValue;
        private int maxIndex;

        #region Filling

        public void FillCells()
        {
            for (int i = 1; i < _elementsCount + 1; i++)
            {
                var str = new FileStream(string.Format("./tempData/temp{0}", i), FileMode.Create, FileAccess.Write);
                var sw = new StreamWriter(str);

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
                sw.Write(string.Join(" ", currentColumn));
                previousColumn = currentColumn;
                currentColumn = new int[_capacity + 1];

                sw.Close();
                str.Close();
            }
        }

        private void CopyPrevious()
        {
            currentColumn = previousColumn;
            previousColumn = null;
        }

        private void FillCurrentCell(int column, int row)
        {
            int prevColumnIndex = column - 1;
            if (_elements[prevColumnIndex].Weight > row)
            {
                currentColumn[row] = previousColumn[row];
            }
            else
            {
                currentColumn[row] = GetBestValue(prevColumnIndex, row);
            }
        }

        private int GetBestValue(int column, int row)
        {
            XElement currentElement = _elements[column];
            int tempRow = row >= currentElement.Weight
                              ? row - currentElement.Weight
                              : 0;

            int newValue = previousColumn[tempRow] + currentElement.Value;
            int previousValue = previousColumn[row];

            if(newValue > maxValue)
            {
                maxValue = newValue;
                maxIndex = column;
            }

            return (previousValue >= newValue)
                       ? previousValue
                       : newValue;
        }

        #endregion

        #region Solution

        public string Solution()
        {
            XElement maxElement = _elements[maxIndex];
            maxElement.IsIncluded = true;

            MarkNext(FindLastColumnChange() - maxElement.Weight, maxIndex);

            string els = string.Join(" ", _elements.Select(i => i.IsIncluded
                                                                    ? 1
                                                                    : 0));

            return string.Format("{0} {1}{2}{3}", maxValue, 1, Environment.NewLine, els);
        }

        private int FindLastColumnChange()
        {
            string[] values = GetColumnElements(maxIndex + 1);
            for (int i = _capacity; i > 1; i--)
            {
                if (values[i] != values[i - 1])
                {
                    return i;
                }
            }
            return 0;
        }

        private void MarkNext(int currentWeight, int currentIndex)
        {
            string[] prevCol = GetColumnElements(currentIndex - 1);
            string[] curCol = GetColumnElements(currentIndex);
            
            if (prevCol[currentWeight] == curCol[currentWeight])
            {
// ReSharper disable RedundantAssignment
                prevCol = null;
                curCol = null;
// ReSharper restore RedundantAssignment
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

        #endregion

        #region Helpers

        private static string[] GetColumnElements(int index)
        {
            var str = new FileStream(string.Format("./tempData/temp{0}", index), FileMode.Open, FileAccess.Read);
            
                var sr = new StreamReader(str);
                // ReSharper disable PossibleNullReferenceException
                string[] curCol = sr.ReadLine().Split(' ');
                // ReSharper restore PossibleNullReferenceException
            
            sr.Close();
            str.Close();
            return curCol;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Directory.Delete("./TempData",true);
            _elements = null;
            previousColumn = null;
            currentColumn = null;
        }

        #endregion
    }
}