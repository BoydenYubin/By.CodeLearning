using ByLearning.SagaTransitionConfiguration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByLearning.FullfillShoppingCartServices
{
    class Program
    {
        static async Task Main(string[] args)
        {
            #region Seed data
            var customerList = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                customerList.Add(Faker.NameFaker.FemaleFirstName());
            }
            var productList = new List<string>()
            {
                "Apple",
                "Banana",
                "Cake",
                "Dumplings",
                "Eggs"
            };
            var config = new ConfigurationOptions()
            {
                EndPoints = {
                      { GlobalConfiguration.GlobalSettings.RedisServerConfiguration.Server_Address, 
                        GlobalConfiguration.GlobalSettings.RedisServerConfiguration.Server_Port }}
            };
            #endregion
            var database = ConnectionMultiplexer.Connect(config).GetDatabase();
            try
            {
                while (true)
                {
                    string value = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter message (or quit to exit)");
                        Console.Write("> ");
                        return Console.ReadLine();
                    });

                    if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                        break;

                    // 模拟添加数据到Redis数据库
                    if ("stock".Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        database.StringSet("Apple", GlobalConfiguration.GlobalSettings.StockNumber);
                        database.StringSet("Banana", GlobalConfiguration.GlobalSettings.StockNumber);
                        database.StringSet("Cake", GlobalConfiguration.GlobalSettings.StockNumber);
                        database.StringSet("Dumplings", GlobalConfiguration.GlobalSettings.StockNumber);
                        database.StringSet("Eggs", GlobalConfiguration.GlobalSettings.StockNumber);
                        database.StringSet("consumed_Apple", 0);
                        database.StringSet("consumed_Banana", 0);
                        database.StringSet("consumed_Cake", 0);
                        database.StringSet("consumed_Dumplings", 0);
                        database.StringSet("consumed_Eggs", 0);
                        continue;
                    }

                    // 模拟数据
                    var customerName = customerList[Faker.NumberFaker.Number(0, 9)];
                    Console.WriteLine($"current customer name is: {customerName}");
                    var cartItems = new List<HashEntry>();
                    for (int i = 0; i < Faker.NumberFaker.Number(1, 5); i++)
                    {
                        cartItems.Add(new HashEntry(productList[Faker.NumberFaker.Number(0, 5)], Faker.NumberFaker.Number(1, 9)));
                    }

                    var trans = database.CreateTransaction();
                    trans.KeyDeleteAsync(customerName);
                    trans.HashSetAsync(customerName, cartItems.ToArray());
                    trans.ListLeftPushAsync("order_trans", customerName);
                    await trans.ExecuteAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception!!! OMG!!! {0}", ex);
                Console.ReadLine();
            }
        }
    }
}
