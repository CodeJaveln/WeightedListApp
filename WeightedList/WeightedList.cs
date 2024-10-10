using System;
using System.Collections;
using System.Collections.Generic;

namespace WeightedListApp
{
    public class WeightedList<T> : ICollection<WeightedElement<T>>
    {
        // Default weight is 1 which is the same as default randomness

        private List<WeightedElement<T>> Elements;

        public int Count => Elements.Count;

        public bool IsReadOnly => false;

        public int TotalWeight;
        private List<int> CumulativeWeight;

        public WeightedList()
            : this(4)
        {
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

            int randomValue = random.Next(0, TotalWeight);

            int leftIndex = 0, rightIndex = CumulativeWeight.Count - 1;

            while (true)
            {
                int middleIndex = (leftIndex + rightIndex) >> 1;

                // Since dividing the sum of two ints by 2 when they are equal to: largerInt + (largerInt - 1)
                // It would always equal largerInt - 1 and therefor there are no ints between the two ints 
                // Which assures the most accurate value is rightIndex's weight to the randomValue
                if (middleIndex == leftIndex)
                {
                    return Elements[rightIndex].Element;
                }
                else if (CumulativeWeight[middleIndex] == randomValue)
                {
                    return Elements[middleIndex].Element;
                }
                else if (CumulativeWeight[middleIndex] > randomValue)
                {
                    rightIndex = middleIndex;
                }
                else
                {
                    leftIndex = middleIndex;
                }
            }

            // Something went wrong if it gets here
            throw new InvalidOperationException("Error during randomizing object");
        }

        public T GetRandomItem(int seed)
        {
            return GetRandomItem(new Random(seed));
        }

        public T GetRandomItem()
        {
            return GetRandomItem(new Random());
        }

        public void SetElement(T element, int index)
        {
            if (index < 0 || index >= Elements.Count)
            {
                throw new IndexOutOfRangeException();
            }
            else if (element == null)
            {
                throw new ArgumentNullException();
            }

            Elements[index].Element = element;
        }

        public void SetWeight(int weight, int index)
        {
            if (index < 0 || index >= Elements.Count)
            {
                throw new IndexOutOfRangeException();
            }
            else if (weight < 1)
            {
                throw new ArgumentException("weight can't be less than 1.");
            }

            for (int i = index; i < CumulativeWeight.Count; i++)
            {
                CumulativeWeight[i] += weight - Elements[index].Weight;
            }

            TotalWeight -= Elements[index].Weight;
            TotalWeight += weight;

            Elements[index].Weight = weight;
        }

        public void Add(WeightedElement<T> element)
        {
            if (element == null)
            {
                throw new ArgumentNullException();
            }
            else if (element.Weight < 1)
            {
                throw new ArgumentException("weight can't be less than 1.");
            }

            if (CumulativeWeight.Count < 1)
            {
                CumulativeWeight.Add(element.Weight);
            }
            else
            {
                CumulativeWeight.Add(element.Weight + CumulativeWeight[CumulativeWeight.Count - 1]);
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

        public bool Remove(WeightedElement<T> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }

            int itemIndex = Elements.IndexOf(item);
            if (itemIndex == -1)
            {
                return false;
            }

            if (!Elements.Remove(item))
            {
                throw new Exception("Elements contains item but couldn't remove item.");
            }

            for (int i = itemIndex + 1; i < CumulativeWeight.Count; i++)
            {
                CumulativeWeight[i] -= item.Weight;
            }
            CumulativeWeight.RemoveAt(itemIndex);

            TotalWeight -= item.Weight;

            return true;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Elements.Count)
            {
                throw new IndexOutOfRangeException();
            }

            int elementWeight = Elements[index].Weight;

            Elements.RemoveAt(index);

            for (int i = index + 1; i < CumulativeWeight.Count; i++)
            {
                CumulativeWeight[i] -= elementWeight;
            }
            CumulativeWeight.RemoveAt(index);

            TotalWeight -= elementWeight;
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
            CumulativeWeight.Clear();

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
