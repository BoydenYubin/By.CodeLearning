using System;
using System.Linq;

namespace ByLearningThread
{
    class Program
    {
        static void Main(string[] args)
        {
            var testTypes = typeof(Program).Assembly.GetTypes()
                .Where(type => type.GetInterfaces().Contains(typeof(ITestWork))
                            && !type.IsAbstract);
            Console.WriteLine("-------------------------");
            Console.WriteLine("Select the Test case to Run!!");
            int i = 0;
            foreach (var type in testTypes)
            {
                Console.WriteLine($"[{i++}]: {type.Name};");
            }
            Console.WriteLine("-------------------------");
            Console.WriteLine();

            Console.WriteLine($"请输入序号以开始运行测试:0-{testTypes.Count()}!");
            var input = Console.ReadLine();
            int num;
            while (!int.TryParse(input, out num))
            {
                input = Console.ReadLine();
            }
            var testType = testTypes.ToArray()[num];
            var instance = testType.Assembly.CreateInstance(testType.FullName) as ITestWork;
            instance.Run();
        }
    }
}