namespace ByLearningBullseye
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new RuningFactory();
            var runing = factory.CreateRuning(Towards.EnumerableRuning);
            runing.Run(args);
        }
    }
}
