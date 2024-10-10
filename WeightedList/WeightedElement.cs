namespace WeightedListApp
{
    public class WeightedElement<T>
    {
        public T Element;
        public int Weight
        {
            get
            {
                return Weight;
            }
            set
            {
                if (value < 1)
                {
                    throw new System.Exception("weight can't be less than 1.");
                }

                Weight = value;
            }
        }

        public WeightedElement(T element)
            : this(element, 1)
        {
        }

        public WeightedElement(T element, int weight)
        {
            if (weight < 1)
            {
                throw new System.ArgumentException("weight can't be less than 1.");
            }

            this.Element = element;
            this.Weight = weight;
        }
    }
}
