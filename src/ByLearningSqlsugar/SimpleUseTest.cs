using System;
using Xunit;
using SqlSugar;
using ByLearningSqlSugar;
using ByLearningORM.Util;

namespace ByLearningSqlsugar
{
    public class SimpleUseTest
    {
        private SqlSugarClient Db;
        public SimpleUseTest()
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.MySql,
                //数据库名字不能为空
                ConnectionString = GetConfig.GetConnectionString(),
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            }); ;
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        [Fact]
        public void ConnectToMysqlTest()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.MySql,
                //数据库名字不能为空
                ConnectionString = GetConfig.GetConnectionString(),
                IsAutoCloseConnection = true
            });
            //创建数据库需要高级用户，有相应权限
            //db.DbMaintenance.CreateDatabase("learnsqlsugar");
        }
        [Fact]
        public void DbFirstTest()
        {
            Db.DbFirst.CreateClassFile(@"D:\OneDrive\OneDrive - mail.scut.edu.cn\boyden's\.NetCore\By.LearningDays\ByLearningSqlsugar\DbFirstClass\", "NewCoder");
        }
        /// <summary>
        /// 使用codefirst观察datetime类型被转换成哪种类型
        /// </summary>
        [SugarTable("with_datetime")]
        public class TableWithDateTime
        {
            [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "id")]
            public int Id { get; set; }
            [SugarColumn(ColumnDataType = "Date", ColumnName = "date")]
            public DateTime date { get; set; }
        }
        /// <summary>
        /// 测试发现5.0.2版本的不会报"IL反射错误"错
        /// 建议使用高版本
        /// </summary>
        [Fact]
        public void DateTimeColumnTest()
        {
            Db.CodeFirst.InitTables(typeof(TableWithDateTime));
        }
    }
}
