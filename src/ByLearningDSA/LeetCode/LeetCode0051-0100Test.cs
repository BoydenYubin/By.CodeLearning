using ByLearningDSA.LeetCode.UtilClass;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ByLearningDSA.LeetCode
{
    public class LeetCode0051_0100Test
    {
        /// <summary>
        /// https://leetcode-cn.com/problems/maximum-subarray/
        /// 给定一个整数数组 nums ，找到一个具有最大和的连续子数组（子数组最少包含一个元素），返回其最大和。
        /// </summary>
        [Fact]
        public void L0053_MaxSubArray()
        {
            var nums = new int[] { -2, 1, -3, 4, -1, 2, 1, -5, 4 };
            var result = MaxSubArray(nums);
            result.ShouldBe(6);
        }
        private int MaxSubArray(int[] nums)
        {
            var len = nums.Length;
            if (len == 0)
            {
                return 0;
            }
            int result = nums[0];
            int sum = nums[0];
            for (int i = 1; i < len; i++)
            {
                if (sum < 0)
                {
                    sum = nums[i];
                }
                else
                {
                    sum += nums[i];
                }
                result = Math.Max(result, sum);
            }
            return result;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/unique-paths/
        /// 一个机器人位于一个 m x n 网格的左上角 （起始点在下图中标记为 “Start” ）。
        /// 机器人每次只能向下或者向右移动一步。机器人试图达到网格的右下角（在下图中标记为 “Finish” ）。
        /// 问总共有多少条不同的路径？
        /// </summary>
        [Fact]
        public void L0062_UniquePathsTest()
        {
            var res = UniquePaths(3, 2);
            res.ShouldBe(3);
            var res1 = UniquePaths(3, 3);
            res1.ShouldBe(6);
            var res2 = UniquePaths(3, 7);
            res2.ShouldBe(28);
        }
        private int UniquePaths(int m, int n)
        {
            //创建二维数组记录每个格可抵达的路径和
            int[,] pos = new int[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    pos[i, j] = 1;
                }
            }
            //每个格子只能由该格子上方或者格子左方达到
            //因此该格子的路径总数等于该格子可抵达路径之和
            for (int i = 1; i < m; i++)
            {
                for (int j = 1; j < n; j++)
                {
                    pos[i, j] = pos[i - 1, j] + pos[i, j - 1];
                }
            }
            return pos[m - 1, n - 1];
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/unique-paths-ii/
        /// 现在考虑网格中有障碍物。那么从左上角到右下角将会有多少条不同的路径？
        /// </summary>
        [Fact]
        public void L0063_UniquePathsWithObstacles()
        {
            var res = UniquePathsWithObstacles(new int[3, 3] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } });
            res.ShouldBe(2);
            res = UniquePathsWithObstacles(new int[3, 2] { { 0, 0 }, { 1, 1 }, { 0, 0 } });
            res.ShouldBe(0);
        }
        private int UniquePathsWithObstacles(int[,] obstacleGrid)
        {
            if (obstacleGrid.Length == 0 || obstacleGrid[0, 0] == 1)
                return 0;
            int m = obstacleGrid.GetLength(0);
            int n = obstacleGrid.GetLength(1);
            //与1异或表示该格子有障碍的情况下路径和为0
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    obstacleGrid[i, j] = obstacleGrid[i, j] ^ 1;
                }
            }
            //遇到该路障时则跳过计算
            //相比没路障的情况下多了这个运算
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (obstacleGrid[i, j] == 0)
                    {
                        continue;
                    }
                    else
                    {
                        if (i - 1 >= 0 && j - 1 >= 0)
                        {
                            obstacleGrid[i, j] = obstacleGrid[i - 1, j] + obstacleGrid[i, j - 1];
                        }
                        else
                        {
                            if (i - 1 < 0 && j - 1 < 0)
                                continue;
                            if (i - 1 < 0)
                            {
                                obstacleGrid[i, j] = obstacleGrid[i, j - 1];
                            }
                            if (j - 1 < 0)
                            {
                                obstacleGrid[i, j] = obstacleGrid[i - 1, j];
                            }
                        }
                    }
                }
            }
            return obstacleGrid[m - 1, n - 1];
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/minimum-path-sum/
        /// </summary>
        [Fact]
        public void L0064_MinPathSum()
        {
            var res = MinPathSum(new int[3, 3] { { 1, 3, 1 }, { 1, 5, 1 }, { 4, 2, 1 } });
            res.ShouldBe(7);
            var res1 = MinPathSum(new int[2, 3] { { 1, 2, 3 }, { 4, 5, 6 } });
            res1.ShouldBe(12);
            var res2 = MinPathSum(new int[][] { new int[] { 1, 3, 1 }, new int[] { 1, 5, 1 }, new int[] { 4, 2, 1 } });
            res2.ShouldBe(7);
        }
        private int MinPathSum(int[,] grid)
        {
            int m = grid.GetLength(0);
            int n = grid.GetLength(1);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    else if (i == 0 && j != 0)
                    {
                        grid[i, j] = grid[i, j] + grid[i, j - 1];
                    }
                    else if (j == 0 && i != 0)
                    {
                        grid[i, j] = grid[i, j] + grid[i - 1, j];
                    }
                    else
                    {
                        grid[i, j] = grid[i, j] + Math.Min(grid[i - 1, j], grid[i, j - 1]);
                    }
                }
            }
            return grid[m - 1, n - 1];
        }
        private int MinPathSum(int[][] grid)
        {
            int m = grid.Length;
            int n = grid[0].Length;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    else if (i == 0 && j != 0)
                    {
                        grid[i][j] = grid[i][j] + grid[i][j - 1];
                    }
                    else if (j == 0 && i != 0)
                    {
                        grid[i][j] = grid[i][j] + grid[i - 1][j];
                    }
                    else
                    {
                        grid[i][j] = grid[i][j] + Math.Min(grid[i - 1][j], grid[i][j - 1]);
                    }
                }
            }
            return grid[m - 1][n - 1];
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/plus-one/
        /// </summary>
        [Fact]
        public void L0066_PlusOneTest()
        {
            var res = PlusOne(new int[] { 1, 2, 3 });
            res.ShouldBe(new int[] { 1, 2, 4 });
            var res1 = PlusOne(new int[] { 1, 2, 9, 9 });
            res1.ShouldBe(new int[] { 1, 3, 0, 0 });
            var res2 = PlusOne(new int[] { 9, 9, 9 });
            res2.ShouldBe(new int[] { 1, 0, 0, 0 });
            var res3 = PlusOne(new int[] { 0 });
            res3.ShouldBe(new int[] { 1 });
        }
        private int[] PlusOne(int[] digits)
        {
            int i = digits.Length - 1;
            int tmp = digits[i] + 1;
            int head = tmp > 9 ? 1 : 0;
            digits[i] = tmp % 10;
            if (head == 0)
                return digits;
            while (i - 1 >= 0)
            {
                tmp = digits[i - 1] + head;
                digits[i - 1] = tmp % 10;
                head = tmp > 9 ? 1 : 0;
                i--;
            }
            if (head == 1)
            {
                int[] res = new int[digits.Length + 1];
                digits.CopyTo(res, 1);
                res[0] = 1;
                return res;
            }
            return digits;
        }
        [Fact]
        public void L0068_FullJustifyTest()
        {

        }
        private IList<string> FullJustify(string[] words, int maxWidth)
        {
            var res = new List<string>();
            int i = 0;
            int lineSum = 0;
            int wordsSum = 0;
            int pos = 0;
            while (i < words.Length)
            {
                lineSum = words[i].Length + 1 + lineSum;
                wordsSum = wordsSum + words[i].Length;
                if (lineSum > maxWidth)
                {
                    int gapNum = maxWidth / (i - pos - 1);
                    int gapAdd = maxWidth % (i - pos - 1);
                    StringBuilder s = new StringBuilder();
                    for (int j = pos; j < i; j++)
                    {
                        s.Append(words[j]);

                    }
                }
            }
            return res;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/simplify-path/
        /// </summary>
        [Fact]
        public void L0071_SimplifyPath()
        {
            var res = SimplifyPath("/a/./b/../../c/");
            res.ShouldBe("/c");
            var res1 = SimplifyPath("/a//b////c/d//././/..");
            res1.ShouldBe("/a/b/c");
            var res2 = SimplifyPath("/a/../../b/../c//.//");
            res2.ShouldBe("/c");
            var res3 = SimplifyPath("/home//foo/");
            res3.ShouldBe("/home/foo");
        }
        private string SimplifyPath(string path)
        {
            Stack<string> res = new Stack<string>();
            int slow = 0, fast;
            while (slow < path.Length)
            {
                while (slow < path.Length && path[slow] == '/')
                {
                    slow++;
                }
                fast = slow;
                string tmp = "";
                while (fast < path.Length && path[fast] != '/')
                {
                    tmp += path[fast];
                    fast++;
                }
                switch (tmp)
                {
                    case "..":
                        if (tmp == ".." && res.Count > 0)
                        {
                            res.Pop();
                        }
                        break;
                    case ".":
                        break;
                    case "":
                        break;
                    default:
                        res.Push(tmp);
                        break;
                }
                slow = fast;
            }
            StringBuilder result = new StringBuilder();
            var popNum = res.Count;
            if (popNum == 0)
            {
                return "/";
            }
            for (int i = 0; i < popNum; i++)
            {
                var tmp = res.Pop();
                result.Insert(0, tmp);
                result.Insert(0, '/');
            }
            return result.ToString();
        }
        [Fact]
        public void L0073_SetZeroes()
        {
            var res = SetZeroes(new int[][] { new int[] { 1, 1, 1 }, new int[] { 1, 0, 1 }, new int[] { 1, 1, 1 } });
            res.ShouldBe(new int[][] { new int[] { 1, 0, 1 }, new int[] { 0, 0, 0 }, new int[] { 1, 0, 1 } });
            var res1 = SetZeroes(new int[][] { new int[] { 0, 1, 2, 0 }, new int[] { 3, 4, 5, 2 }, new int[] { 1, 3, 1, 5 } });
            res1.ShouldBe(new int[][] { new int[] { 0, 0, 0, 0 }, new int[] { 0, 4, 5, 0 }, new int[] { 0, 3, 1, 0 } });
        }
        private int[][] SetZeroes(int[][] matrix)
        {
            Dictionary<int, bool> isJump = new Dictionary<int, bool>();
            int m = matrix.Length;
            int n = matrix[0].Length;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i][j] != 0)
                    {
                        continue;
                    }
                    else
                    {
                        if (isJump.ContainsKey(j))
                            continue;
                        isJump.Add(j, true);
                        int v = i - 1;
                        int h = j - 1;
                        while (v >= 0)
                        {
                            matrix[v][j] = 0;
                            v--;
                        }
                        v = i + 1;
                        while (v < m)
                        {
                            matrix[v][j] = 0;
                            v++;
                        }
                        while (h >= 0)
                        {
                            matrix[i][h] = 0;
                            h--;
                        }
                        j++;
                        while (j < n && matrix[i][j] != 0)
                        {
                            matrix[i][j] = 0;
                            j++;
                        }
                        j--;
                    }
                }
            }
            return matrix;
        }

        [Fact]
        public void L0074_SearchMatrix()
        {
            var res = SearchMatrix(new int[][] { new int[] { 1, 3, 5, 7 }, new int[] { 10, 11, 16, 20 }, new int[] { 23, 30, 34, 50 } }, 3);
            res.ShouldBe(true);
            var res1 = SearchMatrix(new int[][] { new int[] { 1, 3, 5, 7 }, new int[] { 10, 11, 16, 20 }, new int[] { 23, 30, 34, 50 } }, 13);
            res1.ShouldBe(false);
            var res2 = SearchMatrix(new int[][] { new int[] { 1, 3, 5, 7 }, new int[] { 10, 11, 16, 20 }, new int[] { 23, 30, 34, 50 } }, 34);
            res2.ShouldBe(true);
            var res3 = SearchMatrix(new int[][] { }, 0);
            res3.ShouldBe(false);
            var res4 = SearchMatrix(new int[][] { new int[] { 1 }, new int[] { 5 }, new int[] { 8 } }, 7);
            res4.ShouldBe(false);
            var res5 = SearchMatrix(new int[][] { new int[] { 1 }, new int[] { 3 } }, 3);
            res5.ShouldBe(true);
        }
        private bool SearchMatrix(int[][] matrix, int target)
        {
            int m = matrix.Length;
            if (m == 0)
                return false;
            int n = matrix[0].Length;
            int i = 0;
            while (i < m)
            {
                if (matrix[i][0] == target)
                    return true;
                if (matrix[i][0] > target)
                    break;
                if (matrix[i][0] < target)
                    i++;
            }
            if (i != 0)
            {
                i--;
            }
            int lo = 0, hi = n - 1;
            while (lo <= hi)
            {
                int mid = (lo + hi) / 2;
                if (matrix[i][mid] < target)
                {
                    lo = mid + 1;
                    if (lo <= n - 1 && matrix[i][lo] > target)
                        return false;
                }
                else if (matrix[i][mid] > target)
                {
                    hi = mid - 1;
                    if (hi >= 0 && matrix[i][hi] < target)
                        return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        [Fact]
        public void L0074_SortColors()
        {
            var res = SortColors(new int[] { 2, 0, 2, 1, 1, 0 });
            res.ShouldBe(new int[] { 0, 0, 1, 1, 2, 2 });
            var res1 = SortColors(new int[] { 2, 1, 0, 0, 1, 2 });
            res1.ShouldBe(new int[] { 0, 0, 1, 1, 2, 2 });
            var res2 = SortColors(new int[] { 2, 0, 1 });
            res2.ShouldBe(new int[] { 0, 1, 2 });
            var res3 = SortColors(new int[] { 2 });
            res3.ShouldBe(new int[] { 2 });
        }
        private int[] SortColors(int[] colors)
        {
            int lo = 0, hi = colors.Length - 1;
            int i = 0;
            while (lo < hi && i <= hi)
            {
                switch (colors[i])
                {
                    case 2:
                        colors[i] = colors[hi];
                        colors[hi--] = 2;
                        break;
                    case 1:
                        i++;
                        break;
                    case 0:
                        if (i == lo)
                        {
                            i++;
                            lo++;
                            break;
                        }
                        colors[i] = colors[lo];
                        colors[lo++] = 0;
                        break;
                }
            }
            return colors;
        }
        [Fact]
        public void L0077_Combine()
        {
            var res = Combine(4, 2);
            res.Count.ShouldBe(6);
            var res1 = Combine(4, 3);
            res1.Count.ShouldBe(3);
            var res2 = Combine(5, 3);
            res2.Count.ShouldBe(10);
        }
        private IList<IList<int>> Combine(int n, int k)
        {
            var res = new List<IList<int>>();
            if (k > n)
                return res;
            while (n - k >= 0)
            {
                int curNum = n - k + 1;
                while (curNum >= 1)
                {
                    List<int> tmp = new List<int>();
                    tmp.Add(curNum);
                    for (int i = curNum + 1; i <= n; i++)
                    {
                        tmp.Add(i);
                    }
                    curNum--;
                    res.Add(tmp);
                }
                n--;
            }
            return res;
        }
        [Fact]
        public void L0078_Subsets()
        {

        }
        private IList<IList<int>> Subsets(int[] nums)
        {
            List<IList<int>> result = new List<IList<int>>();
            //SubsetsBFS(nums, 0, 1, result);
            return result;
        }
        private void SubsetsBFS(int[] nums, int i, IList<IList<int>> result)
        {
            if (i > 3)
            {
                return;
            }
            else
            {
                while (i < 3)
                {

                    SubsetsBFS(nums, i++, result);
                }
            }
        }
        [Fact]
        public void L0080_RemoveDuplicates()
        {
            RemoveDuplicates(new int[] { 1, 1, 1, 2, 2, 3 });
        }
        private int RemoveDuplicates(int[] nums)
        {
            int j = 1, count = 1;
            for (int i = 1; i < nums.Length; i++)
            {

                if (nums[i] == nums[i - 1])
                {
                    count++;
                }
                else
                {
                    count = 1;
                }
                if (count <= 2)
                {
                    nums[j++] = nums[i];
                }
            }
            return j;
        }
        //for 82 && 83 && 86
        internal class ListNode
        {
            public int val;
            public ListNode next;
            public ListNode(int x)
            {
                val = x;
            }
        }
        [Fact]
        public void L0082_DeleteDuplicates()
        {
            var listnode1 = new ListNode(1);
            var listnode2 = new ListNode(1);
            var listnode3 = new ListNode(2);
            var listnode4 = new ListNode(2);
            var listnode5 = new ListNode(3);
            var listnode6 = new ListNode(4);
            listnode1.next = listnode2;
            listnode2.next = listnode3;
            listnode3.next = listnode4;
            listnode4.next = listnode5;
            listnode5.next = listnode6;
            DeleteDuplicates_0082(listnode1);
        }
        private ListNode DeleteDuplicates_0082(ListNode head)
        {
            int count = 1;
            ListNode cur = head;
            while (head.next != null)
            {
                if (head.next.val == head.val)
                {
                    count++;
                    head = head.next;
                }
                else
                {
                    if (count > 1)
                    {
                        cur = head.next;
                    }
                    else
                    {
                        cur.next = head.next;
                    }
                    cur = cur.next;
                    count = 1;
                    head = head.next;
                }
            }
            return cur;
        }
        [Fact]
        public void L0083_DeleteDuplicates()
        {
            var listnode1 = new ListNode(1);
            var listnode2 = new ListNode(1);
            var listnode3 = new ListNode(2);
            var listnode4 = new ListNode(3);
            var listnode5 = new ListNode(3);
            listnode1.next = listnode2;
            listnode2.next = listnode3;
            listnode3.next = listnode4;
            listnode4.next = listnode5;
            var result = DeleteDuplicates_0083(listnode1);
        }
        private ListNode DeleteDuplicates_0083(ListNode head)
        {
            ListNode result = new ListNode(head.val);
            ListNode tmp = result;
            while (head.next != null)
            {
                if (head.next.val != head.val)
                {
                    tmp.next = new ListNode(head.next.val);
                    tmp = tmp.next;
                }
                head = head.next;
            }
            return result;
        }
        [Fact]
        public void L0086_Partition()
        {
            var listnode1 = new ListNode(1);
            var listnode2 = new ListNode(4);
            var listnode3 = new ListNode(3);
            var listnode4 = new ListNode(2);
            var listnode5 = new ListNode(5);
            var listnode6 = new ListNode(2);
            listnode1.next = listnode2;
            listnode2.next = listnode3;
            listnode3.next = listnode4;
            listnode4.next = listnode5;
            listnode5.next = listnode6;
            Partition(listnode1, 3);
        }
        private ListNode Partition(ListNode head, int x)
        {
            return null;
        }
        [Fact]
        public void L0087_Merge()
        {
            var res = Merge(new int[] { 1, 2, 3, 0, 0, 0 }, 3, new int[] { 2, 5, 6 }, 3);
            res = Merge(new int[] { 0 }, 0, new int[] { 1 }, 1);
            res = Merge(new int[] { 2, 0 }, 1, new int[] { 1 }, 1);
        }
        private int[] Merge(int[] nums1, int m, int[] nums2, int n)
        {
            int p1 = m - 1;
            int p2 = n - 1;
            for (int i = m + n - 1; i >= 0; i--)
            {
                if (p2 >= 0 && p1 >= 0)
                {
                    if (nums2[p2] > nums1[p1])
                    {
                        nums1[i] = nums2[p2--];
                    }
                    else
                    {
                        nums1[i] = nums1[p1--];
                    }
                }
                else
                {
                    if (p1 < 0)
                    {
                        nums1[i] = nums2[p2--];
                    }
                    else
                    {
                        nums1[i] = nums1[p1--];
                    }
                }
            }
            return nums1;
        }

        [Fact]
        public void L0089_GrayCode()
        {
        }
        [Fact]
        public void L0091_NumDecodings()
        {
            var res = NumDecodings("226");
            res.ShouldBe(3);
            var res1 = NumDecodings("22613");
            res1.ShouldBe(6);
        }
        private int NumDecodings(string s)
        {
            if (s.StartsWith('0'))
                return 0;
            int[] dp = new int[s.Length + 1];
            dp[0] = 1;
            dp[1] = s[0] == '0' ? 0 : 1;
            for (int i = 1; i < s.Length; i++)
            {
                if (s[i - 1] == '1' || s[i - 1] == '2' && s[i] < '7')
                {
                    if (s[i] == '0')
                        dp[i + 1] = dp[i - 1];
                    else
                        dp[i + 1] = dp[i] + dp[i - 1];
                }
                else if (s[i] == '0')
                {
                    return 0;
                }
                else
                {
                    dp[i + 1] = dp[i];
                }
            }
            return dp[s.Length];
        }
        [Fact]
        public void L0092_ReverseBetween()
        {
            var listnode1 = new ListNode(1);
            var listnode2 = new ListNode(2);
            var listnode3 = new ListNode(3);
            var listnode4 = new ListNode(4);
            var listnode5 = new ListNode(5);
            var listnode6 = new ListNode(6);
            listnode1.next = listnode2;
            listnode2.next = listnode3;
            listnode3.next = listnode4;
            listnode4.next = listnode5;
            listnode5.next = listnode6;
            ReverseBetween(listnode1, 2, 5);
        }
        private void ReverseBetween(ListNode head, int m, int n)
        {
            int i = 1;
            ListNode p = head;
            Stack<ListNode> nodes = new Stack<ListNode>();
            while (p.next != null)
            {
                i++;
                if (i == n)
                {
                    ListNode tmp = p.next.next;
                    while (nodes.Count > 0)
                    {

                        p.next.next = nodes.Pop();
                        p = p.next;
                    }
                    p.next = tmp;
                    break;
                }
                if (i >= m)
                {
                    nodes.Push(p.next);
                }
                p = p.next;
            }
        }

        [Fact]
        public void L0094_InorderTraversal()
        {
            ///<seealso cref="BinaryTreeTraversalTest"/>
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/validate-binary-search-tree/
        /// 给定一个二叉树，判断其是否是一个有效的二叉搜索树。
        ///   节点的左子树只包含小于当前节点的数。 
        ///   节点的右子树只包含大于当前节点的数。
        ///   所有左子树和右子树自身必须也是二叉搜索树。
        /// </summary>
        [Fact]
        public void L0098_IsValidBST()
        {
            var root = TreeNode.CreateTree(new object[] { 5, 1, 4, null, null, 3, 6 });
            var result = IsValidBST(root);
            result.ShouldBe(false);
            root = TreeNode.CreateTree(new object[] { int.MinValue, null, int.MaxValue });
            result = IsValidBST(root);
            result.ShouldBe(true);
        }
        private bool IsValidBST(TreeNode root)
        {
            if (root == null)
                return true;
            return IsValid(root, long.MinValue, long.MaxValue);
        }
        //这里参数用long是因为树的值已经是int了，要选择比int值范围广的类型
        private bool IsValid(TreeNode root, long min, long max)
        {
            if (root == null)
                return true;
            if (root.val <= min || root.val >= max)
                return false;
            return IsValid(root.left, min, root.val) && IsValid(root.right, root.val, max);
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/recover-binary-search-tree/
        /// 给你二叉搜索树的根节点 root ，该树中的两个节点被错误地交换。请在不改变其结构的情况下，恢复这棵树。
        /// </summary>
        [Fact]
        public void L0099_RecoverTree()
        {
            TreeNode root = TreeNode.CreateTree(new object[] { 3, 1, 4, null, null, 2 });
            RecoverTree(root);
            root.val.ShouldBe(2);
        }
        private void RecoverTree(TreeNode root)
        {
            //二叉搜索树，中序遍历即可从小到大排列结果
            List<TreeNode> datas = new List<TreeNode>();
            InOrderTraersal(root, datas);
            TreeNode pre = null, post = null;
            for (int i = 0; i < datas.Count - 1; i++)
            {
                if (datas[i].val > datas[i + 1].val)
                {
                    //这里的小技巧是先将pre和post都赋值，
                    //再次遇到仅改变post的值即可
                    post = datas[i + 1];
                    if (pre == null)
                    {
                        pre = datas[i];
                    }
                }
            }
            int tmp = pre.val;
            pre.val = post.val;
            post.val = tmp;
        }
        private void InOrderTraersal(TreeNode root, List<TreeNode> datas)
        {
            if (root == null)
                return;
            InOrderTraersal(root.left, datas);
            datas.Add(root);
            InOrderTraersal(root.right, datas);
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/same-tree/
        /// 给你两棵二叉树的根节点 p 和 q ，编写一个函数来检验这两棵树是否相同。
        ///如果两个树在结构上相同，并且节点具有相同的值，则认为它们是相同的。
        /// </summary>
        [Fact]
        public void L0100_IsSameTree()
        {
            var tree1 = TreeNode.CreateTree(new object[] { 1, 2, 1 });
            var tree2 = TreeNode.CreateTree(new object[] { 1, 1, 2 });
            var result = IsSameTree(tree1, tree2);
            result.ShouldBe(false);
        }
        private bool IsSameTree(TreeNode p, TreeNode q)
        {
            if (p == null && q == null)
                return true;
            if (p == null || q == null)
                return false;
            if (p.val != q.val)
                return false;
            return IsSameTree(p.left, q.left) && IsSameTree(p.right, q.right);
        }
    }
}
