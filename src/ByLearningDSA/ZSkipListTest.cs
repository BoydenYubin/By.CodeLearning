using ByLearningDSA.RedisSource;
using System;
using Xunit;

namespace ByLearningDSA
{
    public class ZSkipListTest
    {
        private ZSkipList zSkipList;
        public ZSkipListTest()
        {
            zSkipList = ZSkipList.CreateList();
        }
        [Fact]
        public void InsertNodeTest()
        {
            for (int i = 1; i <= 100; i++)
            {
                zSkipList.ZslInsert(i, i);
            }
            Assert.Equal(100, (int)zSkipList.Length);
            Assert.True(zSkipList.Level < 32);
        }

        [Fact]
        public void InsertNodeWithStringTest()
        {
            zSkipList.ZslInsert(1, "a");
            zSkipList.ZslInsert(2, "b");
            zSkipList.ZslInsert(3, "c");
            zSkipList.ZslInsert(5, "e");
            zSkipList.ZslInsert(6, "f");

            zSkipList.ZslInsert(4, "d");
        }
    }
}
