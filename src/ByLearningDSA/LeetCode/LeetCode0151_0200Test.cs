using ByLearningDSA.LeetCode.UtilClass;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace ByLearningDSA.LeetCode
{
    public class LeetCode0151_0200Test
    {
        /// <summary>
        /// https://leetcode-cn.com/problems/maximum-product-subarray/
        /// 给你一个整数数组 nums ，请你找出数组中乘积最大的连续子数组（该子数组中至少包含一个数字），并返回该子数组所对应的乘积。
        /// </summary>
        [Fact]
        public void L0152_MaxProduct()
        {
            //注意负负得正
            var result = MaxProduct(new int[] { -2, 3, -1 });
            result.ShouldBe(6);
        }
        private int MaxProduct(int[] nums)
        {
            int max = int.MinValue, imax = 1, imin = 1;
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] < 0)
                {
                    int tmp = imax;
                    imax = imin;
                    imin = tmp;
                }
                imax = Math.Max(imax * nums[i], nums[i]);
                imin = Math.Min(imin * nums[i], nums[i]);

                max = Math.Max(max, imax);
            }
            return max;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/binary-tree-right-side-view/
        /// 给定一棵二叉树，想象自己站在它的右侧，按照从顶部到底部的顺序，返回从右侧所能看到的节点值。
        /// </summary>
        [Fact]
        public void L0199_RightSideView()
        {
            var root = TreeNode.CreateTree(new object[] { 1, 2, 3, null, 5, null, 4 });
            var result = RightSideView(root);
            result.Count.ShouldBe(3);
        }
        private IList<int> RightSideView(TreeNode root)
        {
            List<int> result = new List<int>();
            if (root == null)
                return result;
            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                int level_count = queue.Count;
                for (int i = 0; i < level_count; i++)
                {
                    var temp = queue.Dequeue();
                    if (temp.left != null)
                        queue.Enqueue(temp.left);
                    if (temp.right != null)
                        queue.Enqueue(temp.right);
                    if (i == level_count - 1)
                        result.Add(temp.val);
                }
            }
            return result;
        }
    }
}
