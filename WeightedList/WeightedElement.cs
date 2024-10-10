namespace WeightedList
{
    public class WeightedElement<T>
    {
        public T Element;
        public int Weight;

        public WeightedElement(T element)
            : this(element, 1)
        {
        }

        public WeightedElement(T element, int weight)
        {
            if (weight < 1)
            {
                throw new ArgumentException("weight can't be less than 1.");
            }

            this.Element = element;
            this.Weight = weight;
        }
    }
}
