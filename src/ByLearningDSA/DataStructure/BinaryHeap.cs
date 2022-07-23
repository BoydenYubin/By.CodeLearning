using System;

namespace ByLearningDSA.DataStructure
{
    public class BinaryHeap<T>
    {
        public void UpAdjust(IComparable<T>[] array)
        {
            int childIndex = array.Length - 1;
            int parentIndex = (childIndex - 1) / 2;
            IComparable<T> temp = array[childIndex];
            while (childIndex > 0 && array[parentIndex].CompareTo((T)temp) > 0)
            {
                array[childIndex] = array[parentIndex];
                childIndex = parentIndex;
                parentIndex = (parentIndex - 1) / 2;
            }
            array[parentIndex] = temp;
        }
        public void DownAdjust(IComparable<T>[] array, int parentIndex, int length)
        {
            IComparable<T> temp = array[parentIndex];
            int childIndex = 2 * parentIndex + 1;
            while (childIndex < length)
            {
                if (childIndex + 1 < length && array[childIndex].CompareTo((T)array[childIndex + 1]) > 0)
                {
                    childIndex++;
                }
                if (temp.CompareTo((T)array[childIndex]) <= 0)
                    break;
                array[parentIndex] = array[childIndex];
                parentIndex = childIndex;
                childIndex = 2 * childIndex + 1;
            }
            array[parentIndex] = temp;
        }

        public void BuildHeap(IComparable<T>[] array)
        {
            for (int i = (array.Length - 2) / 2; i >= 0; i--)
            {
                DownAdjust(array, i, array.Length);
            }
        }
    }
}