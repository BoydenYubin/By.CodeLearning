using ByLearningORM.Util;
using SqlSugar;

namespace ByLearningSqlSugar
{
    public class GetDbClient
    {
        public static ISqlSugarClient GetSugarClient()
        {
            return new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.MySql,
                //数据库名字不能为空
                ConnectionString = GetConfig.GetConnectionString(),
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }
    }
}
