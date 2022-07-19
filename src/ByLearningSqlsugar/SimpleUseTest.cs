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
                //���ݿ����ֲ���Ϊ��
                ConnectionString = GetConfig.GetConnectionString(),
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            }); ;
        }
        /// <summary>
        /// �������ݿ�
        /// </summary>
        [Fact]
        public void ConnectToMysqlTest()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.MySql,
                //���ݿ����ֲ���Ϊ��
                ConnectionString = GetConfig.GetConnectionString(),
                IsAutoCloseConnection = true
            });
            //�������ݿ���Ҫ�߼��û�������ӦȨ��
            //db.DbMaintenance.CreateDatabase("learnsqlsugar");
        }
        [Fact]
        public void DbFirstTest()
        {
            Db.DbFirst.CreateClassFile(@"D:\OneDrive\OneDrive - mail.scut.edu.cn\boyden's\.NetCore\By.LearningDays\ByLearningSqlsugar\DbFirstClass\", "NewCoder");
        }
        /// <summary>
        /// ʹ��codefirst�۲�datetime���ͱ�ת������������
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
        /// ���Է���5.0.2�汾�Ĳ��ᱨ"IL�������"��
        /// ����ʹ�ø߰汾
        /// </summary>
        [Fact]
        public void DateTimeColumnTest()
        {
            Db.CodeFirst.InitTables(typeof(TableWithDateTime));
        }
    }
}
