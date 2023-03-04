using RestSharp;
using Xunit;

namespace ByLearningRestSharp
{
    public class HowToUseTest
    {
        [Fact]
        public async void SimpleGetTest()
        {
            var client = new RestClient("https://www.baidu.com");
            string? resource = null;
            System.Threading.CancellationTokenSource tokenSource = new System.Threading.CancellationTokenSource();
            var request = new RestRequest(resource, method: Method.Get);
            var response = await client.GetAsync(request, tokenSource.Token);
        }
    }
}
