using System.Collections.Generic;

namespace ByLearningDSA.LeetCode.UtilClass
{
    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
        {
            this.val = val;
            this.left = left;
            this.right = right;
        }
        /// <summary>
        /// 需包含所有的节点信息
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static TreeNode CreateTree(object[] nums)
        {
            if (nums.Length == 0)
                return null;
            TreeNode[] trees = new TreeNode[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] != null)
                {
                    trees[i] = new TreeNode((int)nums[i]);
                }
            }
            for (int i = 0; i < (trees.Length - 1) / 2; i++)
            {
                if (trees[i] != null)
                {
                    trees[i].left = trees[2 * i + 1];
                    trees[i].right = trees[2 * i + 2];
                }
            }
            return trees[0];
        }
    }

    public class Node
    {
        public int val;
        public Node left;
        public Node right;
        public Node next;
        public Node() { }
        public Node(int _val)
        {
            val = _val;
        }
        public Node(int _val, Node _left, Node _right, Node _next)
        {
            val = _val;
            left = _left;
            right = _right;
            next = _next;
        }
        public static Node CreateNode(object[] nums)
        {
            if (nums.Length == 0)
                return null;
            Node[] trees = new Node[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] != null)
                {
                    trees[i] = new Node((int)nums[i]);
                }
            }
            for (int i = 0; i < (trees.Length - 1) / 2; i++)
            {
                trees[i].left = trees[2 * i + 1];
                trees[i].right = trees[2 * i + 2];
            }
            return trees[0];
        }
    }
}
