using ByLearningSqlSugar;
using Shouldly;
using SqlSugar;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;
using NewCoder;
using ByLearningORM.Util;

namespace ByLearningSqlsugar
{
    /// <summary>
    /// 牛客网sql试题测试
    /// </summary>
    public class NewCoderSqlTest
    {

        private SqlSugarClient Db;
        public NewCoderSqlTest()
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.MySql,
                //数据库名字不能为空
                ConnectionString = GetConfig.GetConnectionString(),
                IsAutoCloseConnection = true,
                //AopEvents = new AopEvents()
                //{
                //    OnLogExecuting = (str, para) =>
                //    {
                //        var ss = str;
                //    }
                //}
            });
        }
        /// <summary>
        /// 简单Gropuby查询
        /// https://www.nowcoder.com/practice/ca274ebe6eac40ab9c33ced3f2223bb2?tpId=82&tqId=35084&rp=1&ru=%2Fta%2Fsql&qru=%2Fta%2Fsql%2Fquestion-ranking&tab=answerKey
        /// </summary>
        [Fact]
        public void SQL66Test()
        {
            //如果遇到mysql date/time无法转换为system.datetime时
            //考虑
            //链接MySQL的字符串中添加：Convert Zero Datetime=True 和 Allow Zero Datetime=True两个属性
            var result = Db.Queryable<login>()
                .GroupBy(o => o.user_id)
                .Select(o => new { usr_id = o.user_id, date = SqlFunc.AggregateMax(o.date) })
                .OrderBy(it => it.usr_id);
            var list = result.ToList();
            list.First().usr_id.ShouldBe(2);
            list.Last().usr_id.ShouldBe(3);
        }
        /// <summary>
        /// 复杂查询
        /// https://www.nowcoder.com/practice/7cc3c814329546e89e71bb45c805c9ad?tpId=82&tags=&title=&diffculty=0&judgeStatus=0&rp=1&tab=answerKey
        /// </summary>
        [Fact]
        public void SQL67Test()
        {
            List<int> consume = new List<int>();
            //平均用时350ms
            //耗时较久
            for (int i = 0; i < 20; i++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //先查询登录表中用户及最晚登录时间
                var user_maxDate =
                    Db.Queryable<login>()
                    .GroupBy(l => l.user_id)
                    .Select(l => new { u_id = l.user_id, m_date = SqlFunc.AggregateMax(l.date) }).ToList();
                //使用linq查询合并客户端ID
                var user_cID_maxDate = user_maxDate.Join(Db.Queryable<login>().ToList(),
                    a => new { id = a.u_id, date = a.m_date },
                    b => new { id = b.user_id, date = b.date },
                    (inner, outter) => new { inner.u_id, outter.client_id, inner.m_date });
                //联表查询获得最终结果
                var result = user_cID_maxDate.Join(Db.Queryable<user>().ToList(),
                    l => l.u_id,
                    r => r.id,
                    (l, r) => new { r.name, l.client_id, l.m_date })
                    .Join(Db.Queryable<client>().ToList(),
                    l => l.client_id,
                    r => r.id,
                    (l, r) => new { l.name, client_name = r.name, l.m_date })
                    .OrderBy(it => it.name);
                sw.Stop();
                consume.Add((int)sw.ElapsedMilliseconds);
            }
            //直接使用原生sql语句
            var sql = @"
             select u.name u_n, c.name c_n, t1.m_date
             from
             (
                 select l1.user_id, l1.m_date,l2.client_id
                 from(select user_id, max(date) m_date from login group by user_id) l1
                 join login l2
                 on l1.user_id = l2.user_id
                 and l1.m_date = l2.date
             ) as t1
             join user u
             on t1.user_id = u.id
             join client c
             on t1.client_id = c.id
             order by u_n;
             ";

            //执行原生语句比较快
            var tableresult = Db.Ado.GetDataTable(sql);

            //linq的join查询方法
            //当join需要匹配多个键时
            //动态类型的名称要一致
            /*  写法一
            var newresult = from s in result
                         join login in Db.Queryable<login>().ToList()
                         on new { id = s.u_id, date = s.m_date } equals new { id =login.user_id, date = login.date }
                         select new { s.u_id, login.client_id, s.m_date };
            */

            /*
              写法二
                var result = result.Join(Db.Queryable<login>().ToList(), 
                a => new { id = a.u_id, date = a.m_date },
                b => new { id = b.id, date = b.date }, 
                (inner, outter) => new { inner.u_id, outter.client_id, inner.m_date });
             */
        }
    }
}
