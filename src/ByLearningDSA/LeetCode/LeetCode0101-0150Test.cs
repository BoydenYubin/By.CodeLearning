using ByLearningDSA.LeetCode.UtilClass;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace ByLearningDSA.LeetCode
{
    public class LeetCode0101_0150Test
    {
        [Fact]
        public void L0101_IsSymmetric()
        {

        }
        private bool IsSymmetric(TreeNode root)
        {
            if (root == null)
            {
                return true;
            }
            Queue<TreeNode> leftTree = new Queue<TreeNode>();
            Queue<TreeNode> rightTree = new Queue<TreeNode>();
            leftTree.Enqueue(root.left);
            rightTree.Enqueue(root.right);
            while (leftTree.Count > 0 && rightTree.Count > 0)
            {
                var p = leftTree.Dequeue();
                var q = rightTree.Dequeue();
                if (p == null && q == null)
                {
                    continue;
                }
                else
                {
                    if (p == null || q == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (p.val == q.val)
                        {
                            leftTree.Enqueue(p.left);
                            leftTree.Enqueue(p.right);
                            rightTree.Enqueue(q.right);
                            rightTree.Enqueue(q.left);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/binary-tree-level-order-traversal/
        /// 给你一个二叉树，请你返回其按 层序遍历 得到的节点值。 （即逐层地，从左到右访问所有节点）。
        /// </summary>
        [Fact]
        public void L0102_LevelOrder()
        {

        }
        private IList<IList<int>> LevelOrder(TreeNode root)
        {

            List<IList<int>> result = new List<IList<int>>();
            if (root == null)
                return result;
            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                int levelNumber = queue.Count;
                var tmp = new List<int>();
                for (int i = 0; i < levelNumber; i++)
                {
                    var node = queue.Dequeue();
                    tmp.Add(node.val);
                    if (node.left != null)
                        queue.Enqueue(node.left);
                    if (node.right != null)
                        queue.Enqueue(node.right);
                }
                result.Add(tmp);
            }
            return result;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/binary-tree-zigzag-level-order-traversal/
        /// 按照下面方法可以，也可以使用双端队列<see cref="LinkedListNode{T}"/>
        /// </summary>
        [Fact]
        public void L0103_ZigzagLeverOrder()
        {
            TreeNode root = new TreeNode(1)
            {
                left = new TreeNode(2)
                {
                    left = new TreeNode(4)
                    {
                        left = new TreeNode(8),
                        right = new TreeNode(9)
                    },
                    right = new TreeNode(5)
                    {
                        left = new TreeNode(10),
                        right = new TreeNode(11)
                    }
                },
                right = new TreeNode(3)
                {
                    left = new TreeNode(6)
                    {
                        left = new TreeNode(12),
                        right = new TreeNode(13)
                    },
                    right = new TreeNode(7)
                    {
                        left = new TreeNode(14),
                        right = new TreeNode(15)
                    }
                }
            };
            var result = ZigzagLeverOrde(root);
        }
        private IList<IList<int>> ZigzagLeverOrde(TreeNode root)
        {
            List<IList<int>> result = new List<IList<int>>();
            if (root == null)
                return result;
            Queue<Stack<TreeNode>> qs = new Queue<Stack<TreeNode>>();
            Stack<TreeNode> levleStack = new Stack<TreeNode>();
            levleStack.Push(root);
            qs.Enqueue(levleStack);

            bool isOrder = true;
            while (qs.Peek().Count > 0)
            {
                var tmpStack = qs.Dequeue();
                levleStack = new Stack<TreeNode>();
                int count = tmpStack.Count;
                List<int> levelList = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    var tmp = tmpStack.Pop();
                    levelList.Add(tmp.val);
                    if (isOrder)
                    {
                        if (tmp.left != null)
                            levleStack.Push(tmp.left);
                        if (tmp.right != null)
                            levleStack.Push(tmp.right);
                    }
                    else
                    {
                        if (tmp.right != null)
                            levleStack.Push(tmp.right);
                        if (tmp.left != null)
                            levleStack.Push(tmp.left);
                    }
                }
                result.Add(levelList);
                qs.Enqueue(levleStack);
                isOrder = !isOrder;
            }
            return result;
        }
        [Fact]
        public void L0104_MaxDepth()
        {
            TreeNode root = new TreeNode(3)
            {
                left = new TreeNode(9),
                right = new TreeNode(20)
                {
                    left = new TreeNode(15),
                    right = new TreeNode(7)
                }
            };
            MaxDepth(root);
        }
        private int MaxDepth(TreeNode root)
        {
            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(root);
            int depth = 0;
            int levelCount = 0;
            while (queue.Count > 0)
            {
                depth++;
                levelCount = queue.Count;
                for (int i = 0; i < levelCount; i++)
                {
                    var tmp = queue.Dequeue();
                    if (tmp != null)
                    {
                        queue.Enqueue(tmp.left);
                        queue.Enqueue(tmp.right);
                    }
                }
            }
            return depth - 1;
        }
        [Fact]
        public void L0107_LevelOrderBottom()
        {
            TreeNode root = new TreeNode(3)
            {
                left = new TreeNode(9),
                right = new TreeNode(20)
                {
                    left = new TreeNode(15),
                    right = new TreeNode(7)
                }
            };
            LevelOrderBottom(root);
        }
        private IList<IList<int>> LevelOrderBottom(TreeNode root)
        {
            Stack<Queue<TreeNode>> outputs = new Stack<Queue<TreeNode>>();
            Queue<TreeNode> loopNodes = new Queue<TreeNode>();
            loopNodes.Enqueue(root);
            while (loopNodes.Count > 0)
            {
                Queue<TreeNode> populate = new Queue<TreeNode>();
                int count = loopNodes.Count;
                for (int i = 0; i < count; i++)
                {
                    var tmp = loopNodes.Dequeue();
                    if (tmp != null)
                    {
                        populate.Enqueue(tmp);
                        loopNodes.Enqueue(tmp.left);
                        loopNodes.Enqueue(tmp.right);
                    }
                }
                if (populate.Count > 0)
                {
                    outputs.Push(populate);
                }
            }
            List<IList<int>> result = new List<IList<int>>();
            while (outputs.Count > 0)
            {
                List<int> tmp = new List<int>();
                var queue = outputs.Pop();
                while (queue.Count > 0)
                {
                    tmp.Add(queue.Dequeue().val);
                }
                result.Add(tmp);
            }
            return result;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/convert-sorted-array-to-binary-search-tree/
        /// 将一个按照升序排列的有序数组，转换为一棵高度平衡二叉搜索树。
        /// 本题中，一个高度平衡二叉树是指一个二叉树每个节点 的左右两个子树的高度差的绝对值不超过 1。
        /// </summary>
        [Fact]
        public void L0108_SortedArrayToBST()
        {
            SortedArrayToBST(new int[] { -10, -3, -1, 0, 5, 7, 9, 12 });
        }
        private TreeNode SortedArrayToBST(int[] nums)
        {
            //不要忘记空数组
            if (nums.Length == 0)
                return null;
            return SortedArray(nums, 0, nums.Length - 1);
        }
        private TreeNode SortedArray(int[] nums, int start, int end)
        {
            TreeNode treeNode;
            if (end - start > 0)
            {
                //当数组数量大于1个时，需要将数组分开继续判断，例如1,2,3
                //选取2作为树的根节点
                //单数时，取中间的值，比如1,2,3,4,5，选3
                //比如说1,2,3,4,5,6,7,8,则选取5
                int mid = (end - start) % 2 == 0 ? (end - start) / 2 + start : (end - start) / 2 + start + 1;
                //如果数组仅剩下2个，比如说1,2
                //则直接选取较大值作为根节点，较小值作为左子树返回
                if (mid == end)
                {
                    treeNode = new TreeNode(nums[end]);
                    treeNode.left = new TreeNode(nums[start]);
                    return treeNode;
                }
                treeNode = new TreeNode(nums[mid]);
                //如果说数组需要继续判断，则递归调用，例如1,2,3,4,5
                //选取2作为根节点，1，2作为左子树继续递归，4,5作为右子树继续递归
                treeNode.left = SortedArray(nums, start, mid - 1);
                treeNode.right = SortedArray(nums, mid + 1, end);
            }
            else
            {
                //仅剩下一个值时，直接返回一个数
                treeNode = new TreeNode(nums[start]);
            }
            //最终结果
            return treeNode;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/balanced-binary-tree/
        /// 给定一个二叉树，判断它是否是高度平衡的二叉树。
        /// 一棵高度平衡二叉树定义为：一个二叉树每个节点 的左右两个子树的高度差的绝对值不超过 1 。
        /// </summary>
        [Fact]
        public void L0110_IsBalanced()
        {
            var root = TreeNode.CreateTree(new object[] { 1, 2, 2, 3, 3, null, null, 4, 4 });
            var result = IsBalanced(root);
            result.ShouldBe(false);
            root = TreeNode.CreateTree(new object[] { 1, 2, 2, 3, null, null, 3, 4, null, null, null, null, null, null, 4 });
            result = IsBalanced(root);
            result.ShouldBe(false);
        }
        private bool IsBalanced(TreeNode root)
        {
            if (root == null)
            {
                return true;
            }
            return IsBalanced(root.left) && IsBalanced(root.right) && Math.Abs(L0110Height(root.left) - L0110Height(root.right)) <= 1;
        }
        private int L0110Height(TreeNode root)
        {
            if (root == null)
                return 0;
            return Math.Max(L0110Height(root.left), L0110Height(root.right)) + 1;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/minimum-depth-of-binary-tree/
        /// 给定一个二叉树，找出其最小深度。
        /// 最小深度是从根节点到最近叶子节点的最短路径上的节点数量。
        /// </summary>
        [Fact]
        public void L0111_MinDepth()
        {
            var root = TreeNode.CreateTree(new object[] { 3, 9, 20, null, null, 15, 7 });
            var result = MinDepth(root);
            result.ShouldBe(2);
            root = TreeNode.CreateTree(new object[] { 
                                                      2,
                                 null,                                      3,
                        null,              null,                null,                4,
                   null,    null,     null,     null,      null,     null,     null,     5,
                null,null,null,null,null,null,null,null,null,null,null,null,null,null,null, 6 });
            result = MinDepth(root);
            result.ShouldBe(5);
            root = TreeNode.CreateTree(new object[] { 
                               -9,
                        -3,                2, 
                   null,     4,       4,        0, //这里是最少路径 
                null,null,-6, null, -5,null,null,null });
            result = MinDepth(root);
            result.ShouldBe(3);
        }
        private int MinDepth(TreeNode root)
        {

            if (root == null) return 0;
            int m1 = MinDepth(root.left);
            int m2 = MinDepth(root.right);
            //1.如果左孩子和右孩子有为空的情况，直接返回m1+m2+1
            //2.如果都不为空，返回较小深度+1
            return root.left == null || root.right == null ? m1 + m2 + 1 : Math.Min(m1, m2) + 1;
        }
        [Fact]
        public void L0116_PathSum()
        {
            //举例1为正常
            TreeNode root = new TreeNode(5)
            {
                left = new TreeNode(4)
                {
                    left = new TreeNode(11)
                    {
                        left = new TreeNode(7),
                        right = new TreeNode(2)
                    }
                },
                right = new TreeNode(8)
                {
                    left = new TreeNode(13),
                    right = new TreeNode(4)
                    {
                        left = new TreeNode(5),
                        right = new TreeNode(1)
                    }
                }
            };
            var result = PathSum(root, 22);
            root = new TreeNode(-2)
            {
                right = new TreeNode(-3)
            };
            //举例2位负数
            result = PathSum(root, -5);
            //举例3为中间节点遇到值为0
            root = new TreeNode(1)
            {
                left = new TreeNode(-2)
                {
                    left = new TreeNode(1)
                    {
                        left = new TreeNode(-1),
                    },
                    right = new TreeNode(3)
                },
                right = new TreeNode(-3)
                {
                    left = new TreeNode(-2)
                }
            };
            result = PathSum(root, -1);
        }
        private IList<IList<int>> PathSum(TreeNode root, int targetSum)
        {
            IList<IList<int>> result = new List<IList<int>>();
            IList<int> path = new List<int>();
            InnerPathSum(root, targetSum, path, result);
            return result;
        }
        private void InnerPathSum(TreeNode root, int targetsum, IList<int> path, IList<IList<int>> results)
        {
            if (root == null)
                return;
            path.Add(root.val);
            if (root.left == null && root.right == null && root.val - targetsum == 0)
            {
                var result = new List<int>(path);
                results.Add(result);
                path.RemoveAt(path.Count - 1);
                return;
            }
            InnerPathSum(root.left, targetsum - root.val, path, results);
            InnerPathSum(root.right, targetsum - root.val, path, results);
            path.RemoveAt(path.Count - 1);
        }
        [Fact]
        public void L0114_Flatten()
        {
            TreeNode root = new TreeNode(1)
            {
                left = new TreeNode(2)
                {
                    left = new TreeNode(3),
                    right = new TreeNode(4)
                },
                right = new TreeNode(5)
                {
                    right = new TreeNode(6)
                }
            };
            Flatten(root);
        }
        private void Flatten(TreeNode root)
        {
            Stack<TreeNode> trees = new Stack<TreeNode>();
            PreOrder(root, trees);
            if (trees.Count < 2)
            {
                return;
            }
            TreeNode temp = trees.Pop();
            while (trees.Count > 0)
            {
                var tem = trees.Pop();
                tem.left = null;
                tem.right = temp;
                temp = tem;
            }
        }
        private void PreOrder(TreeNode root, Stack<TreeNode> trees)
        {
            if (root == null)
            {
                return;
            }
            trees.Push(root);
            PreOrder(root.left, trees);
            PreOrder(root.right, trees);
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/populating-next-right-pointers-in-each-node/
        /// </summary>
        [Fact]
        public void L0116_Connect()
        {
            var root = new Node(1)
            {
                left = new Node(2)
                {
                    left = new Node(4),
                    right = new Node(5)
                },
                right = new Node(3)
                {
                    left = new Node(6),
                    right = new Node(7)
                }
            };
            ConnectI(root);
        }
        private Node ConnectI(Node root)
        {
            var queue = new Queue<Node>();
            if (root == null)
                return root;
            queue.Enqueue(root);
            int count = queue.Count;
            while (count != 0)
            {
                var tmp = queue.Dequeue();
                if (tmp.left != null)
                    queue.Enqueue(tmp.left);
                if (tmp.right != null)
                    queue.Enqueue(tmp.right);
                for (int i = 0; i < count - 1; i++)
                {
                    var temp = queue.Dequeue();
                    if (temp.left != null)
                        queue.Enqueue(temp.left);
                    if (temp.right != null)
                        queue.Enqueue(temp.right);
                    tmp.next = temp;
                    tmp = temp;
                }
                count = queue.Count;
            }
            return root;
        }
        [Fact]
        public void L0117_Connect()
        {
            object[] nums = new object[] { 1, 2, 3, 4, 5, null, 7 };
            var root = Node.CreateNode(nums);
            var result = ConnectII(root);

        }
        private Node ConnectII(Node root)
        {
            if (root == null)
                return root;
            Node start = root;
            Node last = null, nextStart = null;
            var handle = new Action<Node>(node =>
            {
                if (last != null)
                    last.next = node;
                if (nextStart == null)
                    nextStart = node;
                last = node;
            });
            while (start != null)
            {
                last = null;
                nextStart = null;
                for (Node p = start; p != null; p = p.next)
                {
                    if (p.left != null)
                        handle(p.left);
                    if (p.right != null)
                        handle(p.right);
                    start = nextStart;
                }
            }
            return root;
        }

        [Fact]
        public void L0120_MinimumTotal()
        {
            IList<int> a = new List<int> { 2 };
            IList<int> b = new List<int> { 3, 4 };
            IList<int> c = new List<int> { 6, 5, 7 };
            IList<int> d = new List<int> { 4, 1, 8, 3 };
            IList<IList<int>> testData = new List<IList<int>>() { a, b, c, d };
            var result = MinimumTotal(testData);
            result.ShouldBe(11);
            a = new List<int> { -1 };
            b = new List<int> { 2, 3 };
            c = new List<int> { 1, -1, -3 };
            testData = new List<IList<int>> { a, b, c };
            result = MinimumTotal(testData);
            result.ShouldBe(-1);
        }
        private int MinimumTotal(IList<IList<int>> triangle)
        {
            if (triangle.Count == 1)
                return triangle[0][0];
            int[] path = new int[triangle[triangle.Count - 1].Count];
            int i = 1;
            int index = 0;
            path[0] = triangle[0][0];
            while (i < triangle.Count)
            {
                while (index < triangle[i].Count)
                {
                    if (index == 0)
                    {
                        path[index] = triangle[i][index] + triangle[i - 1][index];
                    }
                    else if (index == triangle[i].Count - 1)
                    {
                        path[index] = triangle[i][index] + triangle[i - 1][index - 1];
                    }
                    else
                    {
                        path[index] = triangle[i][index] + Math.Min(triangle[i - 1][index], triangle[i - 1][index - 1]);
                    }
                    triangle[i][index] = path[index];
                    index++;
                }
                index = 0;
                i++;
            }
            Array.Sort(path);
            return path[0];
        }

        [Fact]
        public void L0121_MaxProfit()
        {

        }
        private int MaxProfit(int[] prices)
        {
            return 0;
        }
        [Fact]
        public void L0129_SumNumbers()
        {
            var root = TreeNode.CreateTree(new object[] { 4, 9, 0, 5, 1 });
            var result = SumNumbers(root);
            result.ShouldBe(1026);
            root = TreeNode.CreateTree(new object[] { 1, 2, 3 });
            result = SumNumbers(root);
            result.ShouldBe(25);
        }
        private int SumNumbers(TreeNode root)
        {
            IList<Stack<int>> collection = new List<Stack<int>>();
            IList<int> path = new List<int>();
            InnerSumNumbers(root, path, collection);
            int res = 0;
            int mul = 1;
            foreach (var tmp in collection)
            {
                while (tmp.Count > 0)
                {
                    res += tmp.Pop() * mul;
                    mul = mul * 10;
                }
                mul = 1;
            }
            return res;
        }
        private void InnerSumNumbers(TreeNode root, IList<int> path, IList<Stack<int>> results)
        {
            if (root == null)
                return;
            path.Add(root.val);
            if (root.left == null && root.right == null)
            {
                var result = new Stack<int>(path);
                results.Add(result);
                path.RemoveAt(path.Count - 1);
                return;
            }
            InnerSumNumbers(root.left, path, results);
            InnerSumNumbers(root.right, path, results);
            path.RemoveAt(path.Count - 1);
        }
    }
}
