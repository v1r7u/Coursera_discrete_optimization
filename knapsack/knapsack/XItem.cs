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
    }
}