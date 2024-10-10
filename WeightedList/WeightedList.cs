using System.Collections;

namespace WeightedList
{
    public class WeightedList<T> : ICollection<WeightedElement<T>>
    {
        private List<WeightedElement<T>> Elements;

        public int Count => Elements.Count;

        public bool IsReadOnly => false;

        public int TotalWeight;

        public WeightedList()
            : this(4)
        {
        }

        public WeightedList(int capacity)
        {
            Elements = new List<WeightedElement<T>>(capacity);
            TotalWeight = 0;
        }

        public T GetRandomItem(Random random)
        {
            int randomValue = random.Next(0, TotalWeight);
            int currentRatio = 0;

            foreach (WeightedElement<T> element in Elements)
            {
                currentRatio += element.Weight;

                if (currentRatio >= randomValue)
                {
                    return element.Element;
                }
            }

            return Elements[Elements.Count - 1].Element;
        }

        public T GetRandomItem(int seed)
        {
            return GetRandomItem(new Random(seed));
        }

        public T GetRandomItem()
        {
            return GetRandomItem(new Random());
        }

        public void SetItemAtIndex(WeightedElement<T> item, int index)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }

            TotalWeight -= Elements[index].Weight;
            TotalWeight += item.Weight;

            Elements[index] = item;
        }

        public void SetElement(T element, int index)
        {
            if (element == null)
            {
                throw new ArgumentNullException();
            }

            Elements[index].Element = element;
        }

        public void SetWeight(int weight, int index)
        {
            if (weight < 1)
            {
                throw new ArgumentException("weight can't be less than 1.");
            }

            TotalWeight -= Elements[index].Weight;
            TotalWeight += weight;

            Elements[index].Weight = weight;
        }

        public void Add(WeightedElement<T> element)
        {
            if (element.Weight < 1)
            {
                throw new ArgumentException("weight can't be less than 1.");
            }

            TotalWeight += element.Weight;

            Elements.Add(element);
        }

        public void Add(T item, int weight)
        {
            if (weight < 1)
            {
                throw new ArgumentException("weight can't be less than 1.");
            }

            TotalWeight += weight;

            Elements.Add(new WeightedElement<T>(item, weight));
        }

        public void Add(T item)
        {
            TotalWeight += 1;

            Elements.Add(new WeightedElement<T>(item));
        }

        public bool Remove(WeightedElement<T> item)
        {
            if (Elements.Remove(item))
            {
                TotalWeight -= item.Weight;

                return true;
            }

            return false;
        }

        public bool Contains(WeightedElement<T> item)
        {
            return Elements.Contains(item);
        }

        public void CopyTo(WeightedElement<T>[] array, int arrayIndex)
        {
            Elements.CopyTo(array, arrayIndex);
        }

        public void Clear()
        {
            TotalWeight = 0;

            Elements.Clear();
        }

        IEnumerator<WeightedElement<T>> IEnumerable<WeightedElement<T>>.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }
    }
}
