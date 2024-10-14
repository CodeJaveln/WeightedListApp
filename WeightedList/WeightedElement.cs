namespace WeightedListApp
{
    public struct WeightedElement<T> 
    {
        public T Element { get; set; }
        public int Weight { get; set; }

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
