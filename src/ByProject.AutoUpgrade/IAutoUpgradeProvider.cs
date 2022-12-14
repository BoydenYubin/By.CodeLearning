using System.Threading.Tasks;

namespace ByProject.AutoUpgrade
{
    public interface IAutoUpgradeProvider
    {
        Task<UpgradeResult> CheckUpdateAsync();
    }
}
