using System;

namespace ByLearningDSA.RedisSource
{
    /// <summary>
    /// 跳跃表层数结构
    /// </summary>
    public class ZSkipListLevel
    {
        /// <summary>
        /// 前进指针
        /// </summary>
        public ZSkipListNode Forward { get; set; }
        /// <summary>
        /// 跳跃跨度
        /// </summary>
        public uint Span { get; set; }
    }
    public class ZSkipListNode
    {
        /// <summary>
        /// 存储数据
        /// </summary>
        public IComparable obj { get; set; }
        /// <summary>
        /// 分值
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// 后退指针
        /// </summary>
        public ZSkipListNode Backward { get; set; }
        /// <summary>
        /// 层数据
        /// </summary>
        public ZSkipListLevel[] Level { get; set; }
    }
    /// <summary>
    /// Redis跳跃表——
    /// C#源码源码实现
    /// </summary>
    public class ZSkipList
    {
        /// <summary>
        /// 跳跃表头节点
        /// </summary>
        public ZSkipListNode Header { get; private set; }
        /// <summary>
        /// 跳跃表尾节点
        /// </summary>
        public ZSkipListNode Tail { get; private set; }
        /// <summary>
        /// 跳跃表层数
        /// </summary>
        public int Level { get; private set; }
        /// <summary>
        /// 跳跃表长度
        /// </summary>
        public ulong Length { get; private set; }
        /// <summary>
        /// 默认最大层数
        /// </summary>
        private const int ZSKIPLIST_MAXLevel = 32;
        /// <summary>
        /// 初始化跳跃表
        /// </summary>
        /// <returns>初始跳跃表</returns>
        public static ZSkipList CreateList()
        {
            int j;
            ZSkipList list = new ZSkipList();
            list.Level = 1;
            list.Length = 0;
            //创建一个层数为ZSKIPLIST_MAXLevel，分数为0，值为空的跳跃表头节点
            list.Header = CreateNode(ZSKIPLIST_MAXLevel, 0, null);
            for (j = 0; j < ZSKIPLIST_MAXLevel; j++)
            {
                list.Header.Level[j].Forward = null;
                list.Header.Level[j].Span = 0;
            }
            list.Header.Backward = null;
            list.Tail = null;
            return list;
        }
        /// <summary>
        /// 获取随机层数
        /// </summary>
        /// <returns>层数</returns>
        private int ZslRandomLevel()
        {
            int Level = 1;
            Random random = new Random((int)DateTime.Now.Ticks);
            while (random.Next(0xFFFF) < 0.25 * 0xFFFF)
            {
                Level += 1;
            }
            return Level < ZSKIPLIST_MAXLevel ? Level : ZSKIPLIST_MAXLevel;
        }
        public void ZslInsert(double score, IComparable obj)
        {
            //记录需要变更跨度层级对应的跳跃表节点
            ZSkipListNode[] update = new ZSkipListNode[ZSKIPLIST_MAXLevel];
            //临时节点
            ZSkipListNode x = new ZSkipListNode();
            //记录各层的跨度
            uint[] rank = new uint[ZSKIPLIST_MAXLevel];
            int i, level;
            //从跳跃表头节点开始查找
            x = this.Header;
            //从最高层循环遍历每层数据
            //记录每层的跨度以及需要变更跨度层级对应的跳跃表节点
            for (i = this.Level - 1; i >= 0; i--)
            {
                //最高层先确定为0，后续增加，否则为上一层的跨度的累加值
                rank[i] = i == (this.Level - 1) ? 0 : rank[i + 1];
                //判断前进节点是否为空并保证前进节点的分值小于要插入节点的分值
                //若分值相等则比较插入对象的先后顺序
                while (x.Level[i].Forward != null && x.Level[i].Forward?.Score < score
                    || (x.Level[i].Forward?.Score == score && x.Level[i].Forward?.obj.CompareTo(obj) < 0))
                {
                    rank[i] += x.Level[i].Span;
                    x = x.Level[i].Forward;
                }
                //记录需要变更跨度层级对应的跳跃表节点
                update[i] = x;
            }
            //利用幂次定律获取随机层数
            level = ZslRandomLevel();
            //当随机的层数超过跳跃表的层数时，修改最高层数
            //让高于层数的层级结构指向跳跃表头节点
            //并将跨度设置为跳跃表的长度
            if (level > this.Level)
            {
                for (i = this.Level; i < level; i++)
                {
                    rank[i] = 0;
                    update[i] = this.Header;
                    update[i].Level[i].Span = (uint)this.Length;
                }
                this.Level = level;
            }
            //根据随机层数、分值和对象创建节点
            x = CreateNode(level, score, obj);
            //从最底层开始循环遍历，更新跳跃表
            //详细解释,可以看对应图解
            for (i = 0; i < level; i++)
            {
                x.Level[i].Forward = update[i].Level[i].Forward;
                update[i].Level[i].Forward = x;
                x.Level[i].Span = update[i].Level[i].Span - (rank[0] - rank[i]);
                update[i].Level[i].Span = rank[0] - rank[i] + 1;
            }
            //当后向节点低于插入节点时
            //虽然前进指针为空，但仍需增加跨度值方便后续操作
            for (i = level; i < this.Level; i++)
            {
                update[i].Level[i].Span++;
            }
            //设置后退指针，若增加的节点位于最后曾更新尾节点
            x.Backward = (update[i] == this.Header) ? null : update[0];
            if (x.Level[0].Forward != null)
            {
                x.Level[0].Forward.Backward = x;
            }
            else
            {
                this.Tail = x;
            }
            //跳跃表长度增加
            this.Length++;
        }
        /// <summary>
        /// 创建一个跳跃表的Node
        /// </summary>
        /// <param name="Level">层数</param>
        /// <param name="score">分值</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        private static ZSkipListNode CreateNode(int Level, double score, IComparable obj)
        {
            ZSkipListNode node = new ZSkipListNode();
            node.obj = obj;
            node.Score = score;
            //根据节点层数创建层级结构数据
            node.Level = new ZSkipListLevel[Level];
            for (int i = 0; i < Level; i++)
            {
                node.Level[i] = new ZSkipListLevel();
            }
            return node;
        }
        public bool ZslDeleteNode(double score, IComparable obj)
        {
            ///同插入节点，先查找相应节点位置
            ZSkipListNode[] update = new ZSkipListNode[32];
            ZSkipListNode x = new ZSkipListNode();
            int i;
            x = this.Header;
            for (i = this.Level - 1; i >= 0; i--)
            {
                while (x.Level[i].Forward != null && x.Level[i].Forward?.Score < score
                    || (x.Level[i].Forward?.Score == score && x.Level[i].Forward?.obj.CompareTo(obj) < 0))
                {
                    x = x.Level[i].Forward;
                }
                update[i] = x;
            }
            x = x.Level[0].Forward;
            //当分值和对象都相同时，删除该节点
            if (x != null && score == x.Score && x.obj == obj)
            {
                ZslDeleteNode(x, update);
                return true;
            }
            return false;
        }
        private void ZslDeleteNode(ZSkipListNode x, ZSkipListNode[] update)
        {
            int i;
            for (i = 0; i < this.Level; i++)
            {
                if (update[i].Level[i].Forward == x)
                {
                    //如果找到该节点，将前一个节点的跨度减1
                    update[i].Level[i].Span += x.Level[i].Span - 1;
                    //前一个节点的前进指针指向被删除的节点的后一个节点，跳过该节点
                    update[i].Level[i].Forward = x.Level[i].Forward;
                }
                else
                {
                    //在第i层没找到，只将该层的最后一个节点的跨度减1
                    update[i].Level[i].Span -= 1;
                }
            }
            //设置后退指针
            if (x.Level[0].Forward != null)
            {   
                //如果被删除的前进节点不为空，后面还有节点
                //就将后面节点的后退指针指向被删除节点x的回退指针
                x.Level[0].Forward.Backward = x.Backward;
            }
            else
            {
                //否则直接将被删除的x节点的后退节点设置为表头的tail指针
                this.Tail = x.Backward;
            }
            //更新跳跃表最大层数
            while (this.Level > 1 && this.Header.Level[this.Level - 1].Forward == null)
                this.Level--;
            //节点计数器减1
            this.Length--;
        }
    }
}
