using System;

namespace WeightedListApp
{
    public struct WeightedElement<T> : IEquatable<WeightedElement<T>> where T : IEquatable<T>
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

        public bool Equals(WeightedElement<T> other)
        {
            return Element.Equals(other.Element) && Weight == other.Weight;
        }

        public override bool Equals(object obj)
        {
            if (obj is WeightedElement<T> other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Element, Weight);
        }
    }
}
