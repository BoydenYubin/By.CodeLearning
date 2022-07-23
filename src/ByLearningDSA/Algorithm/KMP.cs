using Shouldly;
using Xunit;

namespace ByLearningDSA.Algorithm
{
    /// <summary>
    /// 参考网址
    /// http://www.ruanyifeng.com/blog/2013/05/Knuth%E2%80%93Morris%E2%80%93Pratt_algorithm.html
    /// </summary>
    public class KMP
    {
        [Fact]
        public void GetNextArrayTest()
        {
            var test = new char[] { 'a', 'b', 'c', 'a', 'b', 'd' };
            var result = new int[] { 0, 0, 0, 1, 2, 0 };
            result.ShouldBe(GetNextArray(test));
            test = "ABCDABD".ToCharArray();
            result = new int[] { 0, 0, 0, 0, 1, 2, 0 };
            result.ShouldBe(GetNextArray(test));
            test = "ABACDEFABACDEG".ToCharArray();
            result = new int[] { 0, 0, 1, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 0 };
            result.ShouldBe(GetNextArray(test));
        }
        /// <summary>
        /// 获取匹配字符串的next数组
        /// </summary>
        /// <param name="arrays"></param>
        /// <returns></returns>
        private int[] GetNextArray(char[] arrays)
        {
            if (arrays == null || arrays.Length == 0)
            {
                return new int[0];
            }
            int[] next = new int[arrays.Length];
            next[0] = 0;
            int uIndex = 1;   //模式串的上游标
            int dIndex = 0;   //Next数组的游标
            //限定条件
            while (uIndex < arrays.Length)
            {
                //如果相等则下标同时+1
                if (arrays[uIndex] == arrays[dIndex])
                {
                    next[uIndex] = dIndex + 1;
                    uIndex++;
                    dIndex++;
                }
                //否认确定下标是否大于0
                else if (dIndex > 0)
                {
                    dIndex = next[dIndex - 1];
                }
                //等于0 则直接赋值0
                else if (dIndex == 0)
                {
                    next[uIndex] = 0;
                    uIndex++;
                }
            }
            return next;
        }

        [Theory]
        [InlineData("BBC ABCDAB ABCDABCDABDE", "ABCDABD", 15)]
        [InlineData("ABCFDE", "CFD", 2)]
        [InlineData("ABCDEFGHIJK", "ABCE", -1)]
        [InlineData("abaabdfg", "baabgh", -1)]
        [InlineData("abaabdfbaabghg", "baabgh", 7)]
        public void FindStrTest(string origin, string findstr, int index)
        {
            FindString(origin, findstr).ShouldBe(index);
        }
        /// <summary>
        /// 返回索引
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private int FindString(string origin, string pattern)
        {
            int[] next = GetNextArray(pattern.ToCharArray());
            int i = 0, j = 0;
            while (i < origin.Length && j < pattern.Length)
            {
                //匹配相同时，上下字符串同时+1
                if (origin[i] == pattern[j])
                {
                    i++;
                    j++;
                }
                //不相同时，如果匹配下标大于0，则移动
                //移动位数 = 已匹配的字符数 - 对应的部分匹配值
                else if (j > 0)
                {
                    j = next[j - 1];
                }
                //否则直接等于0
                else if (j == 0)
                {
                    i++;
                }
            }
            if (j >= pattern.Length)
            {
                return i - j;
            }
            else
            {
                return -1;
            }
        }

    }
}
