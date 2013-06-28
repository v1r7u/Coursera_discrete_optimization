namespace knapsack
{
    public class XElement
    {
        public XElement(int value, int weight)
        {
            Weight = weight;
            Value = value;
        }

        public readonly int Weight;
        public readonly int Value;
        public bool IsIncluded;

        public override string ToString()
        {
            return string.Format("Value:{0}, Weight:{1}, IsIncluded:{2}", Value, Weight, IsIncluded);
        }
    }
}