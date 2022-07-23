using ByLearningDSA.LeetCode.UtilClass;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace ByLearningDSA.LeetCode
{
    public class LeetCode0201_0250Test
    {
        /// <summary>
        /// https://leetcode-cn.com/problems/maximal-square/
        /// 在一个由 '0' 和 '1' 组成的二维矩阵内，找到只包含 '1' 的最大正方形，并返回其面积。
        /// </summary>
        [Fact]
        public void L0221_MaximalSquare()
        {
            char[][] data = new char[4][];
            data[0] = new char[] { '1', '0', '1', '0', '0' };
            data[1] = new char[] { '1', '0', '1', '1', '1' };
            data[2] = new char[] { '1', '1', '1', '1', '1' };
            data[3] = new char[] { '1', '0', '0', '1', '0' };
            var result = MaximalSquare(data);
            result.ShouldBe(4);
            data = new char[5][];
            data[0] = new char[] { '1', '1', '1', '1', '0' };
            data[1] = new char[] { '1', '1', '1', '1', '0' };
            data[2] = new char[] { '1', '1', '1', '1', '1' };
            data[3] = new char[] { '1', '1', '1', '1', '1' };
            data[4] = new char[] { '0', '0', '1', '1', '1' };
            result = MaximalSquare(data);
            result.ShouldBe(16);

        }
        private int MaximalSquare(char[][] matrix)
        {
            if (matrix.Length == 0)
                return 0;
            int row = matrix.Length;           //矩阵行          
            int col = matrix[0].Length;        //矩阵的列值
            int[,] data = new int[row, col];
            int result = 0;   //记录最大值
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (matrix[i][j] == '0')
                    {
                        data[i, j] = 0;
                    }
                    else
                    {
                        if (i - 1 < 0 || j - 1 < 0)
                        {
                            data[i, j] = 1;
                            result = result > 1 ? result : 1;
                            continue;
                        }
                        if (data[i - 1, j] >= data[i - 1, j - 1] && data[i, j - 1] >= data[i - 1, j - 1])
                        {
                            data[i, j] = data[i - 1, j - 1] + 1;
                            result = Math.Max(result, data[i, j]);
                        }
                        else
                        {
                            data[i, j] = Math.Min(data[i - 1, j], data[i, j - 1]) + 1;
                            result = Math.Max(result, data[i, j]);
                        }
                    }
                }
            }
            return result * result;
        }
        [Fact]
        public void L0222_CountNodes()
        {
            var root = TreeNode.CreateTree(new object[] { 1, 2, 3, 4, 5, 6, null });
            var result = CountNodes(root);
            result.ShouldBe(6);
        }
        private int CountNodes(TreeNode node)
        {
            if (node == null)
                return 0;
            int level_count = 0;
            int result = 0;
            Queue<TreeNode> nodes = new Queue<TreeNode>();
            nodes.Enqueue(node);
            bool jump = false;
            while (nodes.Count > 0 && !jump)
            {
                level_count = nodes.Count;
                for (int i = 0; i < level_count; i++)
                {
                    var temp = nodes.Dequeue();
                    result += 1;
                    if (temp.left != null && temp.right != null)
                    {
                        nodes.Enqueue(temp.left);
                        nodes.Enqueue(temp.right);
                    }
                    else
                    {
                        if (temp.left == null)
                        {
                            result += level_count - i - 1;
                            result += 2 * i;
                            jump = true;
                            break;
                        }
                        if (temp.right == null)
                        {
                            result += level_count - i - 1;
                            result += 2 * i + 1;
                            jump = true;
                            break;
                        }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/invert-binary-tree/
        /// 翻转一棵二叉树。
        /// </summary>
        [Fact]
        public void L0226_InvertTree()
        {
            var root = TreeNode.CreateTree(new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });
            InvertTree(root);
        }
        private TreeNode InvertTree(TreeNode root)
        {
            SwapLeftNodeToRight(root);
            return root;
        }
        private void SwapLeftNodeToRight(TreeNode root)
        {
            if (root == null)
                return;
            SwapLeftNodeToRight(root.left);
            SwapLeftNodeToRight(root.right);
            var tmp = root.left;
            root.left = root.right;
            root.right = tmp;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/kth-smallest-element-in-a-bst/
        /// 给定一个二叉搜索树的根节点 root ，和一个整数 k ，请你设计一个算法查找其中第 k 个最小元素（从 1 开始计数）
        /// </summary>
        [Fact]
        public void L0230_KthSmallest()
        {
            var root = TreeNode.CreateTree(new object[] { 3, 1, 4, null, 2 });
            var result = KthSmallest(root, 2);
            result.ShouldBe(2);
            result = KthSmallest(root, 3);
            result.ShouldBe(3);
        }
        private int KthSmallest(TreeNode root, int k)
        {
            Stack<TreeNode> stack = new Stack<TreeNode>();
            while (root != null || stack.Count > 0)
            {
                while (root != null)
                {
                    stack.Push(root);
                    root = root.left;
                }
                if (stack.Count > 0)
                {
                    var tmp = stack.Pop();
                    if (--k == 0) return tmp.val;
                    root = tmp.right;
                }
            }
            return 0;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/lowest-common-ancestor-of-a-binary-search-tree/
        /// 给定一个二叉搜索树, 找到该树中两个指定节点的最近公共祖先。
        ///百度百科中最近公共祖先的定义为：“对于有根树 T 的两个结点 p、q，
        ///最近公共祖先表示为一个结点 x，满足 x 是 p、q 的祖先且 x 的深度尽可能大（一个节点也可以是它自己的祖先）。”
        /// </summary>
        [Fact]
        public void L0235_LowestCommonAncestor()
        {
            var root = TreeNode.CreateTree(new object[] { 6, 2, 8, 0, 4, 7, 9, null, null, 3, 5 });
            var result = LowestCommonAncestor_235(root, root.left.left, root.left.right.right);
            result.val.ShouldBe(2);
        }
        private TreeNode LowestCommonAncestor_235(TreeNode root, TreeNode p, TreeNode q)
        {
            //解析参考https://leetcode-cn.com/problems/lowest-common-ancestor-of-a-binary-search-tree/solution/er-cha-sou-suo-shu-de-zui-jin-gong-gong-zu-xian-26/
            if (root.val > p.val && root.val > q.val)
            {
                return LowestCommonAncestor_235(root.left, p, q);
            }
            else if (root.val > p.val && root.val > q.val)
            {
                return LowestCommonAncestor_235(root.right, p, q);
            }
            else
            {
                return root;
            }
        }
        [Fact]
        public void L0236_LowestCommonAncestor()
        {

        }
        private TreeNode LowestCommonAncestor_236(TreeNode root, TreeNode p, TreeNode q)
        {
            return null;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/binary-tree-paths/
        /// 给定一个二叉树，返回所有从根节点到叶子节点的路径。
        /// </summary>
        [Fact]
        public void L0257_BinaryTreePaths()
        {
            var root = TreeNode.CreateTree(new object[] { 1, 2, 3, null, 5 });
            var result = BinaryTreePaths(root);
            // 1->2->5  和  1->3
            result.Count.ShouldBe(2);
        }
        private IList<string> BinaryTreePaths(TreeNode root)
        {
            var list = new List<string>();
            var oriStr = "";
            if (root == null)
                return list;
            BinaryTreePaths(root, list, oriStr);
            return list;
        }
        private void BinaryTreePaths(TreeNode root, IList<String> result, string input)
        {
            //先添加路径 1
            input += root.val.ToString();
            //如果到了叶子节点，则返回添加结果到集合中
            if (root.left == null && root.right == null)
            {
                result.Add(input);
                return;
            }
            //仍有叶子节点则继续前进，增加路线指引"->"
            input += "->";
            if (root.left != null)
            {
                BinaryTreePaths(root.left, result, input);
            }
            if (root.right != null)
            {
                BinaryTreePaths(root.right, result, input);
            }
        }
        [Fact]
        public void L0279_MaximalSquare()
        {
            var result = NumSquares(11);
            result.ShouldBe(3);
            result = NumSquares(0);
            result.ShouldBe(0);
            result = NumSquares(12);
            result.ShouldBe(4);
        }
        private int NumSquares(int n)
        {
            //特殊情况 0
            if (n == 0)
                return 0;
            //求得距离n最近的平方数floor
            //比如说11最近的平方数是9，则floor=3
            var sqrtNum = Math.Sqrt(n);
            var floor = Math.Floor(sqrtNum);
            //如果n直接可以开平方则返回1
            if (sqrtNum == floor)
            {
                return 1;
            }
            //否则情况是，比如11=9+2
            else
            {
                //先计算去掉最大平方根后的值leftValue，比如11-9=2
                var tmp = Math.Pow(floor, 2);
                var leftValue = Math.Sqrt(n - tmp);
                //如果剩余值n - tmp可以直接开平方根，则返回2
                if (leftValue == (int)leftValue)
                {
                    return 2;
                }
                //否则则根据剩余值的大小进行归纳
                else
                {
                    //如果剩余值为1，2，3，则直接加上该值
                    //因为1，2，3 只能分解为该值个1的和
                    if (n - tmp < 4)
                    {
                        return 1 + n - (int)tmp;
                    }
                    //如果剩余值大于3，则继续判断
                    //
                    return 1 + NumSquares(n - (int)Math.Pow(floor - 1, 2));
                }
            }
        }
        [Theory]
        [InlineData(new int[] { 10, 9, 2, 5, 3, 7, 101, 18 }, 4)]
        [InlineData(new int[] { 0, 1, 0, 3, 2, 3 }, 4)]
        [InlineData(new int[] { 4, 10, 4, 3, 8, 9 }, 3)]
        public void L0300_LengthOfLIS(int[] nums, int excepted)
        {
            var result = LengthOfLIS(nums);
            result.ShouldBe(excepted);
        }
        private int LengthOfLIS(int[] nums)
        {
            if (nums.Length == 0)
                return 0;
            int[] dp = new int[nums.Length];
            Array.Fill(dp, 1);
            int result = 1;
            //顺序遍历，所有的nums数据集
            for (int i = 1; i < nums.Length; i++)
            {
                //倒叙查找数字前所有小于它值的最大一个
                for (int j = i - 1; j >= 0; j--)
                {
                    if (nums[i] > nums[j])
                    {
                        dp[i] = dp[i] > dp[j] + 1 ? dp[i] : dp[j] + 1;
                    }
                }
                result = result > dp[i] ? result : dp[i];
            }
            return result;
            //if (nums.Length == 0)
            //    return 0;
            //Stack<int> stack = new Stack<int>();
            //int result = 1;
            //stack.Push(nums[0]);
            //for (int i = 1; i < nums.Length; i++)
            //{
            //    if(stack.Peek() < nums[i])
            //    {
            //        stack.Push(nums[i]);
            //    }
            //    else
            //    {
            //        result = result > stack.Count ? result : stack.Count;
            //        while(stack.Count >0 && stack.Peek() >= nums[i])
            //        {
            //            stack.Pop();
            //        }
            //        stack.Push(nums[i]);
            //    }
            //}
            //result = result > stack.Count ? result : stack.Count;
            //return result;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/sum-of-left-leaves/
        /// 计算给定二叉树的所有左叶子之和。
        /// </summary>
        [Fact]
        public void L0404_SumOfLeftLeaves()
        {
            var root = TreeNode.CreateTree(new object[] { 3, 9, 20, 13, null, 15, 7 });
            var result = SumOfLeftLeaves(root);
            result.ShouldBe(28);
        }
        private int SumOfLeftLeaves(TreeNode root)
        {
            if (root == null) return 0;
            int sum = 0;
            //如果该分支的左叶子不为空，且左叶子的左右叶子都为空，则可以加上该叶子
            if (root.left != null && root.left.left == null
                && root.left.right == null)
            {
                sum += root.left.val;
            }
            //否则就需要计算左叶子的和
            else
            {
                sum += SumOfLeftLeaves(root.left);
            }
            //计算完左叶子的和再计算右叶子的和
            sum += SumOfLeftLeaves(root.right);
            return sum;
        }
    }
}
