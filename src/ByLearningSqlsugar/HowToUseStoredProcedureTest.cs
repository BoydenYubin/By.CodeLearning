using ByLearningSqlSugar;
using Xunit;

namespace ByLearningSqlsugar
{
    public class HowToUseStoredProcedureTest
    {
        [Fact]
        public void SimpleUseTest()
        {
            GetDbClient.GetSugarClient().Ado.UseStoredProcedure()
                .GetDataTable("");
        }
    }
}
