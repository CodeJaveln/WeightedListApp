using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WeightedListApp
{
    public class WeightedList<T> : IEnumerable<T>
    {
        // Default weight is 1 which is the same as default randomness

        private List<WeightedElement<T>> Elements;

        public T this[int index]
        {
            get => Elements[index].Element;
        }

        public int Count => Elements.Count;

        public bool IsReadOnly => false;

        public int TotalWeight;
        private List<int> CumulativeWeight;

        public WeightedList()
        {
            this.Elements = new List<WeightedElement<T>>();
            this.TotalWeight = 0;
            this.CumulativeWeight = new List<int>();
        }

        public WeightedList(int capacity)
        {
            this.Elements = new List<WeightedElement<T>>(capacity);
            this.TotalWeight = 0;
            this.CumulativeWeight = new List<int>(capacity);
        }

        public T GetRandomItem(Random random)
        {
            if (TotalWeight == 0)
            {
                throw new InvalidOperationException("GetRandomItem called with no items.");
            }

            int randomValue = random.Next(1, TotalWeight + 1);

            int leftIndex = 0, rightIndex = CumulativeWeight.Count;

            while (leftIndex < rightIndex)
            {
                int middleIndex = (leftIndex + rightIndex) >> 1;

                if (CumulativeWeight[middleIndex] >= randomValue)
                {
                    rightIndex = middleIndex;
                }
                else
                {
                    leftIndex = middleIndex + 1;
                }
            }

            return Elements[leftIndex].Element;
        }

        public T GetRandomItem(int seed)
        {
            return GetRandomItem(new Random(seed));
        }

        public T GetRandomItem()
        {
            return GetRandomItem(new Random());
        }

        public int GetWeight(int index)
        {
            return Elements[index].Weight;
        }

        public void SetWeight(int weight, int index)
        {
            if (weight < 1)
            {
                throw new ArgumentException("weight can't be less than 1.");
            }

            for (int i = index; i < CumulativeWeight.Count; i++)
            {
                CumulativeWeight[i] += weight - Elements[index].Weight;
            }

            TotalWeight -= Elements[index].Weight;
            TotalWeight += weight;

            WeightedElement<T> element = Elements[index];
            element.Weight = weight;
            Elements[index] = element;
        }

        public void Add(T item, int weight)
        {
            if (weight < 1)
            {
                throw new ArgumentException("weight can't be less than 1.");
            }

            if (CumulativeWeight.Count < 1)
            {
                CumulativeWeight.Add(weight);
            }
            else
            {
                CumulativeWeight.Add(weight + CumulativeWeight[CumulativeWeight.Count - 1]);
            }

            TotalWeight += weight;

            Elements.Add(new WeightedElement<T>(item, weight));
        }

        public void Add(T item)
        {
            if (CumulativeWeight.Count < 1)
            {
                CumulativeWeight.Add(1);
            }
            else
            {
                CumulativeWeight.Add(1 + CumulativeWeight[CumulativeWeight.Count - 1]);
            }

            TotalWeight += 1;

            Elements.Add(new WeightedElement<T>(item));
        }

        public void RemoveAt(int index)
        {
            int elementWeight = Elements[index].Weight;

            Elements.RemoveAt(index);

            for (int i = index + 1; i < CumulativeWeight.Count; i++)
            {
                CumulativeWeight[i] -= elementWeight;
            }
            CumulativeWeight.RemoveAt(index);

            TotalWeight -= elementWeight;
        }

        public void Clear()
        {
            CumulativeWeight.Clear();

            TotalWeight = 0;

            Elements.Clear();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return Elements.Select(e => e.Element).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Elements.Select(e => e.Element).GetEnumerator();

        }
    }
}
