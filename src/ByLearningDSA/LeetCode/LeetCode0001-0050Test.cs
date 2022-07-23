using Shouldly;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using static ByLearningDSA.LeetCode.LeetCode0051_0100Test;

namespace ByLearningDSA.LeetCode
{
    public class LeetCode0001_0050Test
    {
        /// <summary>
        /// https://leetcode-cn.com/problems/longest-substring-without-repeating-characters/
        /// 给定一个字符串，请你找出其中不含有重复字符的 最长子串 的长度。
        /// 输入: s = "abcabcbb"
        /// 输出: 3 
        ///解释: 因为无重复字符的最长子串是 "abc"，所以其长度为 3。
        /// </summary>
        [Fact]
        public void L0003_LengthOfLongestSubstring()
        {
            var result = LengthOfLongestSubstring("pwwkew");
            result.ShouldBe(3);
            result = LengthOfLongestSubstring("pwakwcab");
            result.ShouldBe(5);
            result = LengthOfLongestSubstring("tmmzuxt");
            result.ShouldBe(5);
        }
        private int LengthOfLongestSubstring(string s)
        {
            Dictionary<char, int> maps = new Dictionary<char, int>();
            int result = 0;
            for (int start = 0, end = 0; end < s.Length; end++)
            {
                if (maps.ContainsKey(s[end]))
                {
                    start = Math.Max(start, maps[s[end]]);
                }
                maps[s[end]] = end + 1;
                result = Math.Max(end - start + 1, result);
            }
            return result;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/zigzag-conversion/
        /// 将一个给定字符串 s 根据给定的行数 numRows ，以从上往下、从左到右进行 Z 字形排列。
        ///比如输入字符串为 "PAYPALISHIRING" 行数为 3 时，排列如下：
        ///P   A   H   N
        ///A P L S I I G
        ///Y   I   R
        /// </summary>
        [Fact]
        public void L0006_Convert()
        {
            var result = Convert("PAYPALISHIRING", 3);
            result.ShouldBe("PAHNAPLSIIGYIR");
            result = Convert("AB", 1);
            result.ShouldBe("AB");
        }
        private string Convert(string s, int numRows)
        {
            if (numRows == 1)
            {
                return s;
            }
            string[] tmp = new string[numRows];
            Array.Fill(tmp, "");
            bool isReverse = false;
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (!isReverse)
                {
                    tmp[count] += s[i];
                    if (count == numRows - 1)
                    {
                        isReverse = true;
                        count--;
                        continue;
                    }
                    count++;
                }
                else
                {
                    tmp[count] += s[i];
                    if (count == 0)
                    {
                        isReverse = false;
                        count++;
                        continue;
                    }
                    count--;
                }
            }
            string result = "";
            foreach (var t in tmp)
            {
                result += t;
            }
            return result;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/container-with-most-water/
        /// 给你 n 个非负整数 a1，a2，...，an，每个数代表坐标中的一个点 (i, ai) 。
        /// 在坐标内画 n 条垂直线，垂直线 i 的两个端点分别为 (i, ai) 和 (i, 0) 。
        /// 找出其中的两条线，使得它们与 x 轴共同构成的容器可以容纳最多的水。
        /// 说明：你不能倾斜容器。
        /// 双指针法
        /// </summary>
        [Fact]
        public void L0011_MaxArea()
        {
            var res1 = MaxArea(new int[] { 1, 8, 6, 2, 5, 4, 8, 3, 7 });
            res1.ShouldBe(49);
        }
        private int MaxArea(int[] height)
        {
            int i = 0, j = height.Length - 1, res = 0;
            while (i < j)
            {
                res = height[i] < height[j] ?
                    Math.Max(res, (j - i) * height[i++]) :
                    Math.Max(res, (j - i) * height[j--]);
            }
            return res;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/3sum/
        /// 给你一个包含 n 个整数的数组 nums，判断 nums 中是否存在三个元素 a，b，c ，
        /// 使得 a + b + c = 0 ？请你找出所有和为 0 且不重复的三元组。
        /// 注意：答案中不可以包含重复的三元组。
        /// </summary>
        [Fact]
        public void L0015_ThreeSum()
        {
            var result = ThreeSum(new int[] { -7, -3, -2, -1, 0, 1, 5, 4, 6, 7 });
            result.Count.ShouldBe(5);
        }
        private IList<IList<int>> ThreeSum(int[] nums)
        {
            IList<IList<int>> res = new List<IList<int>>();
            if (nums == null || nums.Length < 3)
                return res;
            Array.Sort(nums);
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] > 0) break;
                if (i > 0 && nums[i] == nums[i - 1]) continue;
                int left = i + 1;
                int right = nums.Length - 1;
                while (left < right)
                {
                    int sum = nums[i] + nums[left] + nums[right];
                    if (sum == 0)
                    {
                        res.Add(new List<int> { nums[i], nums[left], nums[right] });
                        while (left < right && nums[left] == nums[left + 1]) left++; // 去重
                        while (left < right && nums[left] == nums[right - 1]) right--; // 去重
                        left++;
                        right--;
                    }
                    else if (nums[i] + nums[left] + nums[right] < 0)
                    {
                        left++;
                    }
                    else
                    {
                        right--;
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/merge-two-sorted-lists/submissions/
        /// 将两个升序链表合并为一个新的 升序 链表并返回。新链表是通过拼接给定的两个链表的所有节点组成的
        /// </summary>
        [Fact]
        public void L0021_MergeTwoLists()
        {
            ListNode l1 = new ListNode(2)
            {
                next = new ListNode(4)
                {
                    next = new ListNode(6)
                }
            };
            ListNode l2 = new ListNode(1)
            {
                next = new ListNode(3)
                {
                    next = new ListNode(5)
                }
            };
            var result = MergeTwoLists(l1, l2);
            result.next.next.next.val.ShouldBe(4);
        }
        private ListNode MergeTwoLists(ListNode l1, ListNode l2)
        {
            if (l1 == null)
            {
                return l2;
            }
            if (l2 == null)
            {
                return l1;
            }
            if (l1.val > l2.val)
            {
                l2.next = MergeTwoLists(l1, l2.next);
                return l2;
            }
            else
            {
                l1.next = MergeTwoLists(l1.next, l2);
                return l1;
            }
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/swap-nodes-in-pairs/
        /// 给定一个链表，两两交换其中相邻的节点，并返回交换后的链表。
        /// 你不能只是单纯的改变节点内部的值，而是需要实际的进行节点交换。
        /// </summary>
        [Fact]
        public void L0024_SwapPairs()
        {
            ListNode head = new ListNode(1)
            {
                next = new ListNode(2)
                {
                    next = new ListNode(3)
                    {
                        next = new ListNode(4)
                        {
                            next = new ListNode(5)
                        }
                    }
                }
            };
            var result = SwapPairs(head);
        }
        private ListNode SwapPairs(ListNode head)
        {
            if (head == null)
            {
                return null;
            }
            if (head.next == null)
            {
                return head;
            }
            ListNode result = head.next;
            ListNode p = head;
            ListNode tmp = p.next;
            p.next = SwapPairs(p.next.next);
            tmp.next = p;
            return result;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/remove-element/
        /// </summary>
        [Fact]
        public void L0027_RemoveElementTest()
        {
            RemoveElement(new int[] { 3, 2, 2, 3 }, 3);
        }
        private int RemoveElement(int[] nums, int val)
        {
            if (nums.Length == 0)
                return 0;
            int slow = 0;
            while (slow < nums.Length && nums[slow] != val)
            {
                slow++;
            }
            for (int fast = slow + 1; fast < nums.Length; fast++)
            {
                if (nums[fast] != val)
                {
                    nums[slow++] = nums[fast];
                }
            }
            return slow + 1;
        }
        [Fact]
        public void L0031_NextPermutation()
        {
            var res = NextPermutation(new int[] { 3, 2, 1 });
            res.ShouldBe(new int[] { 1, 2, 3 });
            var res1 = NextPermutation(new int[] { 1, 2, 3 });
            res1.ShouldBe(new int[] { 1, 3, 2 });
            var res2 = NextPermutation(new int[] { 1 });
            res2.ShouldBe(new int[] { 1 });
            var res3 = NextPermutation(new int[] { 1, 1, 5 });
            res3.ShouldBe(new int[] { 1, 5, 1 });
            var res4 = NextPermutation(new int[] { 1, 5, 2, 5, 6, 7, 0 });
            res4.ShouldBe(new int[] { 1, 5, 2, 5, 7, 0, 6 });
            var res5 = NextPermutation(new int[] { 1, 2, 3, 8, 5, 7, 6, 4 });
            res5.ShouldBe(new int[] { 1, 2, 3, 8, 6, 4, 5, 7 });
            var res6 = NextPermutation(new int[] { 5, 4, 3, 2, 1 });
            res6.ShouldBe(new int[] { 1, 2, 3, 4, 5 });
            var res7 = NextPermutation(new int[] { 1, 3, 2 });
            res7.ShouldBe(new int[] { 2, 1, 3 });
            var res8 = NextPermutation(new int[] { 1, 5, 1 });
            res8.ShouldBe(new int[] { 5, 1, 1 });
        }
        private int[] NextPermutation([NotNull] int[] nums)
        {
            if (nums.Length <= 1)
                return nums;
            int i = nums.Length - 2;
            while (i >= 0 && nums[i] >= nums[i + 1])
            {
                i--;
            }
            if (i >= 0)
            {
                int k = nums.Length - 1;
                while (k >= 0 && nums[i] >= nums[k])
                {
                    k--;
                }
                NextPermutationSwap(nums, i, k);
                NextPermutationReverse(nums, i + 1);
            }
            else
            {
                NextPermutationReverse(nums, 0);
            }
            return nums;
        }
        private void NextPermutationReverse(int[] nums, int start)
        {
            int left = start, right = nums.Length - 1;
            while (left < right)
            {
                NextPermutationSwap(nums, left, right);
                left++;
                right--;
            }
        }
        private void NextPermutationSwap(int[] nums, int i, int j)
        {
            int temp = nums[i];
            nums[i] = nums[j];
            nums[j] = temp;
        }
        /// <summary>
        /// https://leetcode-cn.com/problems/search-in-rotated-sorted-array/
        /// </summary>
        [Fact]
        public void L0033_Search()
        {

        }
        private int Search(int[] nums, int target)
        {
            int lo = 0;
            int hi = nums.Length - 1;
            int mid = (hi + lo) / 2;
            return 0;
        }
        [Fact]
        public void L0034_SearchRange()
        {
            var res = SearchRange(new int[] { 5, 7, 7, 8, 8, 10 }, 8);
            res.ShouldBe(new int[] { 3, 4 });
            var res1 = SearchRange(new int[] { 1 }, 1);
            res1.ShouldBe(new int[] { 0, 0 });
        }
        private int[] SearchRange(int[] nums, int target)
        {
            if (nums == null || nums.Length == 0)
                return new int[] { -1, -1 };
            int leftIdx = BinarySearch(nums, target, true);
            int rightIdx = BinarySearch(nums, target, false) - 1;
            if (leftIdx <= rightIdx && rightIdx < nums.Length && nums[leftIdx] == target && nums[rightIdx] == target)
            {
                return new int[] { leftIdx, rightIdx };
            }
            return new int[] { -1, -1 };
        }
        private int BinarySearch(int[] nums, int target, bool lower)
        {
            int lo = 0, hi = nums.Length - 1, ans = nums.Length;
            while (lo <= hi)
            {
                int mid = (lo + hi) / 2;
                if (nums[mid] > target || (lower && nums[mid] >= target))
                {
                    hi = mid - 1;
                    ans = mid;
                }
                else
                {
                    lo = mid + 1;
                }
            }
            return ans;
        }
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void L0035_SearchInsert()
        {
            var res = SearchInsert(new int[] { 1, 3, 5, 6 }, 5);
            res.ShouldBe(2);
            var res1 = SearchInsert(new int[] { 1, 3, 5, 6 }, 2);
            res1.ShouldBe(1);
            var res2 = SearchInsert(new int[] { 1, 3, 5, 6 }, 7);
            res2.ShouldBe(4);
            var res3 = SearchInsert(new int[] { 1, 3, 5, 6 }, 0);
            res3.ShouldBe(0);
        }
        private int SearchInsert(int[] nums, int target)
        {
            int lo = 0, hi = nums.Length - 1, pos = nums.Length;
            while (lo <= hi)
            {
                int mid = (lo + hi) / 2;
                if (nums[mid] == target)
                {
                    return mid;
                }
                else if (nums[mid] > target)
                {
                    hi = mid - 1;
                    pos = mid;
                }
                else
                {
                    lo = mid + 1;
                }
            }
            return pos;
        }
        [Fact]
        public void L0038_CountAndSayTest()
        {
            var res1 = CountAndSay(4);
            res1.ShouldBe("1211");
            var res2 = CountAndSay(5);
            res2.ShouldBe("111221");
        }
        private string CountAndSay(int n)
        {
            if (n == 1)
                return "1";
            var chars = CountAndSay(n - 1).ToCharArray();
            char num = '1';
            string res = "";
            for (int i = 1; i < chars.Length; i++)
            {
                if (chars[i] != chars[i - 1])
                {
                    res += num;
                    res += chars[i - 1];
                    num = '1';
                }
                else
                {
                    num++;
                }
            }
            res += num;
            res += chars[chars.Length - 1];
            return res.ToString();
        }
        [Fact]
        public void L0044_IsMatch()
        {
        }
        [Fact]
        public void L0046_Permute()
        {

        }
        private IList<IList<int>> Permute(int[] nums)
        {

            return null;
        }
        private void DfsPermute(int[] nums, Queue<int> toTake, IList<int> tmp, IList<IList<int>> result)
        {
            if (toTake.Count == 0)
            {
                result.Add(tmp);
                return;
            }

            //for (int i = 0; i < nums.Count; i++)
            //{
            //    var curNum = nums.Dequeue();
            //    DfsPermute(nums, curNum, tmp, result);
            //    nums.Enqueue(curNum);
            //}
        }
        [Fact]
        public void L0049_GroupAnagrams()
        {
            var res1 = GroupAnagrams(new string[] { "eat", "tea", "tan", "ate", "nat", "bat" });

        }
        private IList<IList<string>> GroupAnagrams(string[] strs)
        {
            int sum = 0;
            Dictionary<int, List<string>> contains = new Dictionary<int, List<string>>();
            for (int i = 0; i < strs.Length; i++)
            {
                foreach (var c in strs[i].ToCharArray())
                {
                    sum += c;
                }
                if (contains.ContainsKey(sum))
                {
                    contains[sum].Add(strs[i]);
                }
                else
                {
                    contains.Add(sum, new List<string>());
                    contains[sum].Add(strs[i]);
                }
                sum = 0;
            }
            IList<IList<string>> res = new List<IList<string>>();
            foreach (var list in contains)
            {
                res.Add(list.Value);
            }
            return res;
        }
    }
}
