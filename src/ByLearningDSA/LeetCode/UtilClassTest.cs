using ByLearningDSA.LeetCode.UtilClass;
using Shouldly;
using Xunit;

namespace ByLearningDSA.LeetCode
{
    public class UtilClassTest
    {
        [Fact]
        public void CreateTreesTest()
        {
            object[] nums = new object[] { 3, 9, 20, null, null, 15, 7 };
            var result = TreeNode.CreateTree(nums);
            result.left.val.ShouldBe(9);
            result.right.right.val.ShouldBe(7);
        }
    }
}
