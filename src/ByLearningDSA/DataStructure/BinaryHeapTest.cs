using System;
using Xunit;

namespace ByLearningDSA.DataStructure
{
    public class BinaryHeapTest
    {
        [Fact]
        public void UpAdjustTest()
        {
            IComparable<int>[] testData = new IComparable<int>[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            BinaryHeap<int> binaryHeap = new BinaryHeap<int>();
            binaryHeap.UpAdjust(testData);
            testData = new IComparable<int>[] { 7, 1, 3, 10, 5, 2, 8, 9, 6 };
            binaryHeap.BuildHeap(testData);
        }
    }
}
