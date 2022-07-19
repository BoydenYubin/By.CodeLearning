using ByLearningDapper.BaseClass;
using ByLearningORM.Util;
using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace ByLearningDapper
{
    public class SimpleUseTest
    {
        [Fact]
        public void TryToConnectTest()
        {
            var connectionString = GetConfig.GetConnectionString();
            IDbConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            var delrows = connection.Execute("truncate table dapperuser");


            var list = new List<DapperUser>();
            for (int i = 1; i <= 100; i++)
            {
                list.Add(new DapperUser()
                {
                    ID = i,
                    Name = $"user:{i}",
                    Email = $"rand_user{i}@rand.com"
                });
            }
            connection.Execute("insert into dapperuser(id, name, email) value (@ID, @Name, @Email)",
                list, commandTimeout: 100, commandType: CommandType.Text);

            var users = connection.Query<DapperUser>("select * from dapperuser");
        }
    }
}
